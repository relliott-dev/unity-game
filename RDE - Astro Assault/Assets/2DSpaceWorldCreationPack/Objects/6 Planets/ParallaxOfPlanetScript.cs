using UnityEngine;
using System.Collections;

public class ParallaxOfPlanetScript : MonoBehaviour {
    // Welcome to the Planet Parallax Script. Plenty easier then the Object Parallax Script.
    // As I wanted to be able to give the planets different distances to begin with, I made an own parallax script for them.
    // The main issue with putting them on the layer Settings of the object parallax is, that it really looks horrible
    // if asteroids etc have the same parallax as a planet, which is way bigger. Therefore, the FurthestBack layer is for planets (and Starfields).

    // Aside from that, it has the same functionality as the object parallax.
    // Define the strength of the planet parallax here.
    public float parallax;
    // Remember original position.
    Vector3 origPos;
    // Get an object of refernce for the parallax.
    GameObject player;
    void Start() {
        origPos = transform.position;
        //set the parallax to default if nothing has been entered. If you really need close to 0 parallax, enter 0.001f;
        if (parallax == 0) {
            parallax = 0.4f;
        }
    }

    void Update() {
        // I chose to let the objects position be relative to the player. If you want, you can put this FindPlayer into start, just make sure
        // you tell the game that the parallax script gets executed after the player has been set into the scene.
        // If you stick with this version, be aware that once there is no object with the desired tag (or multiples), problems may arise.
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("PlayerShipTag");
        }
        // Transforming the position relative to the player, multiplied by the parallax, so that only a fracture (or multiple) of the playerposition
        // is added to the baseposition of the object. This gives the desired parallax Effect, greatly enhancing the feeling of depth in a 2D world.
        Vector3 parallaxedPos = new Vector3(origPos.x + player.transform.position.x * parallax, origPos.y + player.transform.position.y * parallax, 0);
        transform.position = parallaxedPos;
    }
}
