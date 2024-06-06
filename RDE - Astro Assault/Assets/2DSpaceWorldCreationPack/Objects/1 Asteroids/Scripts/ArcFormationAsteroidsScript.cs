using UnityEngine;
using System.Collections;

public class ArcFormationAsteroidsScript : MonoBehaviour {
    // Welcome to one of the more complex Scripts. If my explanation confuse you, feel free to shoot me a message!

    // Throw the default Asteroid Prefab in here.
    public GameObject AsteroidPrefab;

    // This here decides how far the asteroids should be darkened and their parallax. Drawn in from the SLR.
	public int sortingLayerOfAsteroids;

    void Start() {
        // First of, we want our Asteroid field to have a random direction each time this fires. Keeps things chaotic and organic.
		transform.Rotate(0, 0, Random.Range(0, 360));

        // This is needed for the upcoming spawning of the asteroids, just wait.
        int numberOfAsteroidsSpawned = 0;

        // Feel free to fiddle with the number. The higher, the more asteroids will form an arc, increasing its density.
        int numberOfAsteroidstoSpawn = Random.Range(15, 25);

        // Getting the sorting layer now, so that we don't have to worry about it in each repetition of the "For".
		sortingLayerOfAsteroids = GetComponent<SortingLayerRelayScirpt> ().sortingLayerFromWorldSpawner;

        // Counting up the Asteroids until we have reached our desired number.
        for (numberOfAsteroidsSpawned = 0; numberOfAsteroidsSpawned <= numberOfAsteroidstoSpawn; numberOfAsteroidsSpawned++) {
            // First, we need a base for the Arc to spread out from. I found that 8 randomly bundled asteroids form a fine base.
            if (numberOfAsteroidsSpawned <= 8) {
                // Once more, in the demo, the base size is sufficient between 0 and 5 World Units. Fiddle with it if you want something different.
                Vector3 spawnVector = transform.position + new Vector3(Random.Range(0, 5), Random.Range(-5, 5), 0);
                // Instantiating the Asteroid at the spawnVector above, uncluttering the hierarchy by parent setting and handing over the sorting layer.
                GameObject Asteroid = Instantiate(AsteroidPrefab, spawnVector, Quaternion.identity) as GameObject;
                Asteroid.transform.parent = transform;
				Asteroid.GetComponent<AsteroidPrefabScript> ().sortingLayerNumber = sortingLayerOfAsteroids;
            } else {
                // Let me explain how this block works first:
                // First, spawnPosY forms the width of the arc. If it is too small, Asteroids would Spawn in the middle of the Arc, therefore it
                // would be no Arc.
                // Next, the length is decided by how far an asteroid is from the center. The further the asteroid is away in one direction, the
                // further it should be away in the other, too, therefore forming an Arc. Now in casu:

                // Deciding how far the spawned asteroid will be from the center (width measurement)
                float spawnPosY = Random.Range(-10, 10);
                // Making sure the asteroid does not at up to the base we set up before by sending it further out if it is too close
                if (Mathf.Abs(spawnPosY) <= 4) {
                    spawnPosY *= 3;
                }
                // Moving the asteroid along the length, depending on how far it is width wise from the center.
                // the MathF.Abs make sure that the Asteroid spawn only in one direction, without it, this would create an X-Form
                // Also, the width is taken twice and divided by ten to create a bending effect instead of a plain linear V-Form.
                float spawnPosX = Mathf.Abs(spawnPosY)*Mathf.Abs(spawnPosY) /10;

                // Setting the Spawn Vector and instantiating like above.
                Vector3 spawnVector = transform.position + new Vector3(spawnPosX, spawnPosY, 0);
                GameObject Asteroid = Instantiate(AsteroidPrefab, spawnVector, Quaternion.identity) as GameObject;
                Asteroid.transform.parent = transform;
				Asteroid.GetComponent<AsteroidPrefabScript> ().sortingLayerNumber = sortingLayerOfAsteroids;
            }
        }
    }
}