using UnityEngine;
using System.Collections;

public class ExampleTurret : MonoBehaviour
{
    public SpriteRenderer lights;

    public Sprite redSprite, greenSprite, yellowSprite;

    public TransitionalObject lightsTransition;

    public void SetState(ExampleWeapon.State state)
    {
        if(lights != null)
            switch(state)
            {
                case ExampleWeapon.State.Waiting:
                    lights.sprite = redSprite;
                    break;

                case ExampleWeapon.State.Firing:
                    lights.sprite = greenSprite;

                    if(lightsTransition != null)
                    {
                        lightsTransition.JumpToEnd();
                        lightsTransition.Stop();
                    }
                    break;

                case ExampleWeapon.State.Reloading:
                    lights.sprite = yellowSprite;

                    if(lightsTransition != null)
                        lightsTransition.TriggerTransition();
                    break;
            }
    }

}
