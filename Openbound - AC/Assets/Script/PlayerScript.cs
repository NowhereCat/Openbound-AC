using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{

    Rigidbody2D rb;
    PlayerInput playerInput;
    PlayerActionInput playerActionInput;

    public float regularSpeed = 250f;
    public float sprintSpeed = 300;
    [SerializeField] private float currentSpeed;
    public bool sprintBool = false;

    public string interactableString = "";

    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        playerActionInput = new PlayerActionInput();
        playerActionInput.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //Sprint
        if(sprintBool)
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = regularSpeed;
        }

        //Movement
        Vector2 inputVector = playerActionInput.Player.Movement.ReadValue<Vector2>();

        rb.velocity = inputVector * currentSpeed * Time.fixedDeltaTime;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        Debug.Log("Interacting | " + context.phase);
    }

    public void ToggleSpeed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            sprintBool = true;
        }

        if (context.canceled)
        {
            sprintBool = false;
        }
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameState currentGameState = GameStateManager.Instance.CurrentGameState;
            GameState newGameState = currentGameState == GameState.Gameplay
                ? GameState.Paused
                : GameState.Gameplay;

            GameStateManager.Instance.SetState(newGameState);
        }
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        Debug.Log("THIS SHOULD WORK");
        enabled = newGameState == GameState.Gameplay;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Interactable")
        {
            interactableString = col.name;
        }
    }

}
