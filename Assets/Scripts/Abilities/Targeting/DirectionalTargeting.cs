using System;
using System.Collections;
using RPG.Abilities.Helpers;
using RPG.Control;
using RPG.Core;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Directional Targeting", menuName = "Abilities/Targeting/Directional", order = 0)]
    public class DirectionalTargeting : TargetingStrategy
    {
        [SerializeField] LayerMask castingLayer;
        [SerializeField] float groundOffset = 1;
        [SerializeField] Texture2D effectCursor;
        [SerializeField] Vector2 cursorHotspot;
        [SerializeField] float areaOfEffectRadius;

         public override IAction MakeAction(TargetingData data, Action<TargetingData> callback)
        {
            return new TargetingAction(this,data,callback);
        }

        class TargetingAction : IAction
        {
            PlayerController playerController = null;
            DirectionalTargeting strategy;
            private readonly TargetingData data;
            private readonly Action<TargetingData> callback;
            Coroutine targetingRoutine;

            public TargetingAction(DirectionalTargeting newStrategy, TargetingData newData, Action<TargetingData> newCallback)
            {
                strategy = newStrategy;
                data = newData;
                callback = newCallback;
            }

            public void Activate()
            {
                playerController = data.GetSource().GetComponent<PlayerController>();
                targetingRoutine = playerController.StartCoroutine(Targeting(data, callback));
            }

            public void Cancel()
            {
               playerController.StopCoroutine(targetingRoutine);
               playerController.enabled = true;
            }

             private IEnumerator Targeting(TargetingData data, Action<TargetingData> callback)
            {
                playerController.enabled = false;
                while (true)
                {
                    Cursor.SetCursor(strategy.effectCursor, strategy.cursorHotspot, CursorMode.Auto);

                    RaycastHit mouseHit;
                    Ray ray = PlayerController.GetMouseRay();
                    if (Physics.Raycast(ray, out mouseHit, 100, strategy.castingLayer))
                    {

                        if (Input.GetMouseButtonDown(0))
                        {
                            data.SetTarget(mouseHit.point + strategy.groundOffset / ray.direction.y * ray.direction);
                            // Capture the whole of this mouse click so we don't move
                            yield return new WaitWhile(() => Input.GetMouseButtonDown(0));
                            if (callback != null) callback(data);
                            Cancel();
                            yield break;
                        }
                    }
                    yield return null;
                }
            }
        }
    }
}
