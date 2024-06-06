using UnityEngine;
using System.Collections;

public class DustSpawnerScript : MonoBehaviour {
    // Welcome to the Dust Spawner Script! So, this Script handles the spawning of little dustspecs, downgrinded asteroids or ships.
    // Who knows why they are there, but they look just too awesome not to have them!

	// Throw in all the associated Dust Cloud Prefabs.
    public GameObject[] Cloudprefab;
    
    void Start() {
        // Dust Clouds should be vastly different in size, as small and huge ones give of very different feelings.
        // Thats why we have such a huge delta in the Random Range. However, this is just my opinion. 
        // If you want to make an entire dustcloud field, feel free to send me a screen shot. Should be funny!
        int numberOfCloudsSpawned = 0;
        int numberOfCloudstoSpawn = Random.Range(2, 9);
		// So this "for" here. First, we check that the number of dustclouds at the beginning really is zero.
		// Then, we check if enough Dust Clouds have spawned. Finally, after each iteration, we increase the count.
        for (numberOfCloudsSpawned = 0; numberOfCloudsSpawned <= numberOfCloudstoSpawn; numberOfCloudsSpawned++) {
			// By picking one cloud Prefab at random, thing get just more beautiful.
			int whichCloudtoSpawn =Random.Range(0, Cloudprefab.Length);
            // So, for the positioning of each new cloud: By increasing the max of the Random Range, the field "moves" outward from its starting
            // position, but may still just stack up on the same place, creating another interesting Dust texture.
	        Vector3 spawnVector = transform.position + new Vector3(Random.Range(0, numberOfCloudsSpawned*2), Random.Range(0,numberOfCloudsSpawned * 2), 0);
            // And of course, the usual Instantiation.
            GameObject Cloud = Instantiate(Cloudprefab[whichCloudtoSpawn], spawnVector, Quaternion.identity) as GameObject;
	        Cloud.transform.parent = transform;
        }
    }
}
