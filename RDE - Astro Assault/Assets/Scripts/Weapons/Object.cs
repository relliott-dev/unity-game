using UnityEngine;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages health, damage, and destruction effects for objects such as asteroids
    /// Objects can inflict damage on players based on their speed at the time of collision
    /// 
    /// @TODO:
    /// - Implement damage interactions between asteroids when they collide with each other?
    /// - Implement damage interactions between player/enemy when they collide with object?
    /// - Add sprite changes to visually represent damage as the object's health decreases
    /// 
    /// </summary>
    public class Object : MonoBehaviour
    {
        [Header("Variables")]
        [SerializeField] private float damageMultiplier = 8f;
        [SerializeField] private float maxHealth = 50f;
        private float currentHealth;

        [Header("Effects")]
        [SerializeField] private GameObject destructionEffect;
        [SerializeField] private Sprite[] damageSprites;
        private SpriteRenderer spriteRenderer;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            currentHealth = maxHealth;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            CharacterManager characterManager = collision.collider.GetComponentInParent<CharacterManager>();
            if (characterManager != null && !characterManager.isDead && characterManager.tag == "Player")
            {
                var locomotionManager = characterManager.GetComponent<CharacterLocomotionManager>();
                float currentSpeed = locomotionManager.CurrentSpeed();

                if (currentSpeed > 1f)
                {
                    float damage = currentSpeed * damageMultiplier;
                    characterManager.characterStatManager.InstantHealth(-damage);
                    locomotionManager.ApplyReflection(collision);
                    Debug.Log(damage + " damage done to: " + characterManager.GetComponent<CharacterManager>().characterName + " by " + name);
                }
            }
        }

        public void ApplyDamage(float damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                DestroyObject();
            }
            else
            {
                UpdateDamageSprite();
            }
        }

        private void UpdateDamageSprite()
        {
            if (spriteRenderer != null && damageSprites.Length > 0)
            {
                float healthPercentage = currentHealth / maxHealth;
                int spriteIndex = Mathf.FloorToInt((1 - healthPercentage) * (damageSprites.Length - 1));
                spriteRenderer.sprite = damageSprites[Mathf.Clamp(spriteIndex, 0, damageSprites.Length - 1)];
            }
        }

        private void DestroyObject()
        {
            if (destructionEffect != null)
            {
                Instantiate(destructionEffect, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}