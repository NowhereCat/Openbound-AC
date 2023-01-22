using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    PlayerScript playerScript;
    //[Header("Visual Cue")]
    //[SerializeField] GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] TextAsset inkJSON;

    bool playerInRange;

    private void Awake()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerInRange = false;
        //visualCue.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && !DialogueManager.GetInstace().dialogueIsPlaying)
        {
            //visualCue.SetActive(true);
            if (playerScript.GetInteractPressed())
            {
                DialogueManager.GetInstace().EnterDialogueMode(inkJSON);
            }
        }
        else
        {
            //visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
