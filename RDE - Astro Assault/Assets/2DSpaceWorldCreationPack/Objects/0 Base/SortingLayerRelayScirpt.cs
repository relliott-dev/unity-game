using UnityEngine;
using System.Collections;

public class SortingLayerRelayScirpt : MonoBehaviour {
    //So, the SLR-Script. Looks simple, is crucial. The World Spawner will write into this Script, therefore giving the information
    //how far in the back the object(s) to be spawned are. This ultimately leads to them becoming darker and changin ttheir parallax.
    //Jup, this is just a storage class. But by adding this to every SpawnerPrefab, we don't have to tell the WorldSpawner to check for
    //each and every possible Class of Spawner there is.
    //This simple One-Line Storage script greatly enhances the performance.

	public int sortingLayerFromWorldSpawner;
}
