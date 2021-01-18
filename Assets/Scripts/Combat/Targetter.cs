using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    public class Targetter : MonoBehaviour
    {
        public CombatTarget currentTarget;

        [SerializeField] int targetCount = 9;
        CombatTarget[] combatTargets;
        int index = 0;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Enemy" && !other.GetComponent<Health>().IsDead())
            {
                combatTargets[index] = other.GetComponent<CombatTarget>();
                index++;
                print(index);
            }
        }

        private void Awake()
        {
            combatTargets = new CombatTarget[targetCount];
        }

        private void Update()
        {
            if (!currentTarget) return;
            if (currentTarget.GetComponent<Health>().IsDead())
            {
                for (int i = 0; i < index; i++)
                {
                    if (!combatTargets[i].GetComponent<Health>().IsDead())
                    {
                        currentTarget = combatTargets[i];
                        return;
                    }
                   
                }
                print(index);
                GetComponent<Fighter>().Cancel();
                index = 0;
                currentTarget = null;
            }
        }
    }

    
}
