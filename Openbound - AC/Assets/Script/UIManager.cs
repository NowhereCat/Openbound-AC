using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.TextCore.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    PlayerScript player;

    [Header("TAB MENU")]
    public GameObject tabMenuPanel;
    public float[] tabMenuPositions;
    public float tabMenuAnimDuration = 10f;
    bool tabMenu = false;
    bool showTabMenu = true;

    [Header("LOCATION PANEL")]
    public RectTransform locationPanel;
    public float[] locationPositions;
    public float locationPanelDuration = 10f;

    [Header("Settings Panel")]
    public GameObject settingsPanel;
    [SerializeField] bool showSettings = false;

    [Header("Stats Panel")]
    public GameObject statsPanel;
    [SerializeField] bool showStats = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();

        ToggleTabMenu(true);

        StartCoroutine(ShowLocation(true));

        UpdateStatusUI();
    }

    // Update is called once per frame
    void Update()
    {
        settingsPanel.SetActive(showSettings);
    }

    public void ToggleTabMenu(bool onlyCheck = false)
    {

        ShowTabMenu(true);

        if (!onlyCheck) tabMenu = !tabMenu;

        if (tabMenu)
        {
            tabMenuPanel.transform.LeanMoveX(tabMenuPositions[0], tabMenuAnimDuration);
        }
        else
        {
            tabMenuPanel.transform.LeanMoveX(tabMenuPositions[1], tabMenuAnimDuration);
        }
    }

    void ShowTabMenu(bool forceShow = false)
    {
        if (!forceShow)
        {
            showTabMenu = !showTabMenu;
        }
        else
        {
            showTabMenu = true;
        }

        tabMenuPanel.SetActive(showTabMenu);
    }

    public IEnumerator ShowLocation(bool _bool)
    {
        if (_bool)
        {
            locationPanel.transform.LeanMoveY(locationPositions[0], locationPanelDuration/3).setOnComplete(() => { StartCoroutine(ShowLocation(false)); });
        }
        else
        {
            yield return new WaitForSeconds(locationPanelDuration/3);
            locationPanel.transform.LeanMoveY(locationPositions[1], locationPanelDuration / 3);
        }
    }

    public void ToggleStatsPanel()
    {
        showStats = !showStats;
        ShowTabMenu();

        statsPanel.SetActive(showStats);
    }

    public void ToggleSettingsPanel()
    {
        showSettings = !showSettings;
    }


    void UpdateStatusUI()
    {
        GameObject superiorLeft = statsPanel.transform.Find("Superior Left").gameObject;
        superiorLeft.GetComponent<Image>().color = player.playerStats.characterProfile.secondaryColor;

        Image profileImage = superiorLeft.transform.Find("Profile Image").GetComponent<Image>();
        TextMeshProUGUI nameText = superiorLeft.transform.Find("Panel/Character Name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI handleText = superiorLeft.transform.Find("Panel/Handle").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI titleText = superiorLeft.transform.Find("Panel/Title").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI ageText = superiorLeft.transform.Find("Panel/Age").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI casteText = superiorLeft.transform.Find("Panel/Caste").GetComponent<TextMeshProUGUI>();
        Image signImage = superiorLeft.transform.Find("Panel 2/Sign Image").GetComponent<Image>();
        TextMeshProUGUI signText = superiorLeft.transform.Find("Panel 2/Sign Text").GetComponent<TextMeshProUGUI>();

        profileImage.sprite = player.playerStats.characterIcon;
        nameText.text = "NAME: " + player.playerStats.fullName;
        handleText.text = "HANDLE: " + player.playerStats.chatHandle;
        titleText.text = "TITLE: " + player.playerStats.title;
        ageText.text = "AGE: " + player.playerStats.age;
        casteText.text = "CASTE: " + player.playerStats.caste;
        signImage.sprite = player.playerStats.signSprite;
        signText.text = player.playerStats.sign.ToUpper();


        GameObject superiorRight = statsPanel.transform.Find("Superior Right").gameObject;
        superiorRight.GetComponent<Image>().color = player.playerStats.characterProfile.secondaryColor;

        List<GameObject> statSections = new List<GameObject>();

        for (int i = 0; i < superiorRight.transform.GetChild(0).childCount; i++)
        {
            statSections.Add(superiorRight.transform.GetChild(0).GetChild(i).gameObject);
        }

        statSections[0].GetComponentInChildren<TextMeshProUGUI>().text = "POWER LEVEL: " + player.playerStats.powerLevel;
        statSections[0].GetComponentInChildren<Slider>().value= player.playerStats.powerLevel;
        statSections[0].transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>().color = player.playerStats.characterProfile.primaryColor;

        statSections[1].GetComponentInChildren<TextMeshProUGUI>().text = "INTELLIGENCE: " + player.playerStats.intelligence;
        statSections[1].GetComponentInChildren<Slider>().value = player.playerStats.intelligence;
        statSections[1].transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>().color = player.playerStats.characterProfile.primaryColor;

        statSections[2].GetComponentInChildren<TextMeshProUGUI>().text = "WISDOM: " + player.playerStats.wisdom;
        statSections[2].GetComponentInChildren<Slider>().value = player.playerStats.wisdom;
        statSections[2].transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>().color = player.playerStats.characterProfile.primaryColor;

        statSections[3].GetComponentInChildren<TextMeshProUGUI>().text = "VITALITY: " + player.playerStats.vitality;
        statSections[3].GetComponentInChildren<Slider>().value = player.playerStats.vitality;
        statSections[3].transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>().color = player.playerStats.characterProfile.primaryColor;

        statSections[4].GetComponentInChildren<TextMeshProUGUI>().text = "SPEED: " + player.playerStats.speed;
        statSections[4].GetComponentInChildren<Slider>().value = player.playerStats.speed;
        statSections[4].transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>().color = player.playerStats.characterProfile.primaryColor;

        statSections[5].GetComponentInChildren<TextMeshProUGUI>().text = "PRECISION: " + player.playerStats.precision;
        statSections[5].GetComponentInChildren<Slider>().value = player.playerStats.precision;
        statSections[5].transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>().color = player.playerStats.characterProfile.primaryColor;

        statSections[6].GetComponentInChildren<TextMeshProUGUI>().text = "CHARISMA: " + player.playerStats.charisma;
        statSections[6].GetComponentInChildren<Slider>().value = player.playerStats.charisma;
        statSections[6].transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>().color = player.playerStats.characterProfile.primaryColor;

        statSections[7].GetComponentInChildren<TextMeshProUGUI>().text = "SPIRIT: " + player.playerStats.spirit;
        statSections[7].GetComponentInChildren<Slider>().value = player.playerStats.spirit;
        statSections[7].transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>().color = player.playerStats.characterProfile.primaryColor;


        GameObject inferior = statsPanel.transform.Find("Inferior").gameObject;
        inferior.GetComponent<Image>().color = player.playerStats.characterProfile.secondaryColor;

        List<GameObject> strifeSections = new List<GameObject>();

        for (int i = 0; i < inferior.transform.GetChild(0).childCount; i++)
        {
            strifeSections.Add(inferior.transform.GetChild(0).GetChild(i).gameObject);
            
            if(i < player.playerStats.strifedecks.Length)
            {
                strifeSections[i].SetActive(true);
                strifeSections[i].GetComponentInChildren<Image>().sprite = player.playerStats.strifedecks[i].strifeIcon;
                strifeSections[i].GetComponentInChildren<TextMeshProUGUI>().text = player.playerStats.strifedecks[i].strifeName;
            }
            else
            {
                strifeSections[i].SetActive(false);
            }
        }



    }

}
