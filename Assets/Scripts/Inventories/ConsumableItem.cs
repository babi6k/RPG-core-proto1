﻿using System;
using GameDevTV.Inventories;
using RPG.Attributes;
using RPG.Stats;
using UnityEditor;
using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = ("RPG/Inventory/Consumable Item"))]
    public class ConsumableItem : ActionItem
    {
        [SerializeField] float consumeValue = 0;
        [SerializeField] Stat stat;


        public override void Use(GameObject user)
        {
            base.Use(user);
            if (stat == Stat.Health)
            {
                user.GetComponent<Health>().Heal(consumeValue);
            }

            if (stat == Stat.Mana)
            {
                user.GetComponent<Mana>().RestoreMana(consumeValue);
            }

        }

        public override string GetDescription()
        {
            Debug.Log("Creating Description");
            string result = base.GetDescription()+"\n";
            result += $"This potion will restore {(int)consumeValue} {stat} points";
            return result;
        }

#if UNITY_EDITOR
        void SetconsumeValue(float value)
        {
            if (FloatEquals(consumeValue, value)) return;
            SetUndo("Change Amount To Heal");
            consumeValue = value;
            Dirty();
        }

        void SetStatConsume(Stat value)
        {
            if (stat == value) return;
            SetUndo("Set stat to consume");
            stat = value;
        }

        bool drawHealingData = true;
        public override void DrawCustomInspector()
        {
            base.DrawCustomInspector();
            drawHealingData = EditorGUILayout.Foldout(drawHealingData, "Cosumable Data");
            if (!drawHealingData) return;
            EditorGUILayout.BeginVertical(contentStyle);
            SetconsumeValue(EditorGUILayout.IntSlider("Amount to Consume", (int)consumeValue, 1, 100));
            SetStatConsume((Stat)EditorGUILayout.EnumPopup(new GUIContent("Choose Stat"),
             stat, IsStatSelectable, false));
            EditorGUILayout.EndVertical();
        }

        bool IsStatSelectable(Enum candidate)
        {
            Stat stat = (Stat) candidate;
            if (stat == Stat.ExperienceReward || stat == Stat.ExperienceToLevelUp || stat == Stat.Damage) return false;
            return true;
        }

#endif

    }
}
