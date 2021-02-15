﻿using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class AggroGroup : MonoBehaviour
    {
        [SerializeField] Fighter[] fighters;
        [SerializeField] bool activateOnStart = false;

        private void Start() 
        {
            Activate(activateOnStart);
        }

        public void Activate(bool shouldActivate)
        {
            foreach (Fighter fighter in fighters)
            {
                fighter.enabled = shouldActivate;
                fighter.GetComponent<AIController>().enabled = shouldActivate;
                fighter.GetComponent<CombatTarget>().enabled = shouldActivate;
            }
        }
    }
}
