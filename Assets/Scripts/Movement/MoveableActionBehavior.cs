using RPG.Attributes;
using RPG.Core;
using UnityEngine;

namespace RPG.Movement
{
    public abstract class MoveableActionBehavior<T> : MonoBehaviour,IAction where T:MonoBehaviour
    {
        protected Health health;
        protected Mover mover;
        protected ActionScheduler actionScheduler;
        protected T target = null;
        protected float acceptanceRadius = 3.0f;

        protected virtual void Awake() 
        {
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        public virtual void StartAction(T newTarget)
        {
            actionScheduler.StartAction(this);
            target = newTarget;
        }

        protected virtual void Update()
        {
            if (health.IsDead())
            {
                actionScheduler.CancelCurrentAction();
                enabled = false;
                return;
            }
            if (target == null) return;
            if (!IsValidAction()) return;
            if (!(Vector3.Distance(transform.position, target.transform.position)<acceptanceRadius))
            {
                mover.MoveTo(target.transform.position,1f);
                HandleOutOfRange();
            }
            else
            {
                mover.Cancel();
                Perform();
            }
        }

        protected abstract void Perform();

        protected virtual bool IsValidAction()
        {
            return true;
        }

        /// <summary>
        /// Override to manage things like canceling an animation state if the character moves out of range.
        /// </summary>
        protected virtual void HandleOutOfRange()
        {

        }

        public void Cancel()
        {
            target = null;
        }
    }
}
