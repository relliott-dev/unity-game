using UnityEngine;
using System.Collections;

public class StarFieldScrollingScript : MonoBehaviour {
    // This script will move the background starfield(s), which are children of the camera, alongside the camera. It does so by checking its
    // transform position (which will change as the camera moves) and then calculating. Just how far the texture needs to be offset to create
    //  a believable parallax effect.


    // Gives the srength of the parallax effect occuring. The lower the value, the faster it moves along. Critical thresshold is 1:
    // at a value of 10, the object will be nearly static towards the camera, at 0.5 it will just flow by quite fast.
    public float parallax;


	Material mat;
	Vector2 offsetBg;
    void Start() {
		// Just making sure that its consistent with the layers.
        GetComponent<Renderer>().sortingLayerName = "FurthestBack";

		// Getting the material component in order to scroll over the material
		mat = GetComponent<MeshRenderer>().material;
		// With the offset, we will tell unity how far exactly we have scrolled.
		offsetBg = mat.mainTextureOffset;
    }

	void Update () {
		// Here we calculate where the camera is right now and then adjusts the shown part of the Starfield material.
        offsetBg.x = transform.position.x / transform.localScale.x / parallax;
        offsetBg.y = transform.position.y / transform.localScale.y / parallax;
        // As we use a material with a repeatable texture for the starfield, we can scroll infinitely often.
        mat.mainTextureOffset = offsetBg;
    }
}
