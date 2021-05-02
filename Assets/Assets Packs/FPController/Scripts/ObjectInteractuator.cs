using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ObjectInteractuator : MonoBehaviour
{
    // [SerializeField] InteractionText interactText;

    // [SerializeField] LayerMask interactionMask;

    // GameActions actions;
    // Camera mainCamera;

    // IActionableObject currInteractableObject;

    // private void Awake() {
    //     actions = new GameActions();
    //     actions.Player.Interact.performed += OnInteractPerformed;
    // }

    // private void OnEnable() {
    //     actions.Player.Enable();
    // }

    // private void OnDisable() {
    //     actions.Player.Disable();
    // }

    // private void OnDestroy() {
    //     actions.Player.Interact.performed -= OnInteractPerformed;
    // }

    // private void Start() {
    //     mainCamera = Camera.main;
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     CheckInteractions();
    // }

    // private void CheckInteractions()
    // {
    //     Ray cameraRay = mainCamera.ViewportPointToRay(Vector3.one / 2f);

    //     RaycastHit hit;
    //     IActionableObject objectHit;

    //     // We are looking towards an interactable object an in range
    //     if (Physics.Raycast(cameraRay, out hit, 1.5f, interactionMask, QueryTriggerInteraction.Collide) 
    //         && hit.transform.TryGetComponent<IActionableObject>(out objectHit) 
    //         && objectHit.IsInteracterActive()
    //     ) {
    //         interactText.SetInteractionText(objectHit.GetInteractionText());
    //         currInteractableObject = objectHit;
            
    //         interactText.Enable();
    //     }
    //     else if (interactText.enabled)
    //     {
    //         interactText.Disable();
    //         currInteractableObject = null;
    //     }
    // }

    // private void OnInteractPerformed(InputAction.CallbackContext obj)
    // {
    //     currInteractableObject?.Interact();
    // }

    // private void OnDrawGizmos() {
    //     if(mainCamera == null) return;

    //     Ray cameraRay = mainCamera.ViewportPointToRay(Vector3.one / 2f);

    //     Gizmos.color = Color.red;
    //     Gizmos.DrawRay(cameraRay.origin, cameraRay.direction * 1.5f);
    // }

}
