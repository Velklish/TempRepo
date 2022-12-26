using System.Linq;
using BugsnagUnity;
using Mirror;
using UnityEngine;

public class BasicPlayerController : NetworkRoomPlayer
{
    private PlayerControls playerInput;
    private CharacterController controller;
    public Animator animator;
    public NetworkAnimator networkAnimator;
    public GameObject stunEffect;
    [SerializeField] public GameObject deathScreen;
    
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private bool receiveInput;
    
    private Transform cameraMain;
    private Vector3 currentMove = Vector3.zero;

    private void Awake()
    {
        playerInput = new PlayerControls();
    }

    private void OnEnable()
    {
        if (receiveInput)
        {
            playerInput.Enable();
        }
    }

    private new void Start()
    {
        cameraMain = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        if (receiveInput)
        {
            playerInput.Enable();
        }
        else
        {
            playerInput.Disable();
        }
    }

    [ClientCallback]
    void Update()
    {
        if (!isOwned)
        {
            return; 
        }

        if (transform.position.y < -20)
        {
            Destroy(gameObject);
        }
        
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movementInput = playerInput.Player.Move.ReadValue<Vector2>();

        if (groundedPlayer)
        {
            currentMove = (cameraMain.forward * movementInput.y + cameraMain.right * movementInput.x);
            currentMove.y = 0;
        }

        controller.Move(currentMove.normalized * (Time.deltaTime * playerSpeed));
        
        if (currentMove != Vector3.zero && groundedPlayer)
        {
            gameObject.transform.forward = currentMove;
        }
        
        // Changes the height position of the player..
        if (playerInput.Player.Jump.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (playerInput.Player.Attack.triggered)
        {
            Attack();
        }

        var speed = Mathf.Abs(movementInput.x) + Mathf.Abs(movementInput.y);
        
        animator.SetFloat("Speed", speed);
    }
    
    private void Attack()
    {
        //animator.SetTrigger("Attack");
        networkAnimator.SetTrigger("Attack");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.root != gameObject.transform.root)
        {
            switch (other.tag)
            {
                case "Crate": Stun(2.0f); break;
                default: break;
            }
        }
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        print("Collision" + collision.gameObject);
    }
    
    public void Stun(float time)
    {
        Instantiate(stunEffect, gameObject.transform.Find("EffectSpot"));
        print("stunned");
        playerInput.Disable();
        Invoke(nameof(OnEnable), time);
    }

    private void OnDestroy()
    {
        var obj = FindObjectOfType<GameController>();
        obj.ActivateDeathScreen(this);
    }
}
