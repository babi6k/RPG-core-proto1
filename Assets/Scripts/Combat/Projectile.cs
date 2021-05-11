using UnityEngine;
using RPG.Attributes;
using UnityEngine.Events;
using System;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] float speed = 1f;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 10;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 2;
        [SerializeField] UnityEvent onHit;
        [SerializeField] Transform effectSpawner;
        [Header("Effects")]
        [SerializeField] Transform effectProejctile;
        [SerializeField] float effectSize;

        Vector3 targetPoint;
        Health target = null;
        GameObject instigator = null;
        float damage = 0;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        void Update()
        {
            if (isHoming && target != null && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            SetTarget(instigator, damage, target);
        }

        public void SetTarget(Vector3 targetPoint, GameObject instigator, float damage)
        {
            SetTarget(instigator, damage, null, targetPoint);
        }


        private void SetTarget(GameObject instigator, float damage, Health target = null, Vector3 targetPoint = default)
        {
            this.targetPoint = targetPoint;
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
            if (effectProejctile != null)
            {
                var effect = Instantiate(effectProejctile,transform.position,Quaternion.identity);
                //effect.position = effectPos;
                effect.localScale = new Vector3(effectSize,effectSize,effectSize);
                effect.parent = effectSpawner;
            }

            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            if (target == null)
            {
                return targetPoint;
            }
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            Health health = other.GetComponent<Health>();
            if (target != null && health != target) return;
            if (health == null || health.IsDead()) return;
            if (other.gameObject == instigator) return;

            health.TakeDamage(instigator, damage);
            speed = 0;
            onHit.Invoke();

            if (hitEffect != null)
            {
                var hitEffectInstance = Instantiate(hitEffect, GetAimLocation(), transform.rotation);
                hitEffectInstance.transform.parent = transform;
            }

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }
    }
}
