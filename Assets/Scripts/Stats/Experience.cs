using UnityEngine;
using GameDevTV.Saving;
using System;

namespace RPG.Stats
{

    public class Experience : MonoBehaviour ,ISaveable 
    {
        [SerializeField] float experiencePoints = 0;

        public event Action onExperienceGained;

        private void Update() 
        {
            if (Input.GetKey(KeyCode.E))
            {
                GainExperience(Time.deltaTime * 1000);
            }    
        }

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            onExperienceGained();
        }

        public float GetExperience()
        {
            return experiencePoints;
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public float GetFraction()
        {
            return experiencePoints / GetMaxExperience();
        }

        public float GetMaxExperience()
        {
            return GetComponent<BaseStats>().GetStat(Stat.ExperienceToLevelUp);
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}
