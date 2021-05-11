using System.Collections;
using System.Collections.Generic;
using AnimateDoors.Animation;
using UnityEngine;

public class StandardActionableObject : ActionableObject
{
    [Header("Animation")]
    [Tooltip("The animation state to play when interacting with this object")]
    [SerializeField] string animationState;

    protected Animator m_animator;

    protected override void InnerStart()
    {
        m_animator = GetComponent<Animator>();        
    }

    protected override void InnerInteract()
    {
        UpdateAnimation();
    }

    private void UpdateAnimation() {
        m_animator.SetFloat(AnimationConstants.ANIMATION_DIRECTION, !m_interacted ? 1f : -1f);
        m_animator.Play(animationState);
    }
}
