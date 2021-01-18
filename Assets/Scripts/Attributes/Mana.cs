using UnityEngine;
using GameDevTV.Saving;
using RPG.Stats;
using GameDevTV.Utils;
using UnityEngine.Events;


namespace RPG.Attributes
{

    public class Mana : MonoBehaviour, ISaveable
    {
        [SerializeField] float manaRegenTime = 5;
        [SerializeField] float regenartionPercentage = 70;
        [SerializeField] LazyValue<float> manaPoints;
     

        //Cached
        float timeSinceLastRegen = Mathf.Infinity;

        private void Update()
        {
            timeSinceLastRegen += Time.deltaTime;
            if (timeSinceLastRegen > manaRegenTime)
            {
                RestoreMana(Mathf.Round(GetMaxManaPoints() * 0.05f));
                timeSinceLastRegen = 0;
            }
        }

        private void Awake()
        {
            manaPoints = new LazyValue<float>(GetInitialMana);
        }

        private float GetInitialMana()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Mana);
        }

        private void Start()
        {
            manaPoints.ForceInit();
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateMana;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateMana;
        }

        private void RegenerateMana()
        {
            float regenMP = GetComponent<BaseStats>().GetStat(Stat.Mana) * (regenartionPercentage / 100);
            manaPoints.value = Mathf.Max(manaPoints.value, regenMP);
        }

        public void UseMana(float manaUsed)
        {
            manaPoints.value = Mathf.Max(manaPoints.value - manaUsed, 0);  
        }

        public void RestoreMana(float manaToRestore)
        {
            manaPoints.value = Mathf.Min(manaPoints.value + manaToRestore, GetMaxManaPoints());
        }

        public float GetMaxManaPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Mana);
        }

        public float GetManaPoints()
        {
            return manaPoints.value;
        }

        public float GetFraction()
        {
            return manaPoints.value / GetComponent<BaseStats>().GetStat(Stat.Mana);
        }

        public object CaptureState()
        {
            return manaPoints.value;
        }

        public void RestoreState(object state)
        {
            manaPoints.value = (float)state;
        }
    }
}
