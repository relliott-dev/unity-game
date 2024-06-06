using UnityEngine;
using System.Collections;

public class NebulaPulseScript : MonoBehaviour {
	// Welcome to the Nebula Pulse Script. Nothing more beautiful than seeing diffuse matter actually ... diffusing. And moving.

	// So... what does it do?
	// Basically, it takes a beautiful nebula sprite, makes it pulse a bit in the size and rotates it. It's simple, yet beautiful.

	// Bool for flipping between growing and shrinking.
    bool nebulaGrowing = true;

	// Initial nebula grow time, lower then the normal min to get more "action" faster.
    float nebulaGrowthTime = 4f;

	// This initializes the nebula between a minimum and maximum size at start.
    float nebulascale;

	// Those are the values that later on determine the nebula size in X and Y Size.
    float nebulascaleX;
    float nebulascaleY;

    void Start() {
		// Initializing the nebula at a random size.
        nebulascale = Random.Range(0.75f, 1.5f);
        transform.localScale = new Vector3(nebulascale, nebulascale, nebulascale);
		// Initializing the X and Y scaling basis.
        nebulascaleX = nebulascale;
        nebulascaleY = nebulascale;
		// Rotating it randomly, more chaos = more realism. At least in this case.
		transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
    }
	
	void Update () {
		// The usual timer which decides if the nebula stops growing or shrinking.
        nebulaGrowthTime -= Time.deltaTime;
		// A little bit  of Rotation movement makes the entire scene more alive.
        transform.Rotate(Vector3.back * Time.deltaTime *0.8f);

		// Bool check if the nebula grows or shrinks.
        if (nebulaGrowing) {
			// By Randomizing a bit, the nebula looks more organic.
            nebulascaleX += Time.deltaTime / Random.Range(90, 180);
            nebulascaleY += Time.deltaTime / Random.Range(90, 180);
            transform.localScale = new Vector3(nebulascaleX, nebulascaleY, nebulascale);
			// And here the check if the nebula should stop growing or not.
            if (nebulaGrowthTime <= 0) {
                nebulaGrowing = false;
                nebulaGrowthTime = Random.Range(5f, 10f);
            }
        } else {
			// Analogue to the top.
            nebulascale = Time.deltaTime/ Random.Range(90, 180);
            nebulascaleX -= Time.deltaTime / Random.Range(90, 180);
            nebulascaleY -= Time.deltaTime / Random.Range(90, 180);
            transform.localScale = new Vector3(nebulascaleX, nebulascaleY, nebulascale);
            if (nebulaGrowthTime <= 0) {
                nebulaGrowing = true;
                nebulaGrowthTime = Random.Range (5f,10f);
            }
			// Yes, it can theoretically be, that the nebula at a certain point gets gigantic or very small. However, in all my numerous playthroughs, I had consistent positive results in the size, so It should be fine. I would however
			// not encourage a 3-day test just to the what size the nebulae have after that period. Might be weird.
        }
    }
}
