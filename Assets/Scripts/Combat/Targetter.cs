using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    public class Targetter : MonoBehaviour
    {
        public CombatTarget currentTarget;

        //[SerializeField] int targetCount = 9;
        List<CombatTarget> combatTargets = new List<CombatTarget>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Enemy" && !other.GetComponent<Health>().IsDead())
            {
                combatTargets.Add(other.GetComponent<CombatTarget>());
            }
        }

        private void Update()
        {
            if (!currentTarget) return;
            if (currentTarget.GetComponent<Health>().IsDead())
            {
                for(int i = 0; i < combatTargets.Count; i++)
                {
                    if (!combatTargets[i].GetComponent<Health>().IsDead())
                    {
                        currentTarget = combatTargets[i];
                        return;
                    }
                }
                GetComponent<Fighter>().Cancel();
                combatTargets.Clear();
                currentTarget = null;
            }
        }
    }
}
