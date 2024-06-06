using UnityEngine;
using System.Collections;

public class CameraPlayerFollowScript : MonoBehaviour {
    public Transform myTarget;


    void Start() {
        //Find the player
		myTarget = GameObject.FindGameObjectWithTag("PlayerShipTag").transform;
    }
	void Update () {
        //If there is a player, transform the cameras position right above the player
        if (myTarget != null) {
            Vector3 targetPosition = myTarget.position;
            targetPosition.z = transform.position.z;
            transform.position = targetPosition;
        }	
	}
}




/* Code for letting the camera follow the player slowly. Issue: Ship shakes.
float g = Time.deltaTime * (Time.timeScale / transitionDuration);
    public float transitionDuration;float t = 0.5f;
    //print(targetPosition +"aaaaand " + transform.position);
    Vector3 lerpedCam = Vector3.Lerp(targetPosition, transform.position, t);*/
