using RPG.Control;
using UnityEngine;

namespace RPG.Inventories
{
    public class CoinDrop : MonoBehaviour, IRaycastable
    {
        float amount = 20;
        Vector3 location;
        [SerializeField] AudioSource audioSource;

        public void DropGold(float amount, Vector3 dropLocation)
        {
            this.amount = amount;
            transform.position = dropLocation;
            location = dropLocation;
        }

        private void AddGold(PlayerController user)
        {
            var money = user.GetComponent<Money>();
            money.UpdateBalance(amount);
            Destroy(gameObject,2f);
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }

        public bool HandleRayCast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                audioSource.Play();
                AddGold(callingController);
            }
            return true;
        }
    }
}
