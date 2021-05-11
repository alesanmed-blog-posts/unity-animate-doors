using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : ActionableObject
{
    protected override void InnerStart(){}

    protected override void InnerInteract() {
        ItemPickEvents.NotifyItemPick(this);
        Destroy(gameObject);
    }
}
