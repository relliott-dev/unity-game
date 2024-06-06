using UnityEngine;
using System.Collections;

public class AnimatedExampleWeapon : ExampleWeapon
{
    public TransitionalObject lightsTransition, recoilAnimation;

    public override void SetState(State state)
    {
        base.SetState(state);

        switch(state)
        {
            case State.Firing:
                if(recoilAnimation != null)
                    recoilAnimation.TriggerTransition();

                if(lightsTransition != null)
                {
                    lightsTransition.JumpToEnd();
                    lightsTransition.Stop();
                }
                break;

            case State.Reloading:
                lights.sprite = yellowSprite;

                if(lightsTransition != null)
                    lightsTransition.TriggerTransition();

                if(recoilAnimation != null)
                    recoilAnimation.Stop();
                break;
        }
    }

    protected override void StartFiring()
    {
        if(recoilAnimation == null)//the recoil aniamtion handles firing, but if there is no animation then run the base method
            base.StartFiring();
    }

    protected override IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);

        if(lightsTransition != null)
            lightsTransition.Stop();

        SetState(State.Firing);//will also fire a bullet
    }

    public override void FireBullet()
    {
        #region Update Firing Speed
        if(recoilAnimation != null)
            if(recoilAnimation.MovingTransition.fadeOutTime != firingSpeed)//for most cases this should be fine to update here. If you need an instant update you can move this to an Update method
                recoilAnimation.MovingTransition.fadeOutTime = firingSpeed;
        #endregion

        if(state == State.Waiting)
        {
            if(recoilAnimation != null)
                recoilAnimation.Stop();//stop moving the gun
        }
        else
            base.FireBullet();
    }
}
