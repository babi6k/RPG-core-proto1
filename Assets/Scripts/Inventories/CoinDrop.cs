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

        public void AddGold()
        {
            var money = GameObject.FindWithTag("Player").GetComponent<Money>();
            money.UpdateBalance(amount);
            audioSource.Play();
            Destroy(gameObject,0.5f);
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }

        public bool HandleRayCast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<GoldCollector>().StartAction(this);
            }
            return true;
        }
    }
}
