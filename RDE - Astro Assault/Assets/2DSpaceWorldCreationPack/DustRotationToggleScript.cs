using UnityEngine;
using System.Collections;

public class DustRotationToggleScript : MonoBehaviour {
    // Welcome to the DustRotationToggle Script! All it does enables/disables the moving and rotation of the dust clouds.
    // I coded the rotation in, saw that it was kind of immersion breaking, but quite funny, so here you go.

    bool rotationEnabled = false;

	public void ButtonPress () {
        if (rotationEnabled == false) {
            foreach (DustRotateScript DRScript in FindObjectsOfType<DustRotateScript>()) {
                DRScript.standartRotationStrength = 40;
                DRScript.standartRotationDuration = 3;
                DRScript.standartPushStrength = 1;
            }
            rotationEnabled = true;
        } else {
            foreach (DustRotateScript DRScript in FindObjectsOfType<DustRotateScript>()) {
                DRScript.standartRotationStrength = 0;
                DRScript.standartRotationDuration = 0;
                DRScript.standartPushStrength = 0;
            }
            rotationEnabled = false;
        }
    }
}
