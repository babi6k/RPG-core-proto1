using GameDevTV.Inventories;
using RPG.Movement;
using UnityEngine;

public class ItemCollector : MoveableActionBehavior<Pickup>
{
    protected override void Perform()
    {
        target.PickupItem();
        Cancel();
    }
}
