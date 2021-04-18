using GameDevTV.Inventories;
using RPG.Attributes;
using RPG.Combat;
using UnityEngine;

namespace RPG.Inventories
{

    [CreateAssetMenu(menuName = ("RPG/Inventory/Skill"))]
    public class Skill : ActionItem
    {
        [SerializeField] float manaAmount = 10f;
        [SerializeField] float spellRange = 15f;
        [SerializeField] float spellDamage = 10f;


        public float GetManaAmount() { return manaAmount; }
        public float GetSpellRange() { return spellRange; }
        public float GetSpellDamage() { return spellDamage; }
    }
}
