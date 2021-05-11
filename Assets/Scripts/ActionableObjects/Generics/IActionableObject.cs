using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActionableObject
{
    void Interact();
    string GetInteractionText();
    bool IsInteracterActive();
}
