using UnityEngine;

namespace RDE
{
    public class SingleShot : MonoBehaviour
    {
        private PlayerManager playerManager;

        [Header("Bullet Properties")]
        [SerializeField] private GameObject impactEffect;
        [SerializeField] private float speed = 20f;
        [SerializeField] private float damage = 10f;
        [SerializeField] private float lifetime = 2f;

        [Header("Helper Variables")]
        private Rigidbody2D rb;

        private void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>();
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.velocity = transform.up * speed;

            if (playerManager != null)
            {
                Collider2D playerCollider = playerManager.GetComponent<Collider2D>();
                Collider2D bulletCollider = GetComponent<Collider2D>();
                if (playerCollider != null && bulletCollider != null)
                {
                    Physics2D.IgnoreCollision(bulletCollider, playerCollider);
                }
            }

            Destroy(gameObject, lifetime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var target = collision.collider.GetComponent<CharacterStatManager>();
            if (target != null && !target.GetComponent<CharacterManager>().isDead)
            {
                damage *= playerManager.playerCombatManager.TryCriticalHit();
                target.InstantHealth(-damage);
                Debug.Log(damage + " damage done to: " + target.GetComponent<CharacterManager>().characterName);
            }

            var objectTarget = collision.collider.GetComponent<Object>();
            if (objectTarget != null)
            {
                objectTarget.ApplyDamage(damage);
            }

            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}