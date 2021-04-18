using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Combat;
using RPG.Inventories;
using UnityEngine;

[CreateAssetMenu(menuName = ("RPG/Inventory/AOESpell"))]
public class AOESpell : Skill
{
   [SerializeField] GameObject AoeEffect = null;

    public override void Use(GameObject user)
    {
        base.Use(user);
        user.GetComponent<Mana>().UseMana(GetManaAmount());
        var target = user.GetComponent<Targetter>().currentTarget;
        if (target == null)
        {
            return;
        }
        user.GetComponent<Caster>().CastAOE(target.gameObject,GetSpellRange(),GetSpellDamage(),AoeEffect);
    }
}
