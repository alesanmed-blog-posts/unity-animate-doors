using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof (Camera))]
public class HeadTilt : MonoBehaviour
{
    [SerializeField] float maxHorizontalRange = 0.5f;
    [SerializeField] float maxAngle = 45f;
    [SerializeField]float tiltSmoothFactor = 0.3f;

    private GameActions gameActions;

    private Vector3 startingPosition;
    private Vector3 targetPosition;
    private Vector3 startingRotation;
    private Vector3 targetRotation;

    Vector3 m_targetLocalPosition;
    Vector3 m_remainingDistance;
    Vector3 m_traveledDistance;

    Vector3 m_targetLocalRotation;
    Vector3 m_remainingRotation;
    Vector3 m_traveledRotation;

    bool tiltStatic = false;

    private void Awake() {
        gameActions = new GameActions();

        gameActions.Player.HeadTilt.performed += OnHeadTilt;
        gameActions.Player.HeadTilt.canceled += OnHeadTiltCanceled;
    }

    private void OnEnable() {
        gameActions.Player.Enable();
    }

    private void OnDisable() {
        gameActions.Player.Disable();
    }

    private void OnDestroy() {
        gameActions.Player.HeadTilt.performed -= OnHeadTilt;
        gameActions.Player.HeadTilt.canceled -= OnHeadTiltCanceled;
    }

    private void Start() {
        startingPosition = transform.localPosition;
        targetPosition = maxHorizontalRange * Vector3.right;

        startingRotation = transform.localEulerAngles;
        targetRotation = new Vector3(0f, 0f, maxAngle);

        m_traveledDistance = Vector3.zero;
    }

    private void Update()
    {
        ProcessMovement();
        ProcessRotation();
    }

    private void ProcessRotation() {
        if (m_traveledRotation.magnitude >= m_remainingRotation.magnitude) {
            if(tiltStatic) transform.localEulerAngles = startingRotation;   
        } else {
            Vector3 localRotation = transform.localEulerAngles + tiltSmoothFactor * Time.deltaTime * m_remainingRotation;

            transform.localEulerAngles = localRotation;

            m_traveledRotation += tiltSmoothFactor * Time.deltaTime * m_remainingRotation;
        }
    }

    private void ProcessMovement() {
        if (m_traveledDistance.magnitude >= m_remainingDistance.magnitude) {
            if(tiltStatic) transform.localPosition = startingPosition;
        } else {
            Vector3 localPosition = transform.localPosition + tiltSmoothFactor * Time.deltaTime * m_remainingDistance;
        
            transform.localPosition = localPosition;

            m_traveledDistance += tiltSmoothFactor * Time.deltaTime * m_remainingDistance;
        }
    }

    private void OnHeadTilt(CallbackContext ctx)
    {
        float tiltValue = ctx.ReadValue<float>();

        SetTargetPosition(tiltValue);
        // Invert the value since we are rotating in Z axis
        SetTargetRotation(-1 * tiltValue);

        tiltStatic = false;
    }

    private void SetTargetRotation(float tiltValue)
    {
        m_targetLocalRotation = new Vector3(startingRotation.x, startingRotation.y, tiltValue * targetRotation.z);
        m_remainingRotation = m_targetLocalRotation - transform.localEulerAngles;

        if (Mathf.Abs(m_remainingRotation.x) > 180f) {
            m_remainingRotation.x -= Mathf.Sign(m_remainingRotation.x) * 360f;
        }

        if (Mathf.Abs(m_remainingRotation.y) > 180f) {
            m_remainingRotation.y -= Mathf.Sign(m_remainingRotation.y) * 360f;
        }

        if (Mathf.Abs(m_remainingRotation.z) > 180f) {
            m_remainingRotation.z -= Mathf.Sign(m_remainingRotation.z) * 360f;
        }

        m_traveledRotation = Vector3.zero;
    }

    private void SetTargetPosition(float tiltValue) {
        m_targetLocalPosition = new Vector3(tiltValue * targetPosition.x, startingPosition.y, startingPosition.z);
        m_remainingDistance = m_targetLocalPosition - transform.localPosition;

        m_traveledDistance = Vector3.zero;
    }

    private void OnHeadTiltCanceled(CallbackContext ctx)
    {
        ResetTargetPoistion();
        ResetTargetRotation();

        tiltStatic = true;
    }

    private void ResetTargetRotation()
    {
        m_targetLocalRotation = startingRotation;
        m_remainingRotation = m_targetLocalRotation - transform.localEulerAngles;

        if (Mathf.Abs(m_remainingRotation.x) > 180f) {
            m_remainingRotation.x -= Mathf.Sign(m_remainingRotation.x) * 360f;
        }

        if (Mathf.Abs(m_remainingRotation.y) > 180f) {
            m_remainingRotation.y -= Mathf.Sign(m_remainingRotation.y) * 360f;
        }

        if (Mathf.Abs(m_remainingRotation.z) > 180f) {
            m_remainingRotation.z -= Mathf.Sign(m_remainingRotation.z) * 360f;
        }

        m_traveledRotation = Vector3.zero;
    }

    private void ResetTargetPoistion()
    {
        m_targetLocalPosition = startingPosition;
        m_remainingDistance = m_targetLocalPosition - transform.localPosition;

        m_traveledDistance = Vector3.zero;
    }
}
