using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;

    [SerializeField] int characterIndex;

    public static GameManager Instance
    {
        get
        {
            if (_instance is null)
                Debug.LogError("Game Manager is NULL");

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public void SetCharacterIndex(int characterIndex)
    {
        this.characterIndex = characterIndex;
    }

    public int GetCharacterIndex()
    {
        return characterIndex;
    }

}
