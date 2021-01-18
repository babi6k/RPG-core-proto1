using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "ScriptabClass", menuName = "Class/New Class", order = 0)]
    public class ScriptableClass : ScriptableObject
    {
        [SerializeField] string className = null;
        [SerializeField] FormulaStat[] formulas;
        Dictionary<Stat, FormulaStat> lookupTable = null;

        void BuildLookup()
        {
            if (lookupTable != null) return;
            lookupTable = new Dictionary<Stat, FormulaStat>();
            foreach (FormulaStat formulaStat in formulas)
            {
                lookupTable[formulaStat.GetStat()] = formulaStat;
            }
        }

        public float GetStat(Stat stat , int level)
        {
            BuildLookup();

            return lookupTable[stat].GetCurrentValue(level);
        }

        [System.Serializable]
        public class FormulaStat
        {
            [SerializeField] Stat stat;
            [SerializeField] float currentValue = 0;
            [SerializeField] float baseValue;
            [SerializeField] float adderPerLevel;
            [SerializeField] float multiplierPerLevel;
            [SerializeField] bool isComplex = false;

            public float GetCurrentValue(int level)
            {
                if (isComplex)
                {
                    currentValue = CalculateComplex(level);
                    return currentValue;
                }
                currentValue = CalculateSimple(level);
                return currentValue;

            }

            private float CalculateComplex(int level)
            {
                if (level <= 0)
                {
                    return 0;
                }
                if (level >= 1 && level < 11)
                {
                    return (baseValue * level * level) + (adderPerLevel * level);
                }
                else if (level >= 11 && level < 30)
                {
                    return (float)(-0.4 * level * level * level + 40.4 * level * level + 396 * level);
                }

                return (float)((65 * level * level - 165 * level - 6750) * 0.82);
            }

            private float CalculateSimple(int level)
            {
                return (baseValue + adderPerLevel * level)* multiplierPerLevel;
            }

            public Stat GetStat()
            {
                return stat;
            }

        }

    }
}
