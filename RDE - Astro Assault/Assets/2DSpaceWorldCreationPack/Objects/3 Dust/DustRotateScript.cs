using UnityEngine;
using System.Collections;

public class DustRotateScript : MonoBehaviour {
    // Welcome to the Dust Rotate Script. Honestly, I love this one. Esepcially if you set the pushStrength to 10 or so. Had a few good laughs.

    // So, what does it do? it registers if something enters the area of the Dust Cloud (via TriggerEnter2D) and then moves the Dust away from
    // that something. It also spins the clouds a little bit, looking fancier. If you want the Dust to "just be a background element" without
    // any distraction for the player, just use the DustRotateSimpleScript.

    // The three here define, how strong the rotation will be, how long it will rotate (and move) and how far the Dust is pushed at TriggerEnter2D. 
    public float standartRotationStrength;
    public float standartRotationDuration;
    public float standartPushStrength;

    // These are used at runtime, to make the movement slow down gently. Yes, in space there is no friction, ergo once in movement, they should
    // move away forever. However, this looks really bad and empties the scene of Dustclouds pretty fast. So let's ignore the realism here and
    // go for more ambience!
    float rotationStrength = 0;
    float rotationDuration = 0;
    float pushStrength = 0;

    // This one here is just a storage variable to make the Script easier to understand. The Script manipulates this for the rotation. I won't go
    // deep into the rotation here, as a detailed explanation can be found in the ShipMovementScript and in the WorldSpawnerScript, which you should
    // already have read.
    float zAngle = 0;

    // Another storage, in this case for the Vector, in which the Dustcloud has to move after a TriggerEnter2D.
    Vector3 pushedVector;

    // Last Storages, saving the effect of the layer this dust is on.
    float pushStrengthMod;
    float rotationStrengthMod;

    // Use this for initialization
    void Start () {
        // Rotating the Dustcloud randomly at the start, for more felt difference between the Sprites.
        zAngle = Random.Range(0, 360);
        transform.Rotate(Vector3.back * zAngle);
                
        // Here we check on which layer the Dustclouds are on. As it looks really weird if something, that is very far from the player suddenly 
        // starts spinning and moving really fast because the player approached it, this differentiation is quite necessary.
        if(transform.parent.GetComponent<SortingLayerRelayScirpt>().sortingLayerFromWorldSpawner == 0) {
            pushStrengthMod = 0.1f;
            rotationStrengthMod = 0.25f;
            print("here");
        } else if (transform.parent.GetComponent<SortingLayerRelayScirpt>().sortingLayerFromWorldSpawner == 1) {
            pushStrengthMod = 0.05f;
            rotationStrengthMod = 0.1f;
        } else if (transform.parent.GetComponent<SortingLayerRelayScirpt>().sortingLayerFromWorldSpawner == 2) {
            pushStrengthMod = 0;
            rotationStrengthMod = 0;
        }
    }

    void Update() {
        // Basically a normal time check. if the rotation duration is not 0 (changed by TriggerEnter2D), it should move and stop if a certain
        // number of seconds has passed. As Time.deltaTime substracts exactly 1 after 1 second has passed (framrate stabilized), this is perfect.
        rotationDuration -= Time.deltaTime;
        if(rotationDuration >= 0) {
            // Now, 2 important thigns here. First, we factor the rotationStrength in to our desired rotationAngle, then we divide the
            // rotationDuration over the standartRotationDuration. As the rotationDuration gets smaller over time, the rotation slows down.
            // We use the standartRotationDuration twice to spread the smooth the slowing a bit and weaken the speed.
            zAngle = rotationStrength * (rotationDuration / 2*standartRotationDuration) * Time.deltaTime;
            // Well, this is obvious. We rotate the transform backwards and multiply it by the strength fo the zAngle, gradually decreasing
            // the rotation per frame.
            transform.Rotate(Vector3.back * zAngle);
            // Now, what is this? It looks gruesome, but it truely isn't. First, we get the parallaxScript, as it defines the position of the object.
            // If we were to directly manipulate the position here AND in the Parallax Script, both Scripts would fight over where the object is.
            // The consequence is a flickering, ugly Dust Cloud. So we just manipulate a Vector in the Parallax Script and factor it in there.
            // The derivatePos Vector gets added to the original Pos of the object, therefore recalculating a new, right parallaxed Pos.

            //The second half is easier. We take the PushVector (which we get from the TriggerEnter2D, multply it by the strength we defined
            // beforehand and decrease it over time, so that the objects gets slower and slower the further it has travelled.
            GetComponent<ObjectParallaxScript>().derivationPos += pushedVector * pushStrength * (rotationDuration / 2 * standartRotationDuration) * Time.deltaTime;
        }
    }

    //If you want only certain objects to Spin the Dust Clouds, just put an if-check for the tag in here.
    void OnTriggerEnter2D(Collider2D col) {
        // Here,we set the three variables back to their base Values and add in a random component. If everything moves just the same, it
        // can easily be very immersion breaking for the player.
        rotationDuration = standartRotationDuration * Random.Range(0.75f,1.25f);
        rotationStrength = standartRotationStrength * Random.Range(0.75f,1.25f) * rotationStrengthMod;
        pushStrength = standartPushStrength * Random.Range(0.75f, 1f) * pushStrengthMod;
        // Fínally, we take our position, determine where the Player was and substract that. The delta Vector is exactly the Vector where we
        // want the Dust Cloud to move. Simple geometry at work here, if you want deeper understanding, I suggest looking only as this has been
        // dealt with very often. Just google "move away from object unity" or something similar.
        // Usually, you would normalize the Vector, but here, it is not really necessary, as no matter how you look at it, the results are jsut fine.
        pushedVector = transform.position - col.transform.position;
        print(rotationStrengthMod);
    }
}
