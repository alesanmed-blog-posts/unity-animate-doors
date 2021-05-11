using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class InteractionText : MonoBehaviour
{
    private Text m_text;
    
    private string m_initialText;
    private string m_interactionText;

    private void Start() {
        m_text = GetComponent<Text>();

        m_initialText = m_text.text;
    }

    private void Update() {
        if(m_text.enabled) {
            m_text.text = $"{m_initialText}{m_interactionText}";
        }
    }

    public void SetInteractionText(string interactionText) {
        m_interactionText = interactionText;
    }

    public void Enable() {
        m_text.enabled = true;
    }

    public void Disable() {
        m_text.enabled = false;
    }

    public bool IsEnabled() => m_text.enabled;
}
