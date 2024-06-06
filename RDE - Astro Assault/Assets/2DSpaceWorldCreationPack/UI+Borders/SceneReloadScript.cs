using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneReloadScript : MonoBehaviour {
    // Welcome to the Scene Reload Script
    // Super simple, it just reloads the entire scene as soon as you press the button.
    //pay attention to the "using SystemEngine.SceneManagment", as this is necessary for scene manipulation.
	
	public void ReloadScene () {
        SceneManager.LoadScene(0);
	}
}
