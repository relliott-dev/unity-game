using UnityEngine;
using System.Collections;

public class RingFormationAsteroidsScript : MonoBehaviour {
    // Welcome to the Ring Formation Script, one of my favorites. We got one cute asteroid alone in the middle
    // and a bunch of others gathering around, keeping their reverend (or mean) distance.

    // Throw the default Asteroid Prefab in here.
    public GameObject AsteroidPrefab;

    // This here decides how far the asteroids should be darkened and their parallax. Drawn in from the SLR.
    public int sortingLayerOfAsteroids;

    void Start() {
        // First of, we want our Asteroid field to have a random direction each time this fires.Keeps things chaotic and organic.
        transform.Rotate(0, 0, Random.Range(0, 360));

        // Getting the sorting layer now, so that we don't have to worry about it in each repetition of the "For".
        // Also, we need it for the one asteroid in the middle.
        sortingLayerOfAsteroids = GetComponent<SortingLayerRelayScirpt> ().sortingLayerFromWorldSpawner;
        
        // First, we spawn the center asteroid right where the Spawner is located.
		Vector3 spawnVector = transform.position;
        // Instantiating the Asteroid at the spawnVector above, uncluttering the hierarchy by parent setting and handing over the sorting layer.
        GameObject CenterAsteroid = Instantiate(AsteroidPrefab, spawnVector, Quaternion.identity) as GameObject;
		CenterAsteroid.transform.parent = transform;
		CenterAsteroid.GetComponent<AsteroidPrefabScript> ().sortingLayerNumber = sortingLayerOfAsteroids;

        // This is needed for the upcoming spawning of the asteroids, just wait.
        int numberOfAsteroidsSpawned = 0;

        // Feel free to fiddle with the number. The higher, the more asteroids will form the outer ring increasing its density.
        // In tht case however, I would suggest increase the treshold for center asteroids, too.
        int numberOfAsteroidstoSpawn = Random.Range(15, 25);

        // Counting up the Asteroids until we have reached our desired number.
        for (numberOfAsteroidsSpawned =0; numberOfAsteroidsSpawned <= numberOfAsteroidstoSpawn; numberOfAsteroidsSpawned++){
            // First we decide on the y-dimension. It doesn't matter if the number is big or small, we just have to adjust the x-dimension
            // accordingly, so that all asteroids have a certain minimal distance to the center one.
            float AsteroidPosY = Random.Range(-15, 15);
            // Declaring it here, because Unity does not like it if we just delcare it in both if & else.
            float AsteroidPosX;
            // Now the script sees if the asteroid is on one side of the center line or the other. As we want both sides to form kind of a
            // circle, both sides are equally valued. If you want a semicircle, just set the following Random.Range to (0,0)
            int asteroidRightOrLeft = Random.Range(0, 1);
            if (asteroidRightOrLeft == 1) {
                // So, what does this here do?
                // If the Y is close to 0, this term will be heavily negative, therefore a y=0 means x=15. Other way round does not work exactly the same
                // But that is fine, as a "perfect" circle around the core piece looks very artifical.
                // That we use the AsteroidPosY twice helps also with a more circular formation. Otherwise, it would just be straight, boring lines.
                AsteroidPosX = AsteroidPosX = (AsteroidPosY * AsteroidPosY / 10) - 15f;
            } else {
                // And, if the Random.Range above decides it, the entire term just turns negative, placing an asteroid on the opposite side.
                AsteroidPosX = -1f * ((AsteroidPosY * AsteroidPosY / 10) - 15f);
            }
            // The following addition serves one purpose: It makes them appear a bit mroe chaotic again.
            // If the asteroids would form a perfect circle, it would feel very unnatural (it does, I tried it). This little Random.Range
            // shakes them up a bit, for the often mentioned organic feel.
            AsteroidPosX += Random.Range(-2.5f, 1.2f);

            // Same old, same old. Defining the Vector, instantiating, tidying hierarchy, sending layer information
            spawnVector = transform.position + new Vector3(AsteroidPosX, AsteroidPosY, 0);
            GameObject Asteroid = Instantiate(AsteroidPrefab, spawnVector, Quaternion.identity) as GameObject;
            Asteroid.transform.parent = transform;
			Asteroid.GetComponent<AsteroidPrefabScript> ().sortingLayerNumber = sortingLayerOfAsteroids;
        }
    }
}