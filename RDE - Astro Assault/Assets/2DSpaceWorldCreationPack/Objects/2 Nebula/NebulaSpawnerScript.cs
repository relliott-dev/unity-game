using UnityEngine;
using System.Collections;

public class NebulaSpawnerScript : MonoBehaviour {
    // ...And welcome to the Nebula Spawner Script. Very simple.

    // Throw in the Nebula Prefabs
    public Transform[] Nebulas;

	void Start () {
        // Let Unity decide which Nebula it wants to spawn
        int whichNebulaToSpawn = Random.Range(0, Nebulas.Length);

        //Instantiate said Nebula and unclutter the hierarchy
        Transform NebulaToSpawn = Instantiate(Nebulas[whichNebulaToSpawn], transform.position, Quaternion.identity) as Transform;
        NebulaToSpawn.transform.parent = transform;
	}
}
