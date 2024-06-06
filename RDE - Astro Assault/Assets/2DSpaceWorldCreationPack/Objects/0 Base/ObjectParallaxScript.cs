using UnityEngine;
using System.Collections;

public class ObjectParallaxScript : MonoBehaviour {
    // So, the object parallax script. Welcome to it! Don't be afraid because it looks complex and lengthy,
    // actually, it is quite simple to understand and I'm sure you'll grow to like it! So, what does it do?
    // It looks for a the player and, depending on how far "away" the object is (by using the layers), the Script moves it relatively
    // faster or slower. Feel free to meddle with the values for a different feeling of "speed". Oh, and yes, the layers are hardcoded,  
    // as I deemed 4 Layers of objects, together with the Starfield Background as enough to create a "feeling" of flying through space.
    // If you want to make it more dimensional, just add layers and give them the according parallax.

    // Strength of parallax, defined by layer it is on.
    float parallax;
	// Saves the original Position as a reference.
    Vector3 origPos;

    // This is only used by the Dustclouds, to push them around.
    public Vector3 derivationPos = new Vector3 (0,0,0);

    // The Gameobject relative to which which the Parallax Effect occurs. Can also be the Camera. In my Project, the Camera is attached
    // to the player, so it wouldn't make a difference. If you need the Parallax Effect however to occur in relation to the camera,
    // feel free to modify it or ask me how to do that.
    GameObject player;

    // Those down here define the size of the objects. Just tell the inspector of the prefab, how big or small an object may be,
    // depending on its layer. If you don't enter anything, It'll use the default settings below.
	float objectScale;
	public float scaleMinFore;
	public float scaleMaxFore;
	public float scaleMinMiddle;
	public float scaleMaxMiddle;
	public float scaleMinBack;
	public float scaleMaxBack;

    void Start() {
        // This little void makes sure that the sorting layer gets fetched from the SortingLayerRelayScript of the parent.
        // It also darkens the color, the "further away" an object is, to make it really seem more distant.
        CheckForSortingLayer();
        // Unity saves the original position of the object
        origPos = transform.position;
        // Here, the strength of the parallax effect is defined, depending on which layer the object is at. This could have been done in the
        // CheckForSortingLayers() void, but keeping it here makes it clearer to understand.
        // Parallax values: 0 = staying in place, 1 = keeping its distance relative to the obejct of refernce (e.g. the player)
        if (GetComponent<SpriteRenderer>().sortingLayerName == "FrontBack") {

            // The parallax is a bit randomized, so that even objects on the same layer are a bit different. Enhances depth.
            // Some of my player tester however found this a bit weird, so judge by your own feeling. The standart values are
            // 0.5f, 0.15f and 0.3f.
            parallax = Random.Range(0.035f,0.075f);

			// These parts are for the default asteroid size. If you enter nothing in the inspector, it will automatically assign the defaults.
			if(scaleMinFore == 0){
				scaleMinFore = 0.33f;
			}
			if (scaleMaxFore == 0){
				scaleMaxFore = 1;
			}
			// Here we randomly determine the scale of the object depending on the Min/Max values. It works the same for the other 2 sorting Layers.
			objectScale = Random.Range(scaleMinFore, scaleMaxFore);
        }
        if(GetComponent<SpriteRenderer>().sortingLayerName == "Middle") {
            parallax = Random.Range(0.17f, 0.21f);
			if(scaleMinMiddle == 0){
				scaleMinMiddle = 0.25f;
			}
			if (scaleMaxMiddle == 0){
				scaleMaxMiddle = 0.75f;
			}
			objectScale = Random.Range(scaleMinMiddle, scaleMaxMiddle);
        }
        if( GetComponent<SpriteRenderer>().sortingLayerName == "Back"){
            parallax = Random.Range(0.26f, 0.34f);
			if(scaleMinBack == 0){
				scaleMinBack = 0.15f;
			}
			if (scaleMaxBack == 0){
				scaleMaxBack = 0.5f;
			}
			objectScale = Random.Range(scaleMinBack, scaleMaxBack);
        }
		// Transformation of the object to its actual, layerbased scale.
		transform.localScale = new Vector3(objectScale, objectScale, objectScale);

        // The only objects on the last layer should be planets, as they need to have a different parallax as other, smaller objects.


		//If it's an asteroid, it needs to know the scale for rotation. That is done here.
		if (GetComponent<AsteroidPrefabScript> ()) {
			GetComponent<AsteroidPrefabScript> ().asteroidScale = objectScale;
		}
    }

    void Update() {
        // I chose to let the objects position be relative to the player. If you want, you can put this FindPlayer into start, just make sure
        // you tell the game that the parallax script gets executed after the player has been set into the scene.
		// If you stick with this version, be aware that once there is no object with the desired tag (or multiples), problems may arise.
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("PlayerShipTag");
        }

        // Transforming the position relative to the player, multiplied by the parallax, so that only a fracture (or multiple) of the playerposition
        // is added to the baseposition of the object. This gives the desired parallax Effect, greatly enhancing the feeling of depth in a 2D world.
        Vector3 parallaxedPos = new Vector3(origPos.x + derivationPos.x + player.transform.position.x * parallax, origPos.y + derivationPos.y + player.transform.position.y * parallax, 0);
        transform.position = parallaxedPos;
    }

    void CheckForSortingLayer() {
        //This check prevents the fog to be colored darker, as this makes the entire scene a bit dull looking.
        // Also, the Fog Prefabs all have their sortingLayer already assorted.
        if (GetComponentInParent<SortingLayerRelayScirpt>()) { 
            // Fetching the sorting Layer from the Parent.
            int SortingLayerOfThis = GetComponentInParent<SortingLayerRelayScirpt>().sortingLayerFromWorldSpawner;
            // Checking which sorting layer, sending that to the Sprite Renderer
            if (SortingLayerOfThis == 0) {
                GetComponent<SpriteRenderer>().sortingLayerName = "FrontBack";
            } else if (SortingLayerOfThis == 1) {
                GetComponent<SpriteRenderer>().sortingLayerName = "Middle";
                //Coloring objects further away darker.
                GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 1);
            } else if (SortingLayerOfThis == 2) {
                GetComponent<SpriteRenderer>().sortingLayerName = "Back";
                GetComponent<SpriteRenderer>().color = new Color(0.47f, 0.47f, 0.47f, 1);
            }
        }
    }
}