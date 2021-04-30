﻿using UnityEngine;
using RPG.Core;
using RPG.Movement;
using GameDevTV.Saving;
using RPG.Attributes;
using RPG.Stats;
using GameDevTV.Utils;
using System;
using GameDevTV.Inventories;

namespace RPG.Combat
{

    public class Fighter : MonoBehaviour, IAction,ISaveable
    {
        [SerializeField] float timeBetweenAttacks = 0.7f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;

        Health target;
        Equipment equipment;
        ActionScheduler actionScheduler;
        Animator animator;
        float timeSinceLastAttack = Mathf.Infinity;
        WeaponConfig currentWeaponConfig;
        LazyValue <Weapon> currentWeapon;

        private void Awake()
        {
            actionScheduler = GetComponent<ActionScheduler>();
            equipment = GetComponent<Equipment>();
            animator = GetComponent<Animator>();
            if (equipment)
            {
                equipment.equipmentUpdated += UpdateWeapon;
            }
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

      
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            if (target.IsDead()) return;
            if (!actionScheduler.IsCurrentAction(this)) return;
            if (!GetIsInRange(target.transform))
            {//Moving to range to attack
                GetComponent<Mover>().MoveTo(target.transform.position,1f);  
            }
            else
            {
                //In range of attack Stoping the movement
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }

        public WeaponConfig GetWeapon()
        {
            return currentWeaponConfig;
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // This will trigger the Hit event
                TriggerAttack();
                timeSinceLastAttack = 0;
            }

        }

        public void TriggerAttack()
        {
            animator.ResetTrigger("stopAttack");
            animator.SetTrigger("attack");
        }
        
        //Animation Event
        public void Hit()
        {
            if (target == null) { return; }

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if (currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }

            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target,gameObject,damage);
            }
            else
            {
                target.TakeDamage(gameObject ,damage);
            }
            
        }

        public void Shoot()
        {
            Hit();
        }

        public void SetHandTransfroms(Transform right, Transform left)
        {
            rightHandTransform = right;
            leftHandTransform = left;
            UpdateWeapon();
        }

        private bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < currentWeaponConfig.WeaponRange;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) &&
               !GetIsInRange(combatTarget.transform ))
            {
                return false;
            }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            actionScheduler.StartAction(this, 1, 2);
            target = combatTarget.GetComponent<Health>();
        }

        //Interface IAction
        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }
        
        private void StopAttack()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
        }

        //Weapon Part

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        public void UpdateWeapon()
        {
            if (!equipment) return;
            var weapon = equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig;
            if (weapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
            else
            {
                EquipWeapon(weapon);
            }
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            animator = GetComponent<Animator>();
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget()
        {
            return target;
        }

        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;   
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }

        public void Activate()
        {
            
        }
    }
}
