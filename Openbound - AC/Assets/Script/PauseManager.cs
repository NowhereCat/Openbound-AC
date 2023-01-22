using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{

    static PauseManager _instance;

    public GameState CurrentGameState;
    public static PauseManager Instance
    {
        get {
            if (_instance is null)
                Debug.LogError("PAUSE MANAGER IS NULL");

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        CurrentGameState = GameState.Gameplay;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGameState(GameState newState)
    {
        CurrentGameState = newState;

        if(CurrentGameState == GameState.Paused)
        {
            Time.timeScale = 0f;
        }else if(CurrentGameState == GameState.Gameplay)
        {
            Time.timeScale = 1f;
        }
    }

}
