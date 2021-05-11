using System;
using RPG.Abilities.Helpers;
using RPG.Core;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "SelfTargeting", menuName = "Abilities/Targeting/Self", order = 0)]
    public class SelfTargeting : TargetingStrategy
    {
        public override void StartTargeting(AbilityData data, Action finished)
        {
            data.SetTargets(new GameObject[]{data.GetUser()});
            data.SetTargetedPoint(data.GetUser().transform.position);
            finished();
        }
    }
}
