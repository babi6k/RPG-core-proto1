using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Abilities
{
    public class TargetingData
    {
        float effectScale;
        GameObject source;
        Vector3 targetPoint;
        IEnumerable<GameObject> targetGameObjects;

        public TargetingData(float newEffectScale, GameObject newSource)
        {
            effectScale = newEffectScale;
            source = newSource;
        }

        public void SetTarget(Vector3 target)
        {
            targetPoint = target;
        }

        public Vector3 GetTargetPoint()
        {
            return targetPoint;
        }

        public void SetTargets(IEnumerable<GameObject> targets)
        {
            targetGameObjects = targets;
        }

        public float GetEffectScaling()
        {
            return effectScale;
        }

        public MonoBehaviour GetCorutineOwner()
        {
            return source.GetComponent<CoolDownManager>();
        }

        public GameObject GetSource()
        {
            return source;
        }

        public IEnumerable<GameObject> GetTargets()
        {
            return targetGameObjects;
        }
    }
}
