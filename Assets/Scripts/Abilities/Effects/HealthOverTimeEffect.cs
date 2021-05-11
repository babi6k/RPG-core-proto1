using System;
using System.Collections;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "HealthOverTime", menuName = "Abilities/Effects/HealthOverTime", order = 0)]
    public class HealthOverTimeEffect : EffectStrategy
    {
        [SerializeField] float healthChange;
        [SerializeField] int steps = 1;
        [SerializeField] float totalTime = 1;

        public override void StartEffect(AbilityData data, Action finished)
        {
            data.StartCoroutine(HealthOverTime(data, finished));
        }

        private void DealDamage(AbilityData data)
        {
            float damage = healthChange / steps;
            foreach (var target in data.GetTargets())
            {
                var health = target.GetComponent<Health>();
                if (health)
                {
                    if (damage < 0)
                    {
                        health.TakeDamage(data.GetUser(), -damage);
                    }
                    else
                    {
                        health.Heal(damage);
                    }
                }
            }
        }

        private IEnumerator HealthOverTime(AbilityData data, Action finished)
        {
            for (int i = 0; i < steps; i++)
            {
                DealDamage(data);
                yield return new WaitForSeconds (totalTime / steps);
            }
            finished();
        }
    }
}