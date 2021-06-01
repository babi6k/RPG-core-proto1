using RPG.Inventories;
using RPG.Movement;
using UnityEngine;
namespace RPG.Control
{
    public class GoldCollector : MoveableActionBehavior<CoinDrop>
    {
        protected override void Perform()
        {
            target.AddGold();
            Cancel();
        }
    }
}
