using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using System;
using UnityEngine;

namespace RPG.Combat
{

    public class Caster : MonoBehaviour , IAction
    {
        [SerializeField] Transform hand;
        //Cached
        Health target;
        Projectile currentProjectile;
        Animator animator;
        bool isCast = false;
        float currentSpellRange = 0;
        float currentDamage = 0;
        private void Start() 
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (isCast) return;
            if (target == null) return;
            if (target.IsDead()) return;

            if (!GetIsInRange(target.transform))
            {//Moving to range to attack
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                //In range of attack Stoping the movement
                GetComponent<Mover>().Cancel();
                SpellBehavior();
            }
        }

        private void SpellBehavior()
        {
            transform.LookAt(target.transform);
            TriggerSpell();
        }

        public void Cast(GameObject spellTarget, float spellRange, float spellDamage, Projectile newProjectile)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = spellTarget.GetComponent<Health>();
            currentSpellRange = spellRange;
            currentDamage = spellDamage;
            currentProjectile = newProjectile;
            isCast = false;
        }

        public void TriggerSpell()
        {
            animator.ResetTrigger("cast");
            animator.SetTrigger("cast");
        }

        private bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < currentSpellRange;
        }

        public void CastSpell()
        {
            if (target == null) { return; }
            if (currentProjectile == null) return;
            Projectile newProjectile = Instantiate(currentProjectile,hand.position, Quaternion.identity);
            newProjectile.SetTarget(target, gameObject, currentDamage);
            isCast = true;
        }

        public void Cancel()
        {
            target = null;
            GetComponent<Mover>().Cancel();
        }
    }
}
