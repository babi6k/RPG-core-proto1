using GameDevTV.Inventories;
using RPG.Combat;
using UnityEngine;
namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = ("RPG/Inventory/Attack Button"))]
    public class AttackButton : ActionItem
    {
        public override void Use(GameObject user)
        {
            base.Use(user);
            var player = GameObject.FindWithTag("Player");
            var target = player.GetComponent<Targetter>().currentTarget;
            if (target == null)
            {
                player.GetComponent<Fighter>().TriggerAttack();
                Debug.Log("no target");
                return;
            }
            player.GetComponent<Fighter>().Attack(target.gameObject);
        }
    }
}
