using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Abilities
{
    public abstract class TargetingStrategy : ScriptableObject
    {
       public abstract IAction MakeAction(TargetingData data, Action<TargetingData> callback);
    }
}
