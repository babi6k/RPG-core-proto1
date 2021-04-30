using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "DamageEffect", menuName = "Abilities/Effects/Damage", order = 0)]
    public class DamageEffect : EffectStrategy
    {
        [SerializeField] float amount = 1;
        [SerializeField] int steps = 1;
        [SerializeField] float totalTime = 1;
        [SerializeField] Transform effectPrefab;
        public override void StartEffect(TargetingData data, Action complete)
        {
            var playerController = data.GetSource().GetComponent<PlayerController>();
            playerController.StartCoroutine(Effect(data,complete));
        }

        private IEnumerator Effect(TargetingData data, Action complete)
        {
            SpawnEffect(data.GetTargetPoint());
            for (int i = 0; i < steps; i++)
            {
                 DealDamage(data);
                 yield return new WaitForSeconds(totalTime / steps);
            }
            if (complete != null) complete();
        }

        private void DealDamage(TargetingData data)
        {
            foreach (var target in data.GetTargets())
            {
                Health healthComp = target.GetComponent<Health>();
                if (healthComp != null)
                {
                    healthComp.TakeDamage(data.GetSource(), amount * data.GetEffectScaling());
                }
            }
        }

        private void SpawnEffect(Vector3 position)
        {
            var effect = Instantiate(effectPrefab);
            effect.position = position;
        }

    }
}
