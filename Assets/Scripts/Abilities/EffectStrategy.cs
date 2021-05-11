using System;
using RPG.Core;
using UnityEngine;
namespace RPG.Abilities
{
    public abstract class EffectStrategy : ScriptableObject
    {
        public abstract void StartEffect(AbilityData data, Action complete);
    }
}
