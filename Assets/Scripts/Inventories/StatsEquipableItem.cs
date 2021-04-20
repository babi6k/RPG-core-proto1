using UnityEngine;
using GameDevTV.Inventories;
using RPG.Stats;
using System.Collections.Generic;
using UnityEditor;
using System;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = ("RPG/Inventory/Equipable Item"))]
    public class StatsEquipableItem : EquipableItem, IModifierProvider
    {
        [SerializeField]
        List<Modifier> addtiveModifers = new List<Modifier>();
        [SerializeField]
        List<Modifier> percentageModifiers = new List<Modifier>();

        [System.Serializable]
        struct Modifier
        {
            public Stat stat;
            public float value;
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            foreach (var modifier in addtiveModifers)
            {
                if (modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }

        public IEnumerable<float> GetPercentageModifers(Stat stat)
        {
            foreach (var modifier in percentageModifiers)
            {
                if (modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }

        string FormatAttribute(Modifier mod, bool percent)
        {
            if ((int)mod.value == 0.0f) return "";
            string percentString = percent ? "percent" : "point";
            string bonus = mod.value > 0.0f ? "<color=#8888ff>bonus</color>" : "<color=#ff8888>penalty</color>";
            return $"{Mathf.Abs((int) mod.value)} {percentString} {bonus} to {mod.stat}\n";
        }

        public override string GetDescription()
        {
            Debug.Log("Creating Description");
            string result =  base.GetDescription()+"\n";
            foreach (Modifier mod in addtiveModifers)
            {
                result += FormatAttribute(mod, false);
            }

            foreach (Modifier mod in percentageModifiers)
            {
                result += FormatAttribute(mod, true);
            }
            return result;
        }

#if UNITY_EDITOR
        bool drawStatsEquipableItemData = true;
        bool drawAdditive = true;
        bool drawPercentage = true;

        public override void DrawCustomInspector()
        {
            base.DrawCustomInspector();
            drawStatsEquipableItemData = EditorGUILayout.Foldout
            (drawStatsEquipableItemData, "StatsEquipableItemData",foldoutStyle);
            if (!drawStatsEquipableItemData) return;
            EditorGUILayout.BeginVertical(contentStyle);
            EditorGUILayout.EndVertical();
            drawAdditive = EditorGUILayout.Foldout(drawAdditive, "Additive Modifiers");
            if (drawAdditive)
            {
                DrawModifierList(addtiveModifers);
            }
            EditorGUILayout.BeginVertical(contentStyle);
            EditorGUILayout.EndVertical();
            drawPercentage = EditorGUILayout.Foldout(drawPercentage, "Percentage Modifiers");
            if (drawPercentage)
            {
                DrawModifierList(percentageModifiers);
            }
            EditorGUILayout.BeginVertical(contentStyle);
            EditorGUILayout.EndVertical();

        }
        void AddModifier (List<Modifier> modifierList)
        {
            SetUndo("Add Modifier");
            modifierList?.Add(new Modifier());
            Dirty();
        }

        void RemoveModifier(List<Modifier> modiferList, int index)
        {
            SetUndo("Remove Modifier");
            modiferList?.RemoveAt(index);
            Dirty();
        }

        void SetStat(List<Modifier> modifierList, int i , Stat stat)
        {
            if (modifierList[i].stat == stat) return;
            SetUndo("Change Modifier Stat");
            Modifier mod = modifierList[i];
            mod.stat = stat;
            modifierList[i] = mod;
            Dirty();
        }

        void SetValue(List<Modifier> modifierList, int i , float value)
        {
            if (modifierList[i].value == value) return;
            SetUndo("Change Modifier Stat");
            Modifier mod = modifierList[i];
            mod.value = value;
            modifierList[i] = mod;
            Dirty();
        }

        void DrawModifierList(List<Modifier> modifierList)
        {
            int modiferToDelete = -1;
            GUIContent statLabel = new GUIContent("Stat");
            for (int i = 0; i < modifierList.Count; i++)
            {
                Modifier modifier = modifierList[i];
                EditorGUILayout.BeginHorizontal();
                SetStat(modifierList, i, (Stat)EditorGUILayout.EnumPopup(statLabel, modifier.stat, IsStatSelectable, false));
                SetValue(modifierList, i, EditorGUILayout.IntSlider("Value", (int)modifier.value, -20, 100));
                if (GUILayout.Button("-"))
                {
                    modiferToDelete = i;
                }

                EditorGUILayout.EndHorizontal();
            }

            if (modiferToDelete > -1)
            {
                RemoveModifier(modifierList,modiferToDelete);
            }

            if (GUILayout.Button("Add Modifier"))
            {
                AddModifier(modifierList);
            }
        }

        bool IsStatSelectable(Enum candidate)
        {
            Stat stat = (Stat) candidate;
            if (stat == Stat.ExperienceReward || stat == Stat.ExperienceToLevelUp) return false;
            return true;
        }
#endif
    }
}
