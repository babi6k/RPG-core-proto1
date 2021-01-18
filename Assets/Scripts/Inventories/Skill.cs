using GameDevTV.Inventories;
using RPG.Attributes;
using RPG.Combat;
using UnityEngine;


[CreateAssetMenu(menuName = ("RPG/Inventory/Skill"))]
public class Skill : ActionItem 
{
    [SerializeField] float manaAmount = 10f;
    [SerializeField] float spellRange = 15f;
    [SerializeField] float spellDamage = 10f;
    [SerializeField] Projectile projectile = null;

    public override void Use(GameObject user)
    {
        base.Use(user);
        user.GetComponent<Mana>().UseMana(manaAmount);
        var player = GameObject.FindWithTag("Player");
        var target = player.GetComponent<Targetter>().currentTarget;
        if (target == null)
        {
            return;
        }
        player.GetComponent<Caster>().Cast(target.gameObject,spellRange,spellDamage,projectile);
    }
}
