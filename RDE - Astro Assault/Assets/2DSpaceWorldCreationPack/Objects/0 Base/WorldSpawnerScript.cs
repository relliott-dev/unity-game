using UnityEngine;
using System.Collections;

public class WorldSpawnerScript : MonoBehaviour {
	//Welcome to one of the heartpieces. As soon as you have everything set up, you just throw this Script into an empty object, feed it all the necessary Prefabs and it creates a beautiful, Moving, yet uninhabited SpaceWorld for you.
	//I guess I have to explain some things here however, so please listen closely to the comments here.

	//Here, you throw in the planet prefab. It will automatically instantiate one at a random location near the map center. If you want more planets, just copy the planet spawn lines below.
    public Transform PlanetPrefab;

    //In here goes the Background Fog Prefab
    public Transform FogSpawner;

    //These will be used in the Script to determine how many objects will spawn where.
    int numberOfForegroundObjects;
	int numberOfMiddlegroundObjects;
	int numberOfBackgroundObjects;

	//And here you'll throw in all the juicy prefabs to be spawned. If you want, say, more asteroids, just make the Array longer and insert more asteroid prefabs, than others.
    public Transform[] ObjectsToBeSpawned;


    // Use this for initialization
    void Start() {
		//This part here is for spawning a planet. In the realm of this demo, where the borders are at 46WorldUnits, teh range of 25/25 gives quite reasonable results. The planets wont be too far out of the players reach,
		// but they wont be the center piece at the beginning. Feel free to change the position as you deem fit for your awesome project!
        Vector3 planetPos = new Vector3(Random.Range(-25, 25), Random.Range(-25, 25), 0);
        Instantiate(PlanetPrefab, planetPos, Quaternion.identity);

        //After the planet, we spawn the Background Fog, which add aesthetic noise to the scene.
        Instantiate(FogSpawner);

		//These here are to determine the number of objects that spawn on each layer. If you want a denser space, up the numbers. If you want bleak emptiness, reduce them.
		//Remember: The Objects that will be spawned are inserted into the inspector at Objects to be spawned..
        numberOfForegroundObjects = Random.Range(3, 4);
        numberOfMiddlegroundObjects = Random.Range(0, 6);
        numberOfBackgroundObjects = Random.Range(2, 8);
		//We'll need this for the upcoming spawning algorithm.
		int numberOfObjectsTotal = numberOfForegroundObjects + numberOfMiddlegroundObjects + numberOfBackgroundObjects;

		//Here, we tell unity to Keep spawning objects until it has spawned as many objects as the number of objects that should be spawned.
		for (int i = 0; i < numberOfObjectsTotal; i++) {
			//Once again, the numbers here are fit for my demo. As the worldborders are bewtween (-46,-46) and (46, 46), I'd like them all to spawn somewhere in there.
			//While the positioning may look simple, it gives really great, organic results. Just try it out.
			Vector3 fieldPos = new Vector3(Random.Range(-46f, 46f), Random.Range(-46f, 46f), 0);

			//This little Random Range picks a random object from the ObjectsToBeSpawned
			int whichFieldToSpawn = Random.Range(0, ObjectsToBeSpawned.Length);

			//The if & else here make sure, that it spawns the right amount of objects on each layer. While the code may seem a bit redundant, Unity tends to mess things up
			//if you don't edit the Instances settings right after the instantiating code. Therefore, we have copied code here.
			if(i<numberOfForegroundObjects) {
				//This spawns the chosen Object at the chosen FieldPos. As Quaternions are a bit hard to work with, we instantiate it with the normal Quaternion identy
				//the respective Scripts in the prefabs handle the rotation, which prevents this script from cluttering unnecessary and allow for easier manipulation and understanding.
				//Also, you may want different starting rotations, which are not entirely random in your objects. Doing that differenciation here would make the code easier breakable.
				Transform ObjectInstance = Instantiate(ObjectsToBeSpawned[whichFieldToSpawn], fieldPos, Quaternion.identity) as Transform;
				//This send the respective sorting layer to the Instantiated prefab. The Sorting Layer Controls the parallax effect, the color and the size of the object.
	            ObjectInstance.GetComponent<SortingLayerRelayScirpt> ().sortingLayerFromWorldSpawner = 0;
				//If every Object has it's own name, finetuning them is way easier.
				ObjectInstance.name = ObjectsToBeSpawned [whichFieldToSpawn].name;
				//The following line prevents the Hierarchy from cluttring. Very valuable.
				ObjectInstance.transform.parent = transform;
			} else if (i<numberOfForegroundObjects+numberOfMiddlegroundObjects){
				Transform ObjectInstance = Instantiate(ObjectsToBeSpawned[whichFieldToSpawn], fieldPos, Quaternion.identity) as Transform;
				ObjectInstance.GetComponent<SortingLayerRelayScirpt> ().sortingLayerFromWorldSpawner = 1;
				ObjectInstance.name = ObjectsToBeSpawned [whichFieldToSpawn].name;
				ObjectInstance.transform.parent = transform;
			} else {
				Transform ObjectInstance = Instantiate(ObjectsToBeSpawned[whichFieldToSpawn], fieldPos, Quaternion.identity) as Transform;
				ObjectInstance.GetComponent<SortingLayerRelayScirpt> ().sortingLayerFromWorldSpawner = 2;
				ObjectInstance.name = ObjectsToBeSpawned [whichFieldToSpawn].name;
				ObjectInstance.transform.parent = transform;
			}
		}
    }
}
//The End of our beautiful world crafting exercise.
//Hint: The starfield in the back is not in here, as it is attached to the camera and always the same. Adding additional starfield with
//other parallax or reducing them gives quite weird results. If you however want to simulate a fast travel through space, go ahead and
//just add more to the camera and modify their parallax.

