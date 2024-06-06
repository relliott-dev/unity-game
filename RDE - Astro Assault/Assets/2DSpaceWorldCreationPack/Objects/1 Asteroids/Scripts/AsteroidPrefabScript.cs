using UnityEngine;
using System.Collections;

public class AsteroidPrefabScript : MonoBehaviour {
    // Welcome to the Asteroid Prefab Script. Space without Asteroids... Can you imagine that? Yes, it would be realistic. Yes, it would look boring.
    // As I had so many Asteroids, this Script differs bit from the nebula or the Dust Scripts. Please bear with me, it's very simple.

	// Here you throw the normal all the Asteroid Sprites in. If you have the 2D Space Kit, I'd suggest throwing theri asteroids and the debris in here to, it's compatible and looks better
    public Sprite[] Asteroids;

	// The speed of the automatic asteroid rotation. If everything is rotating, the entire world feels more alive and real, my standart value is 0.5
    public float rotationSpeed;
    
	// This sorts the Prefab on one of the Sorting Layers, respectively Front, Middle or Back;
	public int sortingLayerNumber;

	// This is needed for the asteroid inherent rotation
	public float asteroidScale = 0;

    void Start () {
        // First, the script takes a random asteroid appearance
        GetComponent<SpriteRenderer>().sprite = Asteroids[Random.Range(0, Asteroids.Length)]; 
    }
	
	void Update () {
		// Don't worry about the magic calculation. I just found this calculation the most aesthetically pleasing. Also, making the roation speed
		// depending on the size of the asteroid is actually pretty stunning, additionally, it give a random component and a more organic feeling.
		if (asteroidScale != 0) {
			transform.Rotate (Vector3.forward * Time.deltaTime * 8 / (asteroidScale * asteroidScale) * rotationSpeed);
		}
	}
}
