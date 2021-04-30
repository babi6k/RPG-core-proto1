using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Abilities
{
    [CreateAssetMenu(fileName = "MyAbility", menuName = "Abilities/Ability", order = 0)]
    public class Ability : ActionItem
    {
        [SerializeField] TargetingStrategy targeting;

        public override void Use(GameObject user)
        {
            if (targeting != null)
            {
                Debug.Log("Targeting");
                targeting.StartTargeting(user,TargetAquired);
            }
        }

        private void TargetAquired(IEnumerable<GameObject> targets)
        {
            foreach (var target in targets)
            {
                Debug.Log(target);
            }
        }
    }
}
