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
            public float initialCoolDownTime;
        }

        private void Update() 
        {
            var keys = new List<string>(itemsInCoolDown.Keys);
            foreach (string itemId in keys)
            {
                itemsInCoolDown[itemId].coolDownTime -= Time.deltaTime;
                if (itemsInCoolDown[itemId].coolDownTime < 0)
                {
                    itemsInCoolDown.Remove(itemId);
                }
            }
        }

        public void StartCoolDown(string itemId, float time)
        {
            var itemInCoolDown = new ItemInCooldown();
            itemInCoolDown.coolDownTime = time;
            itemInCoolDown.initialCoolDownTime = time;
            itemsInCoolDown[itemId] = itemInCoolDown; 
        }

        public float GetTimeRemaining (string itemID)
        {
            if (!itemsInCoolDown.ContainsKey(itemID)) return 0;
            return itemsInCoolDown[itemID].coolDownTime;
        }

        public float GetFractionRemaining(string itemID)
        {
            float remainingTime = GetTimeRemaining(itemID);
            if (remainingTime <= 0)
            {
                return 0;
            } 
            return remainingTime / itemsInCoolDown[itemID].initialCoolDownTime;
        }
    }
}
