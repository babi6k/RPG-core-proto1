using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;
using RPG.Combat;
using RPG.Inventories;

[CreateAssetMenu(menuName = ("RPG/Inventory/Spell"))]
public class Spell : Skill
{
     [SerializeField] Projectile projectile = null;

    public override void Use(GameObject user)
    {
        base.Use(user);
        user.GetComponent<Mana>().UseMana(GetManaAmount());
        //var player = GameObject.FindWithTag("Player");
        var target = user.GetComponent<Targetter>().currentTarget;
        if (target == null)
        {
            return;
        }
        user.GetComponent<Caster>().CastSpell(target.gameObject,GetSpellRange(),GetSpellDamage(),projectile);
    }
}
