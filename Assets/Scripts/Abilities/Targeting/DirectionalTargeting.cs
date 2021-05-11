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
        [SerializeField] Texture2D cursorTexture;
        [SerializeField] Vector2 cursorHotspot;

        public override void StartTargeting(AbilityData data, Action finished)
        {
            PlayerController playerController = data.GetUser().GetComponent<PlayerController>();
            playerController.StartCoroutine(Targeting(data, playerController, finished));
        }


        private IEnumerator Targeting(AbilityData data, PlayerController playerController, Action finished)
        {
            playerController.enabled = false;
            while (!data.IsCancelled())
            {
                Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);

                RaycastHit mouseHit;
                Ray ray = PlayerController.GetMouseRay();
                if (Physics.Raycast(ray, out mouseHit, 100, castingLayer))
                {

                    if (Input.GetMouseButtonDown(0))
                    {
                        // Capture the whole of this mouse click so we don't move
                        yield return new WaitWhile(() => Input.GetMouseButtonDown(0));
                        data.SetTargetedPoint(mouseHit.point + groundOffset / ray.direction.y * ray.direction);
                        break;
                    }
                }
                yield return null;
            }
            playerController.enabled = true;
            finished();
        }
    }
}
