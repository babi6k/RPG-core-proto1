using System;
using GameDevTV.Utils;
using GameDevTV.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour,ISaveable
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] ScriptableClass characterClass;
        [SerializeField] GameObject levelUpParticleEffect = null;
        [SerializeField] bool shouldUseModifiers = false;

        public event Action onLevelUp;

        LazyValue <int> currentLevel;

        Experience experience;

        private void Awake()
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }


        private void Start()
        {
            currentLevel.ForceInit();
        }

        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                LevelUpEffect();
                onLevelUp();
            }    
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
            
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat)/100);
        }


        private float GetBaseStat(Stat stat)
        {
            return characterClass.GetStat(stat,currentLevel.value);
        }

        public Experience GetExperience()
        {
            return experience;
        }


        public int GetLevel()
        {
            return currentLevel.value;
        }

        private int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;

            float currentXp = experience.GetExperience();
            if (currentXp == 0) return startingLevel;

            var xpToLevelUp = characterClass.GetStat(Stat.ExperienceToLevelUp, currentLevel.value);
            if (xpToLevelUp > currentXp)
            {
                return currentLevel.value;
            }
            return currentLevel.value + 1;
        }


        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float total = 0;
           foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifers in provider.GetAdditiveModifiers(stat))
                {
                    total += modifers;
                }
            }
            return total;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifers in provider.GetPercentageModifers(stat))
                {
                    total += modifers;
                }
            }
            return total;

        }

        public object CaptureState()
        {
            return currentLevel.value;
        }

        public void RestoreState(object state)
        {
            currentLevel.value = (int) state;
        }
    }
}
