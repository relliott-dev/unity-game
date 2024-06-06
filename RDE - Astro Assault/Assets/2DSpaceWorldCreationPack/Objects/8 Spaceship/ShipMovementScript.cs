using UnityEngine;
using System.Collections;

public class ShipMovementScript : MonoBehaviour {
    // Welcome to the Ship Movement Script! While you are here, let me quickly explain the structure of the PreFab. It wil benefit you greatly.
    
    // So, we have the main Ship object and two Children, one with the Sprite, the other with this Script and the Collider.
    // The advantage is, if you split those two up, you can manipulate the rotation of the Sprite, without changing the rotation of everything.
    // Additionally, if you have everything in one object, using stuff like shields can be a hassle:
    // Unity does not differentiate the colliders of children and parent. So, if you need different colliders (say a Vector shield which only
    // covers one side and has its own Scripts, will fire all the OnCollisionEnter Functions in the Main Object as well. Therefore, by defining
    // one parent object and making everything in it as children, you can easier expand your mechanics, without quarelling with Unity.

    // So, back to the Script itself.

    // Flying and Rotation Speed. Fairly obvious.
    public float speed;
    public float rotationSpeed;

    // Defining the Default rotation along the z-axis, changing this will later on change the rotation of the ship.
    float zAngle = 0;

    void Start() {
        // Once more, setting Speeds to default if no Input has been registered. If you want turrets, you should change this, lest you want to be
        // bombarded by warnings at Runtime.
        if (speed == 0) {
            speed = 10;
        }
        if (rotationSpeed== 0) {
            Debug.LogWarning("No rotationSpeed set for" + transform.parent);
        }
    }

    void Update () {
        // Checking if the player wants to move forward by registering Inputs. If you need collisions, I suggest using the velocity instead of this
        // as directly changing the transform "teleports" the object, therefore kind of ignoring the boundaries of colliders.
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            // By multipliying with "Time.deltaTime" we stabilize the speed to the framerate. What this does is that the ship will always move
            // at the same Speed, no matter the Framerate of the PC. If you didn't know that, I suggest reading up on it!
            // Also, we use "transform.up" to make sure the object is moving in the direction it is facing.
            transform.parent.transform.position += transform.up * speed * Time.deltaTime;

        // Doing the same with moving backward. "else if" because it is weird if the player moves forwards AND backwards.
        } else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            transform.parent.transform.position += transform.up * -speed * Time.deltaTime;
        }

        // Now on to the rotation. You remember our zAngle at 0? Here, we gradually increment or decrement it, depending on where we want to rotate.
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            // Incrementing the zAngle by the rotationSpeed, again framerate-stabilized.
            zAngle += rotationSpeed*Time.deltaTime;
            // Now now, doesn't this look scary? Don't worry, it's rather easy!
            // First, we get the rotation of the parent (remember? This Script is only in the Child. If we want to rotate everything (ergo, this, 
            // the Sprite, possibly shields or whatever comes to mind), we need to access the parent.
            // Quaternion.Euler transforms our zAngle into an Quaternion, the way Unity works considering rotations. In layman's terms: You saw the
            // rotation values of the Transform in the inspector? Quaternion.Euler writes the numbers into these and Unity recalculates this into
            // a rotation. 
            transform.parent.transform.rotation = Quaternion.Euler(0f, 0f, zAngle);
        }
        //And this is doing the same, just in the opposite direction
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            zAngle -= rotationSpeed * Time.deltaTime;
            transform.parent.transform.rotation = Quaternion.Euler(0f, 0f, zAngle);
        }
	}
}
