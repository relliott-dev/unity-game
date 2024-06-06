using UnityEngine;
using System.Collections;

public class RandomOrientationScript : MonoBehaviour {
    // Welcome to the Random Rotation Script!
	// This small script here rotates an Object like the Wreck randomly. Really simple, huh?

	void Start (){
		transform.Rotate(0, 0, Random.Range(0, 360));
	}
}
