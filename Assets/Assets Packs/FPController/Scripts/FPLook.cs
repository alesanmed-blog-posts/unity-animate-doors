using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPLook : MonoBehaviour
{
    [SerializeField]
    private LookSensitivity lookSensitivity = new LookSensitivity(1f, 0.2f);
    
    [Serializable] 
    public class LookSensitivity {
        [SerializeField]
        protected float vertical;

        public float Vertical {
            get {
                return vertical;
            }
            set {
                vertical = value;
            }
        }

        [SerializeField]
        protected float horizontal;

        public float Horizontal {
            get {
                return horizontal;
            }
            set {
                horizontal = value;
            }
        }

        public LookSensitivity(float horizontal, float vertical) {
            this.horizontal = horizontal;
            this.vertical = vertical;
        }
    }

    private Rigidbody m_rigidbody;
    private CharacterController m_characterController;

    private GameActions gameActions;

    private float m_cameraPitch = 0f;
    private float m_cameraYaw = 0f;

    private void Awake() {
        gameActions = new GameActions();
        gameActions.Player.ShowCursor.performed += ShowCursor;
        Cursor.visible = false;
    }

    private void OnEnable() {
        gameActions.Player.Enable();
    }
    
    private void OnDisable() {
        gameActions.Player.ShowCursor.performed -= ShowCursor;
        gameActions.Player.Disable();
    }

    void Start()
    {
        if (!TryGetComponent<Rigidbody>(out m_rigidbody) && !TryGetComponent<CharacterController>(out m_characterController)) {
            throw new Exception("No rigidbody nor characterController defined");   
        }
    }

    void Update()
    {
        ProcessLook();
    }

    private void ProcessLook()
    {
        var lookDelta = gameActions.Player.Look.ReadValue<Vector2>();

        m_cameraPitch = Mathf.Clamp(m_cameraPitch + -lookDelta.y * lookSensitivity.Vertical, -90, 90);
        m_cameraYaw = m_cameraYaw + lookDelta.x * lookSensitivity.Horizontal;

        Quaternion rotation = Quaternion.Euler(0f, m_cameraYaw, 0f);

        if(m_rigidbody != null) {
            // The rigidbody only rotates along the Y axis (horizontal) so it can not end facing down, but we can still apply
            // a relative force
            m_rigidbody.MoveRotation(rotation);
        } else {
            m_characterController.transform.eulerAngles = rotation.eulerAngles;
        }

        Vector3 currRotation = Camera.main.transform.eulerAngles;

        // The camera, instead, rotates along the X and Y axes so the player can look above and sideways
        Camera.main.transform.eulerAngles = new Vector3(m_cameraPitch, m_cameraYaw, currRotation.z);
    }

    private void ShowCursor(InputAction.CallbackContext obj)
    {
        Cursor.visible = true;
    }
}
