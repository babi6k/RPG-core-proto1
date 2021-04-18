using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using System;
using UnityEngine;

namespace RPG.Combat
{

    public class Caster : MonoBehaviour, IAction
    {
        [SerializeField] Transform hand;
        //Cached
        Health target;
        Projectile currentProjectile;
        GameObject currentAOEEffect;
        Animator animator;
        bool isCastSpell = false;
        bool isCastAOE = false;
        float currentSpellRange = 0;
        float currentDamage = 0;
        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (target == null) return;
            if (target.IsDead()) return;

            if (!GetIsInRange(target.transform))
            {//Moving to range to attack
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                //In range of attack Stoping the movement
                if (isCastSpell || isCastAOE)
                {
                    GetComponent<Mover>().Cancel();
                    SpellBehavior();
                }
            }
        }

        public void SetHandTransfroms(Transform left)
        {
            hand = left;
        }


        public void CastSpell(GameObject spellTarget, float spellRange, float spellDamage, Projectile newProjectile)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = spellTarget.GetComponent<Health>();
            currentSpellRange = spellRange;
            currentDamage = spellDamage;
            currentProjectile = newProjectile;
            isCastSpell = true;
        }

        public void CastAOE(GameObject spellTarget,float spellRange, float spellDamage, GameObject AoeEffect)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = spellTarget.GetComponent<Health>();
            currentSpellRange = spellRange;
            currentDamage = spellDamage;
            currentAOEEffect = AoeEffect;
            isCastAOE = true;
        }

        private void TriggerSpell()
        {
            Debug.Log("Casting Spell");
            //animator.ResetTrigger("stopCast");
            if (isCastSpell)
            {
                animator.ResetTrigger("cast");
                animator.SetTrigger("cast");
            }
            if (isCastAOE)
            {
                animator.ResetTrigger("AOE");
                animator.SetTrigger("AOE");
            }
        }

        private void StopCast()
        {
            Debug.Log("Stop Casting");
            if (!isCastAOE)
            {
                animator.ResetTrigger("AOE");
            }
            if (!isCastSpell)
            {
                animator.ResetTrigger("cast");
            }
            animator.SetTrigger("stopCast");
        }

        private bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < currentSpellRange;
        }

        public void Shoot()
        {
            if (target == null) { return; }
            if (currentProjectile == null) return;
            Projectile newProjectile = Instantiate(currentProjectile, hand.position, Quaternion.identity);
            newProjectile.SetTarget(target, gameObject, currentDamage);
            isCastSpell = false;
            Cancel();
        }

        public void AOEDamage()
        {
            if (currentAOEEffect == null) return;
            GameObject newAoeEffect = Instantiate(currentAOEEffect,transform.position,Quaternion.identity);
            Collider[] colliders = Physics.OverlapSphere(transform.position, currentSpellRange);
            foreach (Collider c in colliders)
            {
                if (c.tag == "Enemy" && !c.GetComponent<Health>().IsDead())
                {
                    c.GetComponent<Health>().TakeDamage(gameObject, currentDamage);
                }
            }
            isCastAOE = false;
            Cancel();
            Destroy(newAoeEffect, 3f); 
        }

        private void SpellBehavior()
        {
            transform.LookAt(target.transform);
            TriggerSpell();
        }

        public void Cancel()
        {
            //StopCast();
            target = null;
            GetComponent<Mover>().Cancel();
        }
    }
}
