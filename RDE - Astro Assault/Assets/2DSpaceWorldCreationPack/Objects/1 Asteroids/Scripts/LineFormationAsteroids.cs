using UnityEngine;
using System.Collections;

public class LineFormationAsteroids : MonoBehaviour {
    // Welcome to the Line Formation Script! Just a small patch of asteroids, floating cuddly through space and then something
    // with gravity comes along and stretches them out. Or something like that. At least, it looks awesome!

    // Throw the default Asteroid Prefab in here.
    public GameObject AsteroidPrefab;

    // This here decides how far the asteroids should be darkened and their parallax. Drawn in from the SLR.
    public int sortingLayerOfAsteroids;

    void Start() {
        // First of, we want our Asteroid field to have a random direction each time this fires.Keeps things chaotic and organic.
        transform.Rotate(0, 0, Random.Range(0, 360));

        // This is needed for the upcoming spawning of the asteroids, just wait.
        int numberOfAsteroidsSpawned = 0;

        // Feel free to fiddle with the number. The higher, the more asteroids will form the outer ring increasing its density.
        // In tht case however, I would suggest increase the treshold for center asteroids, too.
        int numberOfAsteroidstoSpawn = Random.Range(15, 25);

        // Getting the sorting layer now, so that we don't have to worry about it in each repetition of the "For".
        sortingLayerOfAsteroids = GetComponent<SortingLayerRelayScirpt>().sortingLayerFromWorldSpawner;

        // Counting up the Asteroids until we have reached our desired number.
        for (numberOfAsteroidsSpawned = 0; numberOfAsteroidsSpawned <= numberOfAsteroidstoSpawn; numberOfAsteroidsSpawned++) {
            // These Asteroids make up the center. While being rather restricted along the x-dimension, the high Random.Range in y-dimension
            // form a nice basis for the line formation.
            if (numberOfAsteroidsSpawned <= 8) {
                // While you can of course increase the distance the asteroids have in this Random.Range, I suggest sticking to roughly a
                // 1:3 relation for the line formation.
                Vector3 spawnVector = transform.position + new Vector3(Random.Range(-3, 3), Random.Range(-10, 10), 0);
                // Instantiating the Asteroid at the spawnVector above, uncluttering the hierarchy by parent setting and handing over the sorting layer.
                GameObject Asteroid = Instantiate(AsteroidPrefab, spawnVector, Quaternion.identity) as GameObject;
                Asteroid.transform.parent = transform;
            } else {
                // The next Asteroids may spread further from the center. In fact, we want that. After drawing such a strong line in the center
                // we want the rest of the asteroids to spread chaotically around, otherwise the entire structure looks to forced.
                Vector3 spawnVector = transform.position + new Vector3(Random.Range(-5, 5), Random.Range(-10, 10), 0);
                //Yes, we force them to spread out, so that it does not look forced. Ironic, eh?
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