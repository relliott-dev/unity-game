using UnityEngine;
using System.Collections;

public class FogRandomizerScript : MonoBehaviour {
	// Hey, hi there again and welcome to the Fog Randomizer Script!
	// This script allows the fogs to change color, therefore creating all this beautiful space background noise.

	// I also included a few different fogs (nine, I guess), if you want your background noise to look differently from the standart fog.

	void Start () {
        // Okay, so this generates the nebula color randomly. I am once more astonished, how beautiful the results are.
		// If you want to make the entire scene darker, I provided a swap codebit at the end of the script.  
        float redValue = Random.Range(0, 256);
		float greenValue = Random.Range(0, 256);
		float blueValue = Random.Range(0, 256);
        // Also, a small difference in the alpha values allows the nebulas to look quite different
        float alphaValue = Random.Range(200, 250);

        // Setting the spriterenderer to the desired color, by diving our values by 255. In the new Color code, Unity needs a float between 0 and 1 to work with.
		Color nebulaColor = new Color(redValue / 255, greenValue / 255, blueValue / 255, alphaValue/255);
        GetComponent<SpriteRenderer>().color = nebulaColor;

        // Last but not least, we randomly flip the rotation of the nebula by 180°, defacto doubling the value of the blank prefabs. This makes everything a bit more interesting.
        int flipNebula = Random.Range(0,2);
        if (flipNebula == 1) {
            transform.rotation.Set(0, 0, 180, 0);
        }
	}

	void Update(){
		//As the ParallaxAndScaleScript messes with the Background noise as well, we will unmess this here.
		if (transform.localScale.x != 1 || transform.localScale.y != 1) {
			transform.localScale = new Vector3 (1, 1, 1);
		}
	}
		
}


/* As promised, here is the darker swap bit. If you want it even darker, mess with the colors. Brighting things up works analog. If you can't figure it out, shoot me a message. But I have confidence in you. You can do it!
 * float redValue = Random.Range(0, 210) / 255;
	float greenValue = Random.Range(0, 210) / 255;
	float blueValue = Random.Range(0, 210) / 255;
	*/
