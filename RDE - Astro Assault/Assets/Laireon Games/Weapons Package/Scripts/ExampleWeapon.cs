using UnityEngine;
using System.Collections;
using LaireonGames;

public class ExampleWeapon : MonoBehaviour
{
    public enum State { Waiting = 0, Reloading, Firing }

    public bool muzzleFlashEnabled = true;

    public ParticleSystem muzzleFlash;//if this is included then a muzzle flash will spawn on the firing points as they are used
    public ParralaxItem bulletPrefab;
    public float bulletScale = 1;

    ParticleSystem[] muzzleFlashes;

    public Transform[] firingPoints;//where bullets and flashes are spawned from

    public SpriteRenderer lights;

    public Sprite redSprite, greenSprite, yellowSprite;

    [Range(0.15f, 1)]
    public float firingSpeed = 0.15f;

    public float reloadTime = 0.25f;
    public int shotsBeforeReload = 1;
    int shotsFired;

    protected State state;

    public ExampleTurret turret;

    void Start()
    {
        if(muzzleFlash != null)
        {
            muzzleFlashes = new ParticleSystem[firingPoints.Length];

            for(int i = 0; i < firingPoints.Length; i++)
            {
                muzzleFlashes[i] = Instantiate<GameObject>(muzzleFlash.gameObject).GetComponent<ParticleSystem>();//spawn muzzle flashes for each firing point
                muzzleFlashes[i].transform.parent = firingPoints[i];//parent to this transform
                muzzleFlashes[i].transform.localPosition = Vector3.zero;//position over the firing point
                muzzleFlashes[i].transform.localScale = Vector3.one;//fix scaling issues
                muzzleFlashes[i].transform.forward = firingPoints[i].up;
            }

        }
    }

    void OnEnable()
    {
        SetState(State.Firing);
    }

    public virtual void SetState(State state)
    {
        this.state = state;

        if(turret != null)
            turret.SetState(state);

        switch(state)
        {
            case State.Waiting:
                lights.sprite = redSprite;
                break;

            case State.Firing:
                lights.sprite = greenSprite;

                FireBullet();
                StartFiring();
                break;

            case State.Reloading:
                lights.sprite = yellowSprite;

                StartCoroutine(Reload());
                break;
        }
    }

    protected virtual void StartFiring()
    {
        StartCoroutine(Fire());//start firing bullets
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(firingSpeed);

        FireBullet();

        if(shotsFired != 0)//if we are not reloading 
            StartCoroutine(Fire());//continue to fire
    }

    protected virtual IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);

        SetState(State.Firing);//will also fire a bullet
    }

    public virtual void FireBullet()
    {
        shotsFired++;

        if(shotsFired > shotsBeforeReload)
        {
            shotsFired = 0;
            SetState(State.Reloading);
        }
        else
            for(int i = 0; i < firingPoints.Length; i++)
            {
                if(muzzleFlashes != null && muzzleFlashEnabled)
                    muzzleFlashes[i].Play();//spawn some particles

                if(bulletPrefab != null)//fire a bullet
                {
                    ParralaxItem temp = Instantiate<GameObject>(bulletPrefab.gameObject).GetComponent<ParralaxItem>();
                    temp.transform.position = firingPoints[i].transform.position;
                    temp.transform.localScale = new Vector3(bulletScale * bulletPrefab.transform.localScale.x, bulletScale * bulletPrefab.transform.localScale.y, bulletScale * bulletPrefab.transform.localScale.z);

                    temp.transform.rotation = firingPoints[i].rotation;
                    temp.minDirection = firingPoints[i].transform.up;
                    temp.maxDirection = temp.minDirection;
                }
            }
    }
}
