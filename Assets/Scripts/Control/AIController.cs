using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using GameDevTV.Utils;
using System;
using RPG.Abilities;

namespace RPG.Control
{

    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] float aggroCooldownTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;
        [SerializeField] float guardAttackSpeedFraction = 0.5f;
        [SerializeField] float shoutDistance = 5f;
        [SerializeField] Ability ability;

        //Cached ref
        Fighter fighter;
        Health health;
        GameObject player;
        Mover mover;

        LazyValue<Vector3> guardPosition;
        int currentWaypointIndex = 0;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        float timeSinceAggrevated = Mathf.Infinity;


        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
            guardPosition = new LazyValue<Vector3>(GetGuardPostion);
        }

        private Vector3 GetGuardPostion()
        {
            return transform.position;
        }

        private void Start()
        {
            guardPosition.ForceInit();
        }


        private void Update()
        {
            if (health.IsDead()) { return; }
            //GameObject player = GameObject.FindWithTag("Player");

            if (InChaseRangeOfPlayer() && !player.GetComponent<Health>().IsDead())
            {
                //Chase state
                ChaseBehaviour();
            }

            else if (IsAggrevated() && fighter.CanAttack(player))
            {
                //Attack state
                AttackBehaviour(player);
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                //Suspicion state
                SuspicionBehaviour();
            }
            else
            {
                //Guard state
                PatrolBehaviour();
            }
            UpdateTimers();
        }

        public void Aggrevate()
        {
            timeSinceAggrevated = 0;
        }

        private bool InChaseRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return (distanceToPlayer <= chaseDistance && distanceToPlayer > fighter.GetWeapon().WeaponRange);
                
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;

            if(patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition,patrolSpeedFraction);
            }
            
        }


        //Waypoint methods for PatrolGuards

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        //Enemy behavior 

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour(GameObject player)
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
            if (ability)
            {
                ability.Use(gameObject);
            }

            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai == null) continue;
                ai.Aggrevate();
            }
        }

        private void ChaseBehaviour ()
        {
            mover.StartMoveAction(player.transform.position, guardAttackSpeedFraction);
        }

        private bool IsAggrevated()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer <= fighter.GetWeapon().WeaponRange || timeSinceAggrevated < aggroCooldownTime;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
            
        }
    }
}
