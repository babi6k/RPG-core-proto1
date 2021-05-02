using System;
using RPG.Core;
using UnityEngine;
namespace RPG.Abilities
{
    public abstract class EffectStrategy : ScriptableObject
    {
        public abstract IAction MakeAction(TargetingData data, Action complete);
    }
}
