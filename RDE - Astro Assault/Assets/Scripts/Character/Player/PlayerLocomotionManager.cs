using UnityEngine;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages various aspects of player locomotion including walking, running, sprinting, jumping, dodging, and free-fall movements
    /// Integrates with the PlayerManager for state management and PlayerCameraManager for direction control
    /// It handles player movement based on input, stamina, and other gameplay conditions
    /// 
    /// @TODO:
    /// - Dodge mechanic?
    /// - Add SFX
    /// - Add particles
    /// - Possible rewrite?
    /// 
    /// </summary>
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        #region Variables

        private PlayerManager playerManager;

        [Header("Movement Settings")]
        [SerializeField] private float accelerationSpeed = 3f;
        [SerializeField] private float brakeSpeed = 5f;
        [SerializeField] private float maxSpeed = 5f;
        [SerializeField] private float rotationSpeed = 180f;
        [SerializeField] private float inertiaDamping = 1f;

        [Header("Boost Settings")]
        [SerializeField] private float boostMultiplier = 2f;
        [SerializeField] private float boostCost = 15f;
        [SerializeField] private float heatCost = 20f;

        [Header("Helper Variables")]
        [HideInInspector] public Transform modelTransform;
        private float verticalMovement;
        private float horizontalMovement;

        #endregion

        #region Base Functions

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            modelTransform = transform.Find("Model");
        }

        private void Update()
        {
            verticalMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;

            HandleMovement();
            HandleRotation();
            HandleStrafing();
            HandleBoosting();
        }

        #endregion

        #region Private Functions

        //Handles player movement
        private void HandleMovement()
        {
            if (playerManager.isDead || !playerManager.canMove)
            {
                return;
            }

            if (verticalMovement > 0)
            {
                velocity += (Vector2)modelTransform.up * accelerationSpeed * Time.deltaTime;
            }
            else if (verticalMovement < 0 && velocity.magnitude > 0)
            {
                velocity -= velocity.normalized * brakeSpeed * Time.deltaTime;
            }
            else
            {
                velocity *= 1 - Time.deltaTime * inertiaDamping;
            }

            velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
            modelTransform.position += (Vector3)velocity * Time.deltaTime;
        }

        //Handles player rotation
        private void HandleRotation()
        {
            if (playerManager.isDead || !playerManager.canMove || !playerManager.canRotate)
            {
                return;
            }

            if (horizontalMovement != 0)
            {
                modelTransform.Rotate(Vector3.forward, -horizontalMovement * rotationSpeed * Time.deltaTime);
            }
        }

        //Handles player strafing
        private void HandleStrafing()
        {
            if (playerManager.isDead || !playerManager.canMove || playerManager.canRotate)
            {
                return;
            }

            Vector2 strafeDirection = new Vector2(modelTransform.up.y, -modelTransform.up.x) * horizontalMovement;
            velocity += accelerationSpeed * Time.deltaTime * strafeDirection;
        }

        //Handles player boost
        private void HandleBoosting()
        {
            if (playerManager.isDead || !playerManager.canMove || playerManager.currentEnergy <= 0f || !playerManager.isBoosting)
            {
                playerManager.isBoosting = false;
                return;
            }

            velocity *= boostMultiplier;
            velocity = Vector2.ClampMagnitude(velocity, maxSpeed * boostMultiplier);

            playerManager.characterStatManager.InstantEnergy(-boostCost * Time.deltaTime);
            playerManager.characterStatManager.InstantHeat(heatCost * Time.deltaTime);
        }

        #endregion
    }
}