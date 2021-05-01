using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Control;
using RPG.Core;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "HealthEffect", menuName = "Abilities/Effects/Health", order = 0)]
    public class HealthEffect : EffectStrategy, IAction
    {
        [SerializeField] float amount = 1;
        [SerializeField] int steps = 1;
        [SerializeField] float totalTime = 1;
        [SerializeField] float initialDelay = 0;
        [Header("Effect VFX")]
        [SerializeField] float effectSize = 0;
        [SerializeField] Transform effectPrefab;

        public override void StartEffect(TargetingData data, Action complete)
        {
            var playerController = data.GetSource().GetComponent<PlayerController>();
            playerController.StartCoroutine(Effect(data,complete));
        }

        private IEnumerator Effect(TargetingData data, Action complete)
        {
            var scheduler = data.GetSource().GetComponent<ActionScheduler>();
            scheduler.StartAction(this, 3, 3);
            yield return new WaitUntil(() => scheduler.IsCurrentAction(this));
            yield return new WaitForSeconds(initialDelay);
            var effect = SpawnEffect(data.GetTargetPoint());
            for (int i = 0; i < steps; i++)
            {
                 DealDamage(data);
                 if (scheduler.IsCurrentAction(this))
                 {
                     scheduler.FinishAction(this);
                 }
                 yield return new WaitForSeconds(totalTime / steps);
            }
            if (complete != null) complete();
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

        public void Activate()
        {
           
        }

        public void Cancel()
        {
            
        }
    }
}
