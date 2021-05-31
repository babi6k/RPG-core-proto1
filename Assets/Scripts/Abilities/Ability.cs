using System;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Attributes;
using RPG.Core;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Abilities
{
    [CreateAssetMenu(fileName = "MyAbility", menuName = "Abilities/Ability", order = 0)]
    public class Ability : ActionItem
    {
        [SerializeField] TargetingStrategy targetingStrategy;
        [SerializeField] FilterStrategy[] filterStrategies;
        [SerializeField] EffectStrategy[] effectStrategies;
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

            AbilityData data = new AbilityData(user);

            ActionScheduler scheduler = user.GetComponent<ActionScheduler>();
            scheduler.StartAction(data);

            targetingStrategy.StartTargeting(data, () =>
            {
                TargetAquired(data);
            });
        }

        private void TargetAquired(AbilityData data)
        {
            if (data.IsCancelled()) return;

            Mana mana = data.GetUser().GetComponent<Mana>();
            if (mana)
            {
                if (!mana.UseMana(manaCost)) return;
            }
            
            CoolDownManager coolDownManager = data.GetUser().GetComponent<CoolDownManager>();
            coolDownManager.StartCoolDown(GetItemID(), GetCoolDownTime());

            foreach (var filterStrategy in filterStrategies)
            {
                data.SetTargets(filterStrategy.Filter(data.GetTargets()));
            }

            foreach (var effect in effectStrategies)
            {
                effect.StartEffect(data, EffectFinished);
            }
        }

        private void EffectFinished()
        {

        }
    }

}
