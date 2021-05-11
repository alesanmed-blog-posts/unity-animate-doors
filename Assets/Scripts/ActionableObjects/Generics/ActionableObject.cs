using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionableObject : MonoBehaviour, IActionableObject
{

    [Header("UI")]
    [Tooltip("The text that will appear in the overlay before interacting with this object")]
    [SerializeField] string interactionText = "open";

    [Tooltip("The text that will appear in the overlay after interacting with this object")]
    [SerializeField] string interactionTextReverse = "close";

    protected bool m_interacted = false;

    protected bool m_interactive = true;

    private void Start() {
        InnerStart();
    }

    protected abstract void InnerStart();

    public string GetInteractionText()
    {
        return !m_interacted ? interactionText : interactionTextReverse;
    }

    public void Interact() {
        InnerInteract();
        m_interacted = !m_interacted;
    }

    protected virtual void InnerInteract() {}

    public virtual bool IsInteracterActive() => m_interactive;

    public virtual void SetIsInteractive(bool isInteractive) => m_interactive = isInteractive;
}
