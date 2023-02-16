using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    PlayerScript playerScript;
    Animator playerAnimator;

    [SerializeField] RuntimeAnimatorController[] characterAnimators;

    private void Awake()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");

        playerScript = playerGO.GetComponent<PlayerScript>();
        playerAnimator = playerGO.GetComponentInChildren<Animator>();

        playerAnimator.runtimeAnimatorController = characterAnimators[GameManager.Instance.GetCharacterIndex()];
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
