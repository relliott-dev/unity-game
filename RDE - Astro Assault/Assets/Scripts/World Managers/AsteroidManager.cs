using UnityEngine;

namespace RDE
{
    public class AsteroidManager : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float minSpeed = -1f;
        [SerializeField] private float maxSpeed = 1f;
        [SerializeField] private float minRotationSpeed = -120f;
        [SerializeField] private float maxRotationSpeed = 120f;

        [Header("Helper Variables")]
        private Rigidbody2D rb;
        private float moveSpeed;
        private float rotationSpeed;
        private Vector2 moveDirection;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            moveSpeed = Random.Range(minSpeed, maxSpeed);
            rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);

            moveDirection = Random.insideUnitCircle.normalized;
            rb.velocity = moveDirection * moveSpeed;
            rb.angularVelocity = rotationSpeed;
        }
    }
}