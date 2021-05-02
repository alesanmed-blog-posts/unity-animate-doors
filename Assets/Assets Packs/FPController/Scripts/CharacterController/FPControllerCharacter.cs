using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof (CharacterController))]
[RequireComponent(typeof (PlayerInput))]
public class FPControllerCharacter : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float walkDivider = 2f;
    private Vector3 m_playerVelocity;
    private Vector3 m_moveVector;


    [Header("Ground detection")]
    [SerializeField] LayerMask groundMask;
    private bool groundCheck = true;
    private float groundOffset = 0.1f;

    [Header("Jumping")]
    [SerializeField] float jumpForce = 20f;
    private bool m_jumping;

    private GameActions gameActions;

    // Components
    private CharacterController m_characterController;
    private SprintController m_sprintController;
    private CrouchController m_crouchController;

    // States
    private bool m_walking = false;

    private Vector3 m_gravity = Vector3.zero;

    private void Awake() {
        gameActions = new GameActions();
        
        SetupCallbacks();
    }

    private void OnEnable() {
        gameActions.Player.Enable();
    }
    
    private void OnDisable() {
        gameActions.Player.Disable();
    }

    private void OnDestroy() {
        TeardownCallbacks();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
        m_sprintController = GetComponent<SprintController>();
        m_crouchController = GetComponent<CrouchController>();
    }

    private void FixedUpdate() {
        GroundCheck();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
        ProcessMovement();
    }

    private void ProcessInput() {
        Vector2 readAction = gameActions.Player.Move.ReadValue<Vector2>();

        Vector3 forwardVector = transform.forward * readAction.y;
        Vector3 rightVector = transform.right * readAction.x;

        m_moveVector = (forwardVector + rightVector).normalized;
    }

    private void ProcessMovement() {
        if (groundCheck && m_playerVelocity.y < Mathf.Epsilon) {
            m_playerVelocity.y = -2f;
        }

        if (m_crouchController) {
            m_moveVector /= m_crouchController.GetCrouchDivider();
        } if(m_walking) {
            m_moveVector /= walkDivider;
        } else if(m_sprintController) {
            m_moveVector *= m_sprintController.GetSprintMultiplier();
        }

        m_characterController.Move(movementSpeed * m_moveVector * Time.deltaTime);

        m_playerVelocity += Physics.gravity * Time.deltaTime;

        m_characterController.Move(m_playerVelocity * Time.deltaTime);
    }

    private void GroundCheck() {
        RaycastHit hitInfo;

        groundCheck = Physics.SphereCast(
            transform.position,
            m_characterController.radius,
            Vector3.down,
            out hitInfo,
            m_characterController.bounds.extents.y + groundOffset,
            groundMask
        );
    }

    private void Jump(CallbackContext ctx)
     {
         if (groundCheck) {
             m_playerVelocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
         }
     }

    private void SetupCallbacks() {
        gameActions.Player.Jump.performed += Jump;
        
        gameActions.Player.Walk.performed += (ctx) => m_walking = true;
        gameActions.Player.Walk.canceled += (ctx) => m_walking = false;
    }

    private void TeardownCallbacks() {
        gameActions.Player.Jump.performed -= Jump;
        
        gameActions.Player.Walk.performed -= (ctx) => m_walking = true;
        gameActions.Player.Walk.canceled -= (ctx) => m_walking = false;
    }
}
