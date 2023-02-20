using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    PlayerScript playerScript;

    [SerializeField] ItemSO item;
    [SerializeField] bool itemGiven = false;
    bool playerInRange = false;

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    private void Update()
    {
        if(playerInRange && !itemGiven)
        {
            if (playerScript.GetInteractPressed())
            {
                itemGiven = true;
                AddItem();
            }
        }
    }

    void AddItem()
    {
        InventoryManager._instance.AddItem(item);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
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
