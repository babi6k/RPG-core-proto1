using UnityEngine;
using GameDevTV.Inventories;

namespace RPG.Control
{
    [RequireComponent(typeof(Pickup))]
    public class ClickablePickup : MonoBehaviour, IRaycastable
    {
        Pickup pickup;

        private void Awake()
        {
            pickup = GetComponent<Pickup>();
        }

        public CursorType GetCursorType()
        {
            if (pickup.CanBePickedUp())
            {
                return CursorType.Pickup;
            }
            else
            {
                return CursorType.FullPickup;
            }
        }

        public bool HandleRayCast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<ItemCollector>().StartAction(pickup);
            }
            return true;
        }
    }
}