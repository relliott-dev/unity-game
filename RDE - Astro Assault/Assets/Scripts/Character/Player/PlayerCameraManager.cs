using UnityEngine;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages all aspects of the camera's behavior
    /// This class facilitates smooth camera movement and rotation and allows zooming in and out
    /// The camera has a toggle to rotate with the player or maintain it's rotation
    /// 
    /// @TODO:
    /// - Add offset in player direction to see more ahead
    /// - Add camera effect when we are approaching edge of boundaries
    /// 
    /// </summary>
    public class PlayerCameraManager : MonoBehaviour
    {
        #region Variables

        public static PlayerCameraManager instance;

        [HideInInspector] public PlayerManager playerManager;

        [Header("Camera Settings")]
        [SerializeField] private float cameraSmoothSpeed = 1f;
        [SerializeField] private bool rotateCameraWithPlayer = false;

        [Header("Zoom Settings")]
        [SerializeField] private float zoomSpeed = 10f;
        [SerializeField] private float zoomDamping = 0.1f;
        [SerializeField] private float currentOrthographicSize = 5f;
        [SerializeField] private float minOrthographicSize = 3f;
        [SerializeField] private float maxOrthographicSize = 7f;

        [Header("Helper Variables")]
        private Camera mainCamera;
        private Vector3 cameraVelocity;

        #endregion

        #region Base Functions

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            mainCamera = GetComponentInChildren<Camera>();
        }

        private void Update()
        {
            if (playerManager != null)
            {
                HandleFollowTarget();
                HandleRotateCamera();
            }
        }

        #endregion

        #region Camera Functions

        //Handles camera movement
        private void HandleFollowTarget()
        {
            Vector3 targetPosition = playerManager.playerLocomotionManager.modelTransform.position;
            targetPosition.z = transform.position.z;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref cameraVelocity, cameraSmoothSpeed);
        }

        //Handles camera rotation
        private void HandleRotateCamera()
        {
            if (rotateCameraWithPlayer)
            {
                transform.rotation = Quaternion.Euler(0, 0, playerManager.playerLocomotionManager.modelTransform.eulerAngles.z);
            }
        }

        //Handles zooming in/out on the player
        public void HandleZoomInput(float zoomAmount)
        {
            if (playerManager == null)
            {
                return;
            }

            float scaledZoomAmount = zoomAmount * zoomSpeed * Time.deltaTime;
            currentOrthographicSize -= scaledZoomAmount;
            currentOrthographicSize = Mathf.Clamp(currentOrthographicSize, minOrthographicSize, maxOrthographicSize);
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, currentOrthographicSize, zoomDamping);
        }

        #endregion
    }
}