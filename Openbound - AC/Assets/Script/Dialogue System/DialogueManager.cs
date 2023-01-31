using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEditor.TextCore.Text;

public class DialogueManager : MonoBehaviour
{
    PlayerScript playerScript;

    [Header("Params")]
    [SerializeField] float typingSpeed = 0.04f;
    
    [Header("Load Globals JSON")]
    [SerializeField] TextAsset loadGlobasJSON;
    
    [Header("Dialogue UI")]
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] GameObject continueIcon;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI displayNameText1;
    [SerializeField] TextMeshProUGUI displayNameText2;
    [SerializeField] GameObject[] dialoguePanels;
    //[SerializeField] Animator portraitAnimator;
    //Animator layoutAnimator;

    [Header("Choices UI")]
    [SerializeField] GameObject[] choices;
    TextMeshProUGUI[] choicesText;

    [Header("Profiles")]
    public CharacterProfile[] characterProfiles;

    [Header("Audio")]
    [SerializeField] DialogueAudioInfoSO defaultAudioInfo;
    [SerializeField]
    DialogueAudioInfoSO[] audioInfos;
    DialogueAudioInfoSO currentAudioInfo;

    [SerializeField] bool makePredictable;

    Dictionary<string, DialogueAudioInfoSO> audioInfoDictionary;

    [SerializeField] AudioMixerGroup audioMixerGroupChatter;
    AudioSource audioSource;
    
    Story currentStory;

    public bool dialogueIsPlaying { get; private set; }

    bool cancontinueToNextLine = false;

    Coroutine displayLineCoroutine;

    private static DialogueManager instance;

    const string SPEAKER_TAG = "speaker";
    const string SIDE_TAG = "side";
    const string PORTRAIT_TAG = "portrait";
    const string LAYOUT_TAG = "layout";
    const string AUDIO_TAG = "audio";

    DialogueVariables dialogueVariables;    

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in this scene!");
        }
        instance = this;

        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();

        dialogueVariables = new DialogueVariables(loadGlobasJSON);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = audioMixerGroupChatter;
        currentAudioInfo = defaultAudioInfo;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        //layoutAnimator = dialoguePanel.GetComponent<Animator>();

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach(GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }

        InitializeAudioInfoDictionary();

    }
    
    void InitializeAudioInfoDictionary()
    {
        audioInfoDictionary = new Dictionary<string, DialogueAudioInfoSO>();
        audioInfoDictionary.Add(defaultAudioInfo.id, defaultAudioInfo);
        foreach (DialogueAudioInfoSO audioInfo in audioInfos)
        {
            audioInfoDictionary.Add(audioInfo.id, audioInfo);
        }
    }

    void SetCurrentAudioInfo(string id)
    {
        DialogueAudioInfoSO audioInfo = null;
        audioInfoDictionary.TryGetValue(id, out audioInfo);
        if(audioInfo != null)
        {
            currentAudioInfo = audioInfo;
        }
        else
        {
            Debug.LogWarning("Failed to find audio info for id: " + id);
        }
    }
    
    private void Update()
    {
        if(!dialogueIsPlaying)
        {
            return;
        }

        if (cancontinueToNextLine &&
            currentStory.currentChoices.Count == 0 
            && playerScript.GetSubmitPressed()) {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        dialogueVariables.StartListening(currentStory);

        displayNameText1.text = "???";
        //portraitAnimator.Play("default");
        //layoutAnimator.Play("right");

        ContinueStory();
    }

    IEnumerator ExitDialogueMode()
    {

        yield return new WaitForSeconds(0.2f);

        dialogueVariables.StopListening(currentStory);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        SetCurrentAudioInfo(defaultAudioInfo.id);
    }

    void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if(displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }

            string nextLine = currentStory.Continue();
            //DisplayChoices();

            HandleTags(currentStory.currentTags);

            displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));
            
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    IEnumerator DisplayLine(string line)
    {
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;

        continueIcon.SetActive(false);
        HideChoices();

        cancontinueToNextLine = false;

        bool isAddingRichTextTag = false;

        foreach (char letter in line.ToCharArray())
        {
            if (playerScript.GetSubmitPressed())
            {
                dialogueText.maxVisibleCharacters = line.Length;
                break;
            }

            if(letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;
                if(letter == '>')
                {
                    isAddingRichTextTag = false;
                }
            }
            else
            {
                PlayDialogueSound(dialogueText.maxVisibleCharacters, dialogueText.text[dialogueText.maxVisibleCharacters]);
                dialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
            }

            
        }
        continueIcon.SetActive(true);
        DisplayChoices();

        cancontinueToNextLine = true;
    }
    
    void PlayDialogueSound(int currentDisplayedCharacterCount, char currentCharacter)
    {
        AudioClip[] dialogueTypingSoundClips = currentAudioInfo.dialogueTypingSoundClips;
        int frequenceLevel = currentAudioInfo.frequenceLevel;
        float minPitch = currentAudioInfo.minPitch;
        float maxPitch = currentAudioInfo.maxPitch;
        bool stopAudioSource = currentAudioInfo.stopAudioSource;

        if (currentDisplayedCharacterCount%frequenceLevel == 0)
        {
            if (stopAudioSource)
            {
                audioSource.Stop();
            }

            AudioClip soundClip = null;

            if (makePredictable)
            {
                int hashCode = currentCharacter.GetHashCode();

                int predictableIndex = hashCode % dialogueTypingSoundClips.Length;
                //Debug.Log("Pred Index" + predictableIndex);
                soundClip = dialogueTypingSoundClips[predictableIndex];

                int minPitchInt = (int)(minPitch * 100);
                int maxPitchInt = (int)(maxPitch * 100);
                int pitchRangeInt = maxPitchInt - minPitchInt;

                if(pitchRangeInt != 0)
                {
                    int predictablePitchInt = (hashCode % pitchRangeInt) + minPitchInt;
                    float predictablePitch = predictablePitchInt / 100f;
                    audioSource.pitch = predictablePitch;
                }
                else
                {
                    audioSource.pitch = minPitch;
                }
            }
            else
            {
                int randomIndex = Random.Range(0, dialogueTypingSoundClips.Length);
                soundClip = dialogueTypingSoundClips[randomIndex];

                //Debug.Log("Random Index: " + randomIndex);

                audioSource.pitch = Random.Range(minPitch, maxPitch);
            }

            audioSource.PlayOneShot(soundClip);
        }
    }
    
    void HideChoices()
    {
        foreach (GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }

    void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if(splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    GetCharacterTags(tagValue);
                    displayNameText1.text = tagValue;
                    break;
                case SIDE_TAG:
                    Debug.Log("WHICH SIDE TO: " + tagValue);
                    break;
                case PORTRAIT_TAG:
                    //portraitAnimator.Play(tagValue);
                    break;
                case LAYOUT_TAG:
                    //layoutAnimator.Play(tagValue);
                    break;
                case AUDIO_TAG:
                    SetCurrentAudioInfo(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }

    void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if(currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: " + currentChoices.Count);
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        if (cancontinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            playerScript.RegisterSubmitPressed();
            ContinueStory();
        }
    }

    public Ink.Runtime.Object GetVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.variables.TryGetValue(variableName, out variableValue);
        if(variableValue == null)
        {
            Debug.LogWarning("Ink Variable was found to be null: " + variableName);
        }
        return variableValue;
    }

    void GetCharacterTags(string id)
    {
        Debug.Log("TAG: " + id);

        foreach(CharacterProfile profile in characterProfiles)
        {
            if(profile.characterID == id)
            {
                Debug.Log(profile.characterID);
                Debug.Log(profile.characterName);
                Debug.Log(profile.animator.name);
                Debug.Log(profile.primaryColor);
                Debug.Log(profile.secondaryColor);
                Debug.Log(profile.fontColor);
                //Debug.Log(profile.dialogueASO.id);
                SetCurrentAudioInfo(profile.dialogueASO.id);
                SetUIStyle(profile.primaryColor, profile.fontColor);
            }
        }
    }

    void SetUIStyle(Color32 mainColor, Color32 fontColor)
    {
        continueIcon.GetComponent<Image>().color = mainColor;
        continueIcon.GetComponentInChildren<TextMeshProUGUI>().color = fontColor;

        foreach (GameObject item in dialoguePanels)
        {
            item.GetComponent<Image>().color = mainColor;
        }

        displayNameText1.color = fontColor;
        displayNameText2.color = fontColor;

        dialogueText.color = fontColor;
    }

    public void OnApplicationQuit()
    {
        if (dialogueVariables != null)
        {
            dialogueVariables.SaveVariables();
        }
    }

    public static DialogueManager GetInstace()
    {
        return instance;
    }
}
