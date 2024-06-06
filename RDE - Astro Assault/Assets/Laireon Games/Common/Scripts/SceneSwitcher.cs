using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Place this on an empty object and it will switch between scenes
/// </summary>
public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher Instance;

    public int sceneCount;
    int currentScene;
    float keyDelay;

    public new Transform camera;
    public float maxZoom = 1;

    Vector3 originalPosition;
    Vector3 editedPosition;

    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);//preserve this between scenes
            originalPosition = camera.position;

            if(camera != null)
                DontDestroyOnLoad(camera.gameObject);
        }
        else
        {
            Destroy(gameObject);

            if(camera != null)
                Destroy(camera.gameObject);
        }
    }

    public void OnGUI()
    {
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();

        GUILayout.Space(10);

        if(GUILayout.Button("Previous") || (Input.GetKeyDown(KeyCode.LeftArrow) && keyDelay <= 0))
        {
            currentScene--;

            if(currentScene < 0)
                currentScene = sceneCount - 1;

            SceneManager.LoadScene(currentScene);

            keyDelay = 0.25f;//don't register many key presses
        }

        if(GUILayout.Button("Next") || (Input.GetKeyDown(KeyCode.RightArrow) && keyDelay <= 0))
        {
            currentScene = (currentScene + 1) % sceneCount;

            SceneManager.LoadScene(currentScene);

            keyDelay = 0.25f;//don't register many key presses
        }

        GUILayout.EndHorizontal();

        if(camera != null)
        {
            GUILayout.Space(10);

            GUILayout.Label("Camera Zoom");
            editedPosition.z = GUILayout.HorizontalSlider(editedPosition.z, 0, maxZoom);
        }
    }

    void Update()
    {
        if(keyDelay > 0)
            keyDelay -= Time.deltaTime;

        if(camera != null)
            camera.position = originalPosition + editedPosition;
    }
}
