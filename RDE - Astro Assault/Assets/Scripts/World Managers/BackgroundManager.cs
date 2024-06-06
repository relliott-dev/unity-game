using System.Collections;
using UnityEngine;

namespace RDE
{
    [System.Serializable]
    public class BackgroundLayer
    {
        public GameObject layerObject;
        [Range(0.1f, 100)] public float parallaxScale;
    }

    public class BackgroundManager : MonoBehaviour
    {
        #region Variables

        [Header("Settings")]
        [SerializeField] private BackgroundLayer[] backgroundLayers;

        [Header("Helper Variables")]
        private Transform cameraTransform;
        private Material[] materials;

        #endregion

        #region Basic Functions

        private void Awake()
        {
            cameraTransform = Camera.main.transform;
        }

        private void Start()
        {
            materials = new Material[backgroundLayers.Length];
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = backgroundLayers[i].layerObject.GetComponent<MeshRenderer>().sharedMaterial;
            }
        }

        private void Update()
        {
            Vector3 newPos = cameraTransform.position;
            newPos.z = transform.position.z;
            transform.position = newPos;

            for (int i = 0; i < materials.Length; i++)
            {
                Vector2 materialOffset = materials[i].mainTextureOffset;
                materialOffset.x = backgroundLayers[i].layerObject.transform.position.x / backgroundLayers[i].layerObject.transform.localScale.x / backgroundLayers[i].parallaxScale;
                materialOffset.y = backgroundLayers[i].layerObject.transform.position.y / backgroundLayers[i].layerObject.transform.localScale.y / backgroundLayers[i].parallaxScale;
                materials[i].mainTextureOffset = materialOffset;
            }
        }

        #endregion
    }
}