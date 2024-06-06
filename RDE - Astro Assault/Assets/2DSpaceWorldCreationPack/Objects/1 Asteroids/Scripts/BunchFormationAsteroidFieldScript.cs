using UnityEngine;
using System.Collections;

public class BunchFormationAsteroidFieldScript : MonoBehaviour {
    // Welcome to the Bunch Script! While just randomly instantiating asteroids might sound funny, the results there are too random.
    // Therefore, even this "Bunch" has some structure in it.

    // Throw the default Asteroid Prefab in here.
    public GameObject AsteroidPrefab;

    // This here decides how far the asteroids should be darkened and their parallax. Drawn in from the SLR.
    public int sortingLayerOfAsteroids;
	
	void Start () {
        // First of, we want our Asteroid field to have a random direction each time this fires.Keeps things chaotic and organic.
        transform.Rotate(0, 0, Random.Range(0, 360));

        // This is needed for the upcoming spawning of the asteroids, just wait.
        int numberOfAsteroidsSpawned = 0;

        // Feel free to fiddle with the number. The higher, the more asteroids will form the outer ring increasing its density.
        // In tht case however, I would suggest increase the treshold for center asteroids, too.
        int numberOfAsteroidstoSpawn = Random.Range(15, 25);

        // Getting the sorting layer now, so that we don't have to worry about it in each repetition of the "For".
        sortingLayerOfAsteroids = GetComponent<SortingLayerRelayScirpt> ().sortingLayerFromWorldSpawner;

        // Counting up the Asteroids until we have reached our desired number.
        for (numberOfAsteroidsSpawned =0; numberOfAsteroidsSpawned <= numberOfAsteroidstoSpawn; numberOfAsteroidsSpawned++){
            // These 8 Asteroids form the center, making it roughly the same density every time. This balances some of bad chaotic effects that
            // the Random.Range can sometimes have.
            if (numberOfAsteroidsSpawned <= 8) {
                // Once more, in the demo, the base size is sufficient between 0 and 5 World Units. Fiddle with it if you want something different.
                Vector3 spawnVector = transform.position + new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
                // Instantiating the Asteroid at the spawnVector above, uncluttering the hierarchy by parent setting and handing over the sorting layer.
                GameObject Asteroid = Instantiate(AsteroidPrefab, spawnVector, Quaternion.identity) as GameObject;
                Asteroid.transform.parent = transform;
				Asteroid.GetComponent<AsteroidPrefabScript> ().sortingLayerNumber = sortingLayerOfAsteroids;
            } else {
                // Well, basically the same as above, however, the possible area, where the outer asteroids spawn is increased.
                Vector3 spawnVector = transform.position + new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0);
                // We also make sure that no Asteroid Spawns to close to the center, otherwise it would clutter too much.
                if (Mathf.Abs(spawnVector.x) <= 4) {
                    spawnVector.x *= 3;
                }
                if (Mathf.Abs(spawnVector.y) <= 4) {
                    spawnVector.y *= 3;
                }
                //Same as above. Sorry, no funny explanation here.
                GameObject Asteroid = Instantiate(AsteroidPrefab, spawnVector, Quaternion.identity) as GameObject;
                Asteroid.transform.parent = transform;
				Asteroid.GetComponent<AsteroidPrefabScript> ().sortingLayerNumber = sortingLayerOfAsteroids;
            }
        }
    }
}
