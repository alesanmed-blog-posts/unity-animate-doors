using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CrouchController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] CharacterController characterCollider;

    [Header("Crouch parameters")]
    [SerializeField] float crouchHeight = 0.5f;
    [SerializeField] float crouchDivider = 3f;
    private float currCrouchDivider = 1f;
    private float startingHeight;
    private float decrouchTime = 0.7f;

    [Header("Roof detection")]
    [SerializeField] LayerMask roofMask;
    private bool m_roofCheck = true;
    private float m_roofOffset = 0.1f;

    GameActions gameActions;

    private void Awake() {
        gameActions = new GameActions();

        gameActions.Player.Crouch.performed += OnCrouchPerformed;
        gameActions.Player.Crouch.canceled += OnCrouchCanceled;
    }

    private void OnEnable() {
        gameActions.Player.Enable();
    }

    private void Start() {
        startingHeight = characterCollider.height;
    }

    private void OnDisable() {
        gameActions.Player.Disable();
    }

    private void OnDestroy() {
        gameActions.Player.Crouch.performed += OnCrouchPerformed;
        gameActions.Player.Crouch.canceled += OnCrouchCanceled;
    }

    private void Update() {
        RoofCheck();
    }

    private void OnCrouchPerformed(CallbackContext ctx) {
        StopAllCoroutines();

        currCrouchDivider = crouchDivider;
        characterCollider.height -= crouchHeight;
        characterCollider.center = new Vector3(characterCollider.center.x, characterCollider.center.y + crouchHeight / 2, characterCollider.center.z);
    }

    private void OnCrouchCanceled(CallbackContext ctx) {
        if(m_roofCheck) {
            StartCoroutine(CheckDeCrouching());
        } else {
            currCrouchDivider = 1f;

            StartCoroutine(Decrouch());
        }
    }

    private IEnumerator CheckDeCrouching() {
        while (m_roofCheck) {
            yield return new WaitForSeconds(0.2f);
        }

        OnCrouchCanceled(new CallbackContext());
    }

    private IEnumerator Decrouch() {
        int steps = 60;
        float fulltStep = crouchHeight / 2;
        float targetPosition = transform.position.y + fulltStep;
        float targetCenter = characterCollider.center.y - fulltStep;
        float targetHeight = characterCollider.height + crouchHeight;

        while(characterCollider.height < startingHeight) {
            transform.position = new Vector3(transform.position.x, transform.position.y + fulltStep / steps, transform.position.z);
            characterCollider.center = new Vector3(characterCollider.center.x, characterCollider.center.y - fulltStep / steps, characterCollider.center.z);
            characterCollider.height += crouchHeight / steps;

            yield return new WaitForSeconds(decrouchTime / steps);
        }

        transform.position = new Vector3(transform.position.x, targetPosition, transform.position.z);
        characterCollider.center = new Vector3(characterCollider.center.x, targetCenter, characterCollider.center.z);
        characterCollider.height = targetHeight;

        yield return null;
    }

    private void RoofCheck() {
        RaycastHit hitInfo;

        m_roofCheck = Physics.SphereCast(
            transform.position,
            characterCollider.radius,
            Vector3.up,
            out hitInfo,
            characterCollider.bounds.extents.y + crouchHeight - m_roofOffset,
            roofMask
        );
    }

    public float GetCrouchDivider() => currCrouchDivider;
}
