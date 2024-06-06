using UnityEngine;
using System.Collections;

public class DustRotateSimpleScript : MonoBehaviour {
    // Welcome to the Dust Rotate Simple Script.
    //It simple rotates the attached object by a random amount. Pretty self-explanatory.
	
	void Start () {
        transform.Rotate(Vector3.back * Random.Range(0, 360));
    }
}
