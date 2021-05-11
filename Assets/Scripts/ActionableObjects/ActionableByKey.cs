using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionableByKey : StandardActionableObject
{
    [Tooltip("This item will activate the actuator of this object when picked")]
    [SerializeField] KeyItem actionedBy;

    protected override void InnerStart()
    {
        base.InnerStart();
        SetIsInteractive(false);
    }

    private void OnEnable() {
        ItemPickEvents.OnItemPick += OnItemPick;
    }

    private void OnDisable() {
        ItemPickEvents.OnItemPick -= OnItemPick;
    }

    private void OnItemPick(ActionableObject pickedItem)
    {
        if(pickedItem is KeyItem && pickedItem.Equals(actionedBy)) {
            SetIsInteractive(true);
        }
    }
}
