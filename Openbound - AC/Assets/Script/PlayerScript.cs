using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{

    Rigidbody2D rb;
    PlayerInput playerInput;
    PlayerActionInput playerActionInput;

    public Stats playerStats;

    public float regularSpeed = 250f;
    public float sprintSpeed = 300;
    [SerializeField] private float currentSpeed;
    public bool sprintBool = false;

    public string interactableString = "";
    DialogueTrigger dialogueTrigger;

    private bool interactPressed = false;
    [SerializeField] private bool submitPressed = false;

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    string currentAnimaton;

    [SerializeField] bool mirror;
    [SerializeField] bool isIDLE;
    [SerializeField] bool isSpriting;
    [SerializeField] int direction = 2;
    /*
     *NUMPAD DIRECTIONS
     *  2 = FORWARD
     *  3 = FOWARD RIGHT
     *  6 = RIGHT
     *  9 = BACKWARD RIGHT
     *  8 = BACKWARD
     *  7 = BACKWARD LEFT
     *  4 = LEFT
     *  1 = FORWARD LEFT
     *  
     */

    /*
    const string IDLE_FORWARD = "IDLE_FORWARD";
    const string IDLE_BACKWARD = "IDLE_BACKWARD";
    const string IDLE_SIDE = "IDLE_SIDE";
    const string IDLE_FORWARD_D = "IDLE_FORWARD_DIAGONAL";
    const string IDLE_BACKWARD_D = "IDLE_BACKWARD_DIAGONAL";

    const string WALK_FORWARD = "WALK_FORWARD";
    const string WALK_BACKWARD = "WALK_BACKWARD";
    const string WALK_SIDE = "WALK_SIDE";
    const string WALK_FORWARD_D = "WALK_FORWARD_DIAGONAL";
    const string WALK_BACKWARD_D = "WALK_BACKWARD_DIAGONAL";

    const string RUN_FORWARD = "RUN_FORWARD";
    const string RUN_BACKWARD = "RUN_BACKWARD";
    const string RUN_SIDE = "RUN_SIDE";
    const string RUN_FORWARD_D = "RUN_FORWARD_DIAGONAL";
    const string RUN_BACKWARD_D = "RUN_BACKWARD_DIAGONAL";
    */

    string[] IDLE_STRINGS = {
        "IDLE_FORWARD",
        "IDLE_BACKWARD",
        "IDLE_SIDE",
        "IDLE_FORWARD_DIAGONAL",
        "IDLE_BACKWARD_DIAGONAL"
    };

    string[] WALK_STRINGS = {
        "WALK_FORWARD",
        "WALK_BACKWARD",
        "WALK_SIDE",
        "WALK_FORWARD_DIAGONAL",
        "WALK_BACKWARD_DIAGONAL" 
    };

    string[] RUN_STRINGS = {
        "RUN_FORWARD",
        "RUN_BACKWARD",
        "RUN_SIDE",
        "RUN_FORWARD_DIAGONAL",
        "RUN_BACKWARD_DIAGONAL",
    };

    string[] currentStringArray;

    public Vector2 debugDirectionCoord;

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

        //ChangeAnimationState("WALK_BACKWARD");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        //SetAnimationStatus();

        //Sprint
        if (sprintBool)
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = regularSpeed;
        }

        //Movement
        Vector2 inputVector = playerActionInput.Player.Movement.ReadValue<Vector2>();

        debugDirectionCoord = rb.velocity;

        rb.velocity = inputVector * currentSpeed * Time.fixedDeltaTime;
        //Debug.Log(rb.velocity);

        SetAnimationStatus();
    }

    private void LateUpdate()
    {
        //SetAnimationStatus();
    }

    void SetAnimationStatus()
    {
        //IDLE
        if (rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            isIDLE = true;
        }
        else
        {
            isIDLE = false;

            if (Mathf.Abs(rb.velocity.x) > 5 || Mathf.Abs(rb.velocity.y) > 5)
            {
                isSpriting = true;
            }
            else
            {
                isSpriting = false;
            }

            if (rb.velocity.x == 0 && rb.velocity.y < 0)
            {
                //FORWARD
                direction = 2;
            }
            else if (rb.velocity.x == 0 && rb.velocity.y > 0)
            {
                //BACKWARD
                direction = 8;
            }
            else if (rb.velocity.x > 0 && rb.velocity.y == 0)
            {
                //RIGHT
                direction = 6;
            }
            else if (rb.velocity.x < 0 && rb.velocity.y == 0)
            {
                //LEFT
                direction = 4;
            }
            else if (rb.velocity.x > 0 && rb.velocity.y < 0)
            {
                //FORWARD RIGHT
                direction = 3;
            }
            else if (rb.velocity.x < 0 && rb.velocity.y < 0)
            {
                //FORWAD LEFT
                direction = 1;
            }
            else if (rb.velocity.x > 0 && rb.velocity.y > 0)
            {
                //BACKWARD RIGHT
                direction = 9;
            }
            else if (rb.velocity.x < 0 && rb.velocity.y > 0)
            {
                //BACKWARD LEFT
                direction = 7;
            }
        }

        if (isIDLE)
            currentStringArray = IDLE_STRINGS;
        else if (!isIDLE && !isSpriting)
            currentStringArray = WALK_STRINGS;
        else if (isSpriting)
            currentStringArray = RUN_STRINGS;

        switch (direction)
        {
            case 2:
                ChangeAnimationState(currentStringArray[0]);
                mirror = false;
                break;
            case 3:
                ChangeAnimationState(currentStringArray[3]);
                mirror = false;
                break;
            case 6:
                ChangeAnimationState(currentStringArray[2]);
                mirror = false;
                break;
            case 9:
                ChangeAnimationState(currentStringArray[4]);
                mirror = false;
                break;
            case 8:
                ChangeAnimationState(currentStringArray[1]);
                mirror = false;
                break;
            case 7:
                ChangeAnimationState(currentStringArray[4]);
                mirror = true;
                break;
            case 4:
                ChangeAnimationState(currentStringArray[2]);
                mirror = true;
                break;
            case 1:
                ChangeAnimationState(currentStringArray[3]);
                mirror = true;
                break;
            default:
                ChangeAnimationState(currentStringArray[0]);
                mirror = false;
                break;
        }

        spriteRenderer.flipX = mirror;
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

    public void SubmitPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            submitPressed = true;
        }
        else if (context.canceled)
        {
            submitPressed = false;
        }
    }

    public void GUISubmitPressed()
    {
        submitPressed = true;
    }

    public bool GetSubmitPressed()
    {
        bool result = submitPressed;
        submitPressed = false;
        return result;
    }

    public void RegisterSubmitPressed()
    {
        submitPressed = false;
    }

    public void InteractButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactPressed = true;
        }
        else if (context.canceled)
        {
            interactPressed = false;
        }
    }

    public bool GetInteractPressed()
    {
        bool result = interactPressed;
        interactPressed = false;
        return result;
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

    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
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

            if(col.gameObject.GetComponent<DialogueTrigger>() != null)
            {
                dialogueTrigger = col.gameObject.GetComponent<DialogueTrigger>();
            }
        }
    }

}
