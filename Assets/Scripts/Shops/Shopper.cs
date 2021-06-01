using System;
using RPG.Movement;
using UnityEngine;

namespace RPG.Shops
{
    public class Shopper : MoveableActionBehavior<Shop>
    {
        Shop activeShop = null;

        public event Action activeShopChange;

        public void SetActiveShop(Shop shop)
        {
            if (activeShop != null)
            {
                activeShop.SetShopper(null);
            }

            activeShop = shop;

            if (activeShop != null)
            {
                activeShop.SetShopper(this);
            }

            if (activeShopChange != null)
            {
                activeShopChange();
            }
        }

        public Shop GetActiveShop()
        {
            return activeShop;
        }

        protected override void Perform()
        {
            if (target == activeShop) return;
            SetActiveShop(target);
        }
    }
}
