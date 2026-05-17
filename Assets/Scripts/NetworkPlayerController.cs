using Unity.Netcode;
using UnityEngine;

public class NetworkPlayerController : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float groundedGravity = -2f;

    private CharacterController characterController;
    private float verticalVelocity;

    TopDownCamera cam; 
        

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Start()
    {   
        cam = Camera.main.GetComponent<TopDownCamera>();
        cam.SetFollowTarget(transform);
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector2 movementInput = new Vector2(horizontalInput, verticalInput);

        if (IsServer)
        {
            MovePlayer(movementInput);
        }
        else
        {
            MovePlayerRpc(movementInput);
        }
    }

    [Rpc(SendTo.Server)]
    private void MovePlayerRpc(Vector2 movementInput)
    {
        MovePlayer(movementInput);
    }

    private void MovePlayer(Vector2 movementInput)
    {
        if (characterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = groundedGravity;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 moveDirection = new Vector3(movementInput.x, 0f, movementInput.y).normalized;

        Vector3 horizontalMovement = moveDirection * moveSpeed;

        Vector3 verticalMovement = Vector3.up * verticalVelocity;

        Vector3 finalMovement = horizontalMovement + verticalMovement;

        characterController.Move(finalMovement * Time.deltaTime);
    }
}