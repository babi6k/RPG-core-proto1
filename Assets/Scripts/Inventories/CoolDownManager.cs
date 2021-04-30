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

        public void StartCoolDown(string itemId, float time)
        {
            var itemInCoolDown = new ItemInCooldown();
            itemInCoolDown.coolDownTime = time;
            itemInCoolDown.timeSinceLastCooldown = Time.time + time;
            itemsInCoolDown[itemId] = itemInCoolDown; 
        }

        public float GetTimeRemaining (string itemID)
        {
            if (!itemsInCoolDown.ContainsKey(itemID)) return 0;
            float remaining = itemsInCoolDown[itemID].timeSinceLastCooldown - Time.time;
            if (remaining < 0)
            {
                remaining = 0;
                itemsInCoolDown.Remove(itemID);
            }
            return remaining;
        }

        public float GetFractionRemaining(string itemID)
        {
            float remainingTime = GetTimeRemaining(itemID);
            if (remainingTime <= 0)
            {
                return 0;
            }
            Debug.Log("Fraction Time = " + remainingTime / itemsInCoolDown[itemID].coolDownTime );
            return remainingTime / itemsInCoolDown[itemID].coolDownTime;
        }
    }
}
