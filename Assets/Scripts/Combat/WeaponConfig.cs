using UnityEngine;
using RPG.Attributes;
using GameDevTV.Inventories;
using RPG.Stats;
using System.Collections.Generic;
using UnityEditor;
using System;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "RPG/Weapons/Make New Weapon", order = 0)]
    public class WeaponConfig : EquipableItem, IModifierProvider
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Weapon equippedPrefab = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float percentageBonus = 0;
        [SerializeField] bool isRightHand = true;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";

        public float WeaponRange { get => weaponRange; }
        public float WeaponDamage { get => weaponDamage; }
        public float PercentageBonus { get => percentageBonus; }

        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            Weapon weapon = null;
            DestroyOldWeapon(rightHand, leftHand);
            if (equippedPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                weapon = Instantiate(equippedPrefab, handTransform);
                weapon.gameObject.name = weaponName;
            }
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
            return weapon;
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHand) handTransform = rightHand;
            else handTransform = leftHand;
            return handTransform;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Vector3 position = GetTransform(rightHand,leftHand).position;
            Projectile.Launch(projectile , position, target, instigator, calculatedDamage);
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return weaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return percentageBonus;
            }
        }

        public override string GetDescription()
        {
            string result = projectile ? "Ranged Weapon" : "Melle Weapon";
            result += $"\n\n{base.GetDescription()}\n";
            result += $"\nRange {weaponRange} meters";
            result += $"\nBase Damage {weaponDamage} points";
            if ((int)percentageBonus != 0)
            {
                string bonus = percentageBonus > 0 ? "<color=#8888ff>bonus</color>" : "<color=#ff8888>penalty</color>";
                result += $"\n{(int) percentageBonus} percent {bonus} to attack.";
            }
            return result;
        }

#if UNITY_EDITOR

        bool drawWeaponConfig = true;
        void SetWeaponRange(float newWeaponRange)
        {
            if (FloatEquals(weaponRange, newWeaponRange)) return;
            SetUndo("Set Weapon Range");
            weaponRange = newWeaponRange;
            Dirty();
        }

        void SetWeaponDamage(float newWeaponDamage)
        {
            if (FloatEquals(weaponDamage, newWeaponDamage)) return;
            SetUndo("Set Weapon Damage");
            weaponDamage = newWeaponDamage;
            Dirty();
        }

        void SetPercentageBonus(float newPercentageBonus)
        {
            if (FloatEquals(percentageBonus, newPercentageBonus)) return;
            SetUndo("Set Percentage Bonus");
            percentageBonus = newPercentageBonus;
            Dirty();
        }

        void SetIsRightHanded(bool newRightHanded)
        {
            if (isRightHand == newRightHanded) return;
            SetUndo(newRightHanded ? "Set as Right Handed" : "Set as Left Handed");
            isRightHand = newRightHanded;
            Dirty();
        }

        void SetAnimatorOverride(AnimatorOverrideController newOverride)
        {
            if (newOverride == animatorOverride) return;
            SetUndo("Change AnimatorOverride");
            animatorOverride = newOverride;
            Dirty();
        }

        void SetEquippedPrefab(Weapon newWeapon)
        {
            if (newWeapon == equippedPrefab) return;
            SetUndo("Set Equipped Prefab");
            equippedPrefab = newWeapon;
            Dirty();
        }

        void SetProjectile(Projectile possibleProjectile)
        {
            if (possibleProjectile == null) return;
            if (!possibleProjectile.TryGetComponent<Projectile>(out Projectile newProjectile)) return;
            if (newProjectile == projectile) return;
            SetUndo("Set Projectile");
            projectile = newProjectile;
            Dirty();
        }

        public override void DrawCustomInspector()
        {
            base.DrawCustomInspector();
            drawWeaponConfig = EditorGUILayout.Foldout(drawWeaponConfig, "WeaponConfig Data",foldoutStyle);
            if (!drawWeaponConfig) return;
            EditorGUILayout.BeginVertical(contentStyle);
            SetEquippedPrefab((Weapon)EditorGUILayout.ObjectField("Equipped Prefab", equippedPrefab, typeof(Weapon), false));
            SetWeaponDamage(EditorGUILayout.Slider("Weapon Damage", weaponDamage, 0, 100));
            SetWeaponRange(EditorGUILayout.Slider("Weapon Range", weaponRange, 1, 40));
            SetPercentageBonus(EditorGUILayout.IntSlider("Percentage Bonus", (int)percentageBonus, -10, 100));
            SetIsRightHanded(EditorGUILayout.Toggle("Is Right Handed", isRightHand));
            SetAnimatorOverride((AnimatorOverrideController)EditorGUILayout.ObjectField("Animator Override", animatorOverride, typeof(AnimatorOverrideController), false));
            SetProjectile((Projectile)EditorGUILayout.ObjectField("Projectile", projectile, typeof(Projectile), false));
            EditorGUILayout.EndVertical();
        }

        public override bool IsLocationSelectable(Enum location)
        {
            EquipLocation candidate = (EquipLocation)location;
            return candidate == EquipLocation.Weapon;
        }

#endif
    }
}