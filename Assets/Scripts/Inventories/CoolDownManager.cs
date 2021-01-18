using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Inventories
{
    public class CoolDownManager : MonoBehaviour
    {
        Dictionary<string, ItemInCooldown> itemsInCoolDown = new Dictionary<string, ItemInCooldown>();

        private class ItemInCooldown
        {
            public float coolDownTime;
            public float timeSinceLastCooldown;
        }

        public void AddCoolDown(string itemId , float coolDownTime)
        {
            if (!itemsInCoolDown.ContainsKey(itemId))
            {
                var itemInCoolDown = new ItemInCooldown();
                itemInCoolDown.coolDownTime = coolDownTime;
                itemInCoolDown.timeSinceLastCooldown = Mathf.Infinity;
                itemsInCoolDown[itemId] = itemInCoolDown;
            }
        }

        public void StartCoolDown(string itemId)
        {
            if (itemsInCoolDown.ContainsKey(itemId))
            {
                if (itemsInCoolDown[itemId].coolDownTime < itemsInCoolDown[itemId].timeSinceLastCooldown)
                {
                    itemsInCoolDown[itemId].timeSinceLastCooldown = 0;
                }
            }
        }

        public bool IsInCoolDown(string itemId)
        {
            if (itemsInCoolDown.ContainsKey(itemId))
            {
                return itemsInCoolDown[itemId].coolDownTime > itemsInCoolDown[itemId].timeSinceLastCooldown; 
            }
            return false;
        }

        private void Update()
        {
            foreach (var pair in itemsInCoolDown)
            {
                pair.Value.timeSinceLastCooldown += Time.deltaTime;
            }
        }



    }
}
