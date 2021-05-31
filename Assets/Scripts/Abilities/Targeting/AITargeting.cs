using System;
using RPG.Abilities;
using UnityEngine;

[CreateAssetMenu(fileName = "AI Targeting", menuName = "Abilities/Targeting/AITargeting", order = 0)]
public class AITargeting : TargetingStrategy
{
    public override void StartTargeting(AbilityData data, Action finished)
    {
        var player = GameObject.FindWithTag("Player");
        data.SetTargetedPoint(player.transform.position);
        data.SetTargets(new GameObject[]{player});
        finished();
    }
}
