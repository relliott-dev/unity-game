using UnityEngine;
using System.Collections;

public class FogNoiseSpawnerScript : MonoBehaviour {
    // Welcome to the Fog Noise Script! This nifty little piece of code instantiates the background fog, one of the most important
    // atmospheric pieces of this package. Have fun with it!

	// Here, you throw in the NoiseFogPrefabs, depending on which srting layer you want them.
	public Transform[] FogPrefabs; 

	void Start () {
		// The front layer should always be spawned, because space looks really empty without some fine fog going on
        Instantiate(FogPrefabs[0]);
        // Next, we let the game randomly decide if another layer should spawn in the Middle Layer, the Back layer, both. Dont worry, all 3 options are beautiful.
        int whichFogToSpawn = Random.Range(0, 3);
        if(whichFogToSpawn == 0) {
			// Additionally to the Front Layer, another Fog is spawned behind it.
            Instantiate(FogPrefabs[1]);
        }
        if (whichFogToSpawn == 1) {
			// Additionally to the Front Layer, another Fog is spawned far behind it.
            Instantiate(FogPrefabs[2]);
        }
        if (whichFogToSpawn == 2) {
			// Additionally to the Front Layer, Fogs are spawned both behind and far behind.
            Instantiate(FogPrefabs[1]);
            Instantiate(FogPrefabs[2]);
        }
    }
    // But, why is there no random Option wih no additional Fogs spawning?
    // - Easy. If there are 2 different fogs with different parallax effects in the game, the fog does not look like one static object, but gets the depth it needs for a realistic feeling.

    // But, why do you spawn the Front Fog? I mean, isn't it nicer if sometimes, Object Spawn in front of it?
    // Well, it is a question of Style, obviously. However, if every non-interactive object is behind the fog, ergo darker and a little bit tinted,
    // while the player and his enemies are in front of it, clarity is added. In one glimpse, the player is able to see where and what the enemies
    // are, while not taking away anything from the atmosphere of the entire game. Clarity like this helps the game being easier to grasp for 
    // the player. Also, it enables putting some enemies behind the fog, therefore messing with the player, which is always fun.
}
