using System;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Attributes;
using RPG.Core;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Abilities
{
    [CreateAssetMenu(fileName = "MyAbility", menuName = "Abilities/Ability", order = 0)]
    public class Ability : ActionItem
    {
        [SerializeField] TargetingStrategy targeting;
        [SerializeField] FilterStrategy[] filters;
        [SerializeField] EffectStrategy[] effects;
        [SerializeField] float effectScale = 1;
        [SerializeField] float manaCost = 0;
        [SerializeField] string animatorTrigger = null;
        [SerializeField] bool turnToTarget = false;
        [SerializeField] int actionPriority = 3;
        [SerializeField] int cancelAllActionsLowerThan = 3;

        public override void Use(GameObject user)
        {
            if (manaCost > 0)
            {
                var mana = user.GetComponent<Mana>();
                if (mana.GetMana() < manaCost) return;
            }
            var cooldownManger = user.GetComponent<CoolDownManager>();
            if (cooldownManger && cooldownManger.GetTimeRemaining(GetItemID()) > 0)
            {
                return;
            }

            ActionScheduler scheduler = user.GetComponent<ActionScheduler>();
            scheduler.StartAction(new AbilityAction(this, scheduler, user), actionPriority, cancelAllActionsLowerThan);
        }

        public class AbilityAction : IAction
        {
            Ability parent;
            ActionScheduler scheduler;
            GameObject user;
            List<IAction> activeActions = new List<IAction>();

            public AbilityAction(Ability newParent, ActionScheduler newScheduler, GameObject newUser)
            {
                parent = newParent;
                scheduler = newScheduler;
                user = newUser;
            }
            public void Activate()
            {
                if (parent.targeting != null)
                {
                    var targetingData = new TargetingData(parent.effectScale, user);
                    TrackAndActivate(parent.targeting.MakeAction(targetingData, TargetAquired));
                }
            }

            public void Cancel()
            {
                foreach (var action in activeActions)
                {
                    action.Cancel();
                }
                activeActions.Clear();
            }

            private void TargetAquired(TargetingData data)
            {
                activeActions.Clear();

                if (parent.manaCost > 0)
                {
                    var mana = data.GetSource().GetComponent<Mana>();
                    if (!mana.UseMana(parent.manaCost)) return;
                }
                var cooldownManager = data.GetSource().GetComponent<CoolDownManager>();
                if (cooldownManager)
                {
                    cooldownManager.StartCoolDown(parent.GetItemID(), parent.GetCoolDownTime());
                }
                foreach (var filter in parent.filters)
                {
                    data.SetTargets(filter.Filter(data.GetTargets()));
                }

                if (!String.IsNullOrWhiteSpace(parent.animatorTrigger))
                {
                    var animator = data.GetSource().GetComponent<Animator>();
                    animator.SetTrigger(parent.animatorTrigger);
                }

                if (parent.turnToTarget)
                {
                    data.GetSource().transform.LookAt(data.GetTargetPoint());
                }

                foreach (var effect in parent.effects)
                {
                    TrackAndActivate(effect.MakeAction(data, EffectFinished));
                }
            }

            private void EffectFinished()
            {
                activeActions.Clear();
                scheduler.FinishAction(this);
            }

            private void TrackAndActivate(IAction action)
            {
                activeActions.Add(action);
                action.Activate();
            }
        }
    }
}
