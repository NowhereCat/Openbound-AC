using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    PlayerScript playerScript;
    Animator playerAnimator;

    [SerializeField] Stats[] characterStats;

    private void Awake()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");

        playerScript = playerGO.GetComponent<PlayerScript>();
        playerAnimator = playerGO.GetComponentInChildren<Animator>();

        playerScript.playerStats = characterStats[GameManager.Instance.GetCharacterIndex()];
        playerAnimator.runtimeAnimatorController = characterStats[GameManager.Instance.GetCharacterIndex()].runtimeAnimatorController;

        InventoryManager._instance.SetPresetInventory(characterStats[GameManager.Instance.GetCharacterIndex()].GetPresetInventory());
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
