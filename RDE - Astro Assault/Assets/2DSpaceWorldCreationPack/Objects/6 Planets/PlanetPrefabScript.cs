using UnityEngine;
using System.Collections;

public class PlanetPrefabScript : MonoBehaviour {
    // So, the basic script that every planet should have. I just have one planet prefab, and depending on the SystemType (which can be defined
    // elsewhere) the prefab is generated with one of the suitable planet sprites. 

	// Throw in all the Sprites that you have for planets.
	public Sprite[] Planets;
    // Here you define the type of planet that is spawned. If it is a terran planet, give it type 1, ice planets are type 2, or however
    // you see fit. However, you got to remove the random component further down in the script, it's for show purpose only.
	public int sysType;

	void Start () {
    // Rotating it randomly and determining the size randomly greatly increase the difference between the planets,
    // giving the entire game a better atmosphere.
		transform.Rotate(0, 0, Random.Range(0, 360));
		float planetscale = Random.Range(0.75f, 1.5f);
		transform.localScale = new Vector3(planetscale, planetscale, planetscale);

        // This is just for the demo, if you want the game to throw the right planets for each sysType, just adjust them accordingly!
        int whichPlanetToShow = Random.Range(0, Planets.Length);
        GetComponent<SpriteRenderer>().sprite = Planets[whichPlanetToShow];

    }
}


/*
    This code snippet is for different planet Types that you could define, as mentioned above. Just tell it,
    where the Sprites for the according SysType are.


    if (sysType == 1) {
			GetComponent<SpriteRenderer>().sprite = Planets[Random.Range(0, 8)];
		}
		else if (sysType == 2) {
			GetComponent<SpriteRenderer>().sprite = Planets[Random.Range(9, 18)];
		} else if (sysType == 3) {
			GetComponent<SpriteRenderer>().sprite = Planets[Random.Range(19, 24)];
		} else if (sysType == 4) {
			GetComponent<SpriteRenderer>().sprite = Planets[Random.Range(25, 32)];
		} else if (sysType == 5) {
        GetComponent<SpriteRenderer>().sprite = Planets[Random.Range(33, 41)];
        }*/
