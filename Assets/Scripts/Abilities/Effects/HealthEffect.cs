using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Abilities.Helpers;
using RPG.Attributes;
using RPG.Control;
using RPG.Core;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "HealthEffect", menuName = "Abilities/Effects/Health", order = 0)]
    public class HealthEffect : EffectStrategy
    {
        [SerializeField] float amount = 1;
        [SerializeField] int steps = 1;
        [SerializeField] float totalTime = 1;
        [SerializeField] float initialDelay = 0;
        [Header("Effect VFX")]
        [SerializeField] float effectSize = 0;
        [SerializeField] Transform effectPrefab;

        public override IAction MakeAction(TargetingData data, Action complete)
        {
            return new CoroutineAction(data.GetCorutineOwner(), Effect(data, complete));
        }

        private IEnumerator Effect(TargetingData data, Action complete)
        {
            yield return new WaitForSeconds(initialDelay);
            var effect = SpawnEffect(data.GetTargetPoint());
            for (int i = 0; i < steps; i++)
            {
                 DealDamage(data);
                 if (i == 0)
                 {
                     //Can only canel up to this point, then damage/Heal continues regardless.
                     complete();
                 }
                 yield return new WaitForSeconds(totalTime / steps);
            }
            Destroy(effect.gameObject);
        }

        private void DealDamage(TargetingData data)
        {
            float damage = amount * data.GetEffectScaling() / steps;
            foreach (var target in data.GetTargets())
            {
                Health healthComp = target.GetComponent<Health>();
                if (healthComp != null)
                {
                    if (damage >= 0)
                    {
                        healthComp.TakeDamage(data.GetSource(), damage);
                    }
                    else
                    {
                        healthComp.Heal(-damage);
                    }
                }
            }
        }

        private Transform SpawnEffect(Vector3 position)
        {
            var effect = Instantiate(effectPrefab);
            effect.position = position;
            effect.localScale = new Vector3(effectSize,effectSize,effectSize);
            return effect;
        }
    }
}