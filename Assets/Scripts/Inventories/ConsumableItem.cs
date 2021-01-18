using GameDevTV.Inventories;
using RPG.Attributes;
using RPG.Stats;
using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = ("RPG/Inventory/Consumable Item"))]
    public class ConsumableItem : ActionItem
    {
        [SerializeField] int consumeValue = 0;
        [SerializeField] Stat stat;


        public override void Use(GameObject user)
        {
            base.Use(user);
            if (stat == Stat.Health)
            {
                user.GetComponent<Health>().Heal(consumeValue);
            }

            if (stat == Stat.Mana)
            {
                user.GetComponent<Mana>().RestoreMana(consumeValue);
            }

        }
    }
}
