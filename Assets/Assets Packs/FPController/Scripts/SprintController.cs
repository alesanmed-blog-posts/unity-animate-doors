using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public delegate void NotifyStaminaEmpty();
public delegate void NotifyStaminaRecovered();

public class SprintController : MonoBehaviour
{
    public static event NotifyStaminaEmpty OnStaminaEmpty;
    public static event NotifyStaminaEmpty OnStaminaRecovered;

    [Header("Movement")]
    [SerializeField] float sprintMultiplier = 1.5f;
    private float currSprintMultiplier = 1;

    [Header("Stamina")]
    [SerializeField] float maxStamina = 10;
    [Tooltip("How much stamina points are lost per second")]
    [SerializeField] float staminaLosePerSec = 1;
    [Tooltip("How much seconds has to elapse before staring the stamina recovery")]
    [SerializeField] float staminaIncreaseCooldown = 1.5f;
    [Tooltip("Min % of stamina that has to be recovered before notifying that the stamina can be consumed again")]
    [Range(0, 1)]
    [SerializeField] float minStaminaRecoveredToNotify = 0.1f;

    [SerializeField] Slider staminaIndicator;
    float staminaChangeStep = 0.1f;

    private float currStamina;

    private bool m_sprinting = false;

    private bool m_ableToSprint = true;

    GameActions gameActions;

    public float GetSprintMultiplier() => currSprintMultiplier;

    private void Awake() {
        gameActions = new GameActions();

        gameActions.Player.Sprint.performed += OnSprintPerformed;
        gameActions.Player.Sprint.canceled += OnSprintCanceled;
    }

    private void OnEnable() {
        gameActions.Player.Enable();
    }

    private void OnDisable() {
        gameActions.Player.Disable();
    }

    private void OnDestroy() {
        gameActions.Player.Sprint.performed -= OnSprintPerformed;
        gameActions.Player.Sprint.canceled -= OnSprintCanceled;
    }

    private void Start() {
        currStamina = maxStamina;

        UpdateStaminaIndicator();
    }

    private void OnSprintPerformed(CallbackContext ctx) {
        if (!m_ableToSprint) return;

        currSprintMultiplier = sprintMultiplier;
        m_sprinting = true;
        StartCoroutine(DecreaseStamina());
    }

    private IEnumerator DecreaseStamina() {
        while (m_sprinting && currStamina > Mathf.Epsilon) {
            currStamina -= staminaChangeStep;

            UpdateStaminaIndicator();

            yield return new WaitForSeconds(1 / (staminaLosePerSec / staminaChangeStep));
        }

        // This means that we stopped running because we ran out of stamina, not because the user stopped running
        if (currStamina <= Mathf.Epsilon) {
            // Fix to 0 at the end to self-correct accumulated errors
            currStamina = 0f;

            // Force the player to stop running so the button has to be pressed again
            m_sprinting = false;
            currSprintMultiplier = 1f;

            m_ableToSprint = false;

            OnStaminaEmpty?.Invoke();
        }

        StartCoroutine(IncreaseStamina());
    }

    private void OnSprintCanceled(CallbackContext ctx) {
        m_sprinting = false;
        currSprintMultiplier = 1f;
        StartCoroutine(IncreaseStamina());
    }

    private IEnumerator IncreaseStamina() {
        yield return new WaitForSeconds(staminaIncreaseCooldown);

        while (!m_sprinting && currStamina < maxStamina) {
            currStamina += staminaChangeStep;

            if (currStamina / maxStamina >= minStaminaRecoveredToNotify) {
                m_ableToSprint = true;
                OnStaminaRecovered?.Invoke();
            }

            UpdateStaminaIndicator();

            yield return new WaitForSeconds(1 / (staminaLosePerSec / staminaChangeStep));
        }

        // We filled all the stamina, but the user is not running
        if (!m_sprinting) {
            // Fix to 0 at the end to self-correct accumulated errors
            currStamina = maxStamina;
        }
    }

    private void UpdateStaminaIndicator() {
        if(staminaIndicator) staminaIndicator.value = currStamina / maxStamina;
    }
}
