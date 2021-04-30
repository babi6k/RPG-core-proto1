using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Attributes;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Abilities
{
    [CreateAssetMenu(fileName = "MyAbility", menuName = "Abilities/Ability", order = 0)]
    public class Ability : ActionItem
    {
        [SerializeField] TargetingStrategy targeting;
        [SerializeField] FilterStrategy[] filters;
        [SerializeField] EffectStrategy[] effects;
        [SerializeField] float effectScale = 1;
        [SerializeField] float manaCost = 0;

        public override void Use(GameObject user)
        {
            if (manaCost > 0)
            {
                var mana = user.GetComponent<Mana>();
                if (mana.GetMana() < manaCost) return;
            }
            var cooldownManger = user.GetComponent<CoolDownManager>();
            if (cooldownManger && cooldownManger.GetTimeRemaining(GetItemID()) > 0)
            {
                return;
            }
            if (targeting != null)
            {
                var targetingData = new TargetingData(effectScale,user);
                targeting.StartTargeting(targetingData, TargetAquired);
            }
        }

        private void TargetAquired(TargetingData data)
        {
            if (manaCost > 0)
            {
                var mana = data.GetSource().GetComponent<Mana>();
                if (!mana.UseMana(manaCost)) return;
            }
            var cooldownManager = data.GetSource().GetComponent<CoolDownManager>();
            if (cooldownManager)
            {
                cooldownManager.StartCoolDown(GetItemID(),GetCoolDownTime());
            }
            foreach (var filter in filters)
            {
                data.SetTargets(filter.Filter(data.GetTargets()));
            }

            foreach (var effect in effects)
            {
               effect.StartEffect(data,null);
            }
        }
    }
}
