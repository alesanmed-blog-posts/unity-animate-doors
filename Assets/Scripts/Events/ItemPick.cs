
using UnityEngine;

public delegate void NotifyItemPick(ActionableObject pickedItem);

public static class ItemPickEvents {
    public static event NotifyItemPick OnItemPick;

    public static void NotifyItemPick(ActionableObject pickedItem) {
        OnItemPick?.Invoke(pickedItem);
    }
}