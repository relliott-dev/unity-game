using UnityEngine;
using System.Collections;
using LaireonGames;

public class GUIControlsWP : MonoBehaviour
{
    public GameObject[] weapons, platforms, bullets;

    public float platformSize = 1, weaponSize = 1, bulletSize = 1, firingSpeed = 0.15f, reloadTime = 0.25f;
    bool muzleFlash = true;

    int currentWeapon, currentPlatform, currentBullet;

    GUIStyle style = new GUIStyle();

    void Start()
    {
        for(int i = 1; i < weapons.Length; i++)
            weapons[i].SetActive(false);

        for(int i = 1; i < platforms.Length; i++)
            platforms[i].SetActive(false);

        style.normal.background = MakeTex(1, 1, new Color(0.5f, 0.5f, 0.5f, 0.5f));
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();//add a padding to the left of the screen
        GUILayout.Space(10);
        GUILayout.BeginVertical(style);

        #region Weapons
        GUILayout.Label("Weapons");

        GUILayout.BeginHorizontal();

        if(GUILayout.Button("<<"))
        {
            weapons[currentWeapon].SetActive(false);

            currentWeapon--;

            if(currentWeapon < 0)
                currentWeapon = weapons.Length - 1;

            UpdateWeapon();
            UpdatePlatform();
        }

        if(GUILayout.Button(">>"))
        {
            weapons[currentWeapon].SetActive(false);

            currentWeapon = (currentWeapon + 1) % weapons.Length;

            UpdateWeapon();
            UpdatePlatform();
        }

        GUILayout.EndHorizontal();

        #region Weapon Size
        GUILayout.Label("Weapon Size");
        weaponSize = GUILayout.HorizontalSlider(weaponSize, 0.25f, 1f);

        weapons[currentWeapon].transform.localScale = new Vector3(weaponSize, weaponSize, weaponSize);
        #endregion

        #region Muzzle Flash
        muzleFlash = GUILayout.Toggle(muzleFlash, new GUIContent("Muzzle Flash"));
        weapons[currentWeapon].GetComponent<ExampleWeapon>().muzzleFlashEnabled = muzleFlash;
        #endregion
        #endregion

        GUILayout.Space(10);

        #region Platforms
        GUILayout.Label("Platforms");

        GUILayout.BeginHorizontal();

        if(GUILayout.Button("<<"))
        {
            platforms[currentPlatform].SetActive(false);

            currentPlatform--;

            if(currentPlatform < 0)
                currentPlatform = platforms.Length - 1;

            platforms[currentPlatform].SetActive(true);
            UpdatePlatform();
        }

        if(GUILayout.Button(">>"))
        {
            platforms[currentPlatform].SetActive(false);

            currentPlatform = (currentPlatform + 1) % platforms.Length;

            platforms[currentPlatform].SetActive(true);
            UpdatePlatform();
        }

        GUILayout.EndHorizontal();

        GUILayout.Label("Platform Size");
        platformSize = GUILayout.HorizontalSlider(platformSize, 0.25f, 1f);

        platforms[currentPlatform].transform.localScale = new Vector3(platformSize, platformSize, platformSize);
        #endregion

        GUILayout.Space(10);

        #region Bullets
        GUILayout.Label("Bullets");

        GUILayout.BeginHorizontal();

        if(GUILayout.Button("<<"))
        {
            currentBullet--;

            if(currentBullet < 0)
                currentBullet = bullets.Length - 1;

            UpdateBullet();
        }

        if(GUILayout.Button(">>"))
        {
            currentBullet = (currentBullet + 1) % bullets.Length;
            UpdateBullet();
        }

        GUILayout.EndHorizontal();

        GUILayout.Label("Bullet Size");
        bulletSize = GUILayout.HorizontalSlider(bulletSize, 0.25f, 1f);

        weapons[currentWeapon].GetComponent<ExampleWeapon>().bulletScale = bulletSize;
        #endregion

        GUILayout.Space(25);

        #region Firing and Reloading
        GUILayout.Label("Firing Speed");
        firingSpeed = GUILayout.HorizontalSlider(firingSpeed, 0.25f, 1f);

        GUILayout.Label("Reload Time");
        reloadTime = GUILayout.HorizontalSlider(reloadTime, 0.25f, 5f);

        weapons[currentWeapon].GetComponent<ExampleWeapon>().firingSpeed = firingSpeed;
        weapons[currentWeapon].GetComponent<ExampleWeapon>().reloadTime = reloadTime;
        #endregion

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    void UpdateWeapon()
    {
        weapons[currentWeapon].SetActive(true);
        weapons[currentWeapon].GetComponent<ExampleWeapon>().enabled = true;

        bulletSize = weapons[currentWeapon].GetComponent<ExampleWeapon>().bulletScale;
        firingSpeed = weapons[currentWeapon].GetComponent<ExampleWeapon>().firingSpeed;
        reloadTime = weapons[currentWeapon].GetComponent<ExampleWeapon>().reloadTime;
    }

    void UpdatePlatform()
    {
        weapons[currentWeapon].GetComponent<ExampleWeapon>().turret = platforms[currentPlatform].GetComponent<ExampleTurret>();
    }

    void UpdateBullet()
    {
        weapons[currentWeapon].GetComponent<ExampleWeapon>().bulletPrefab = bullets[currentBullet].GetComponent<ParralaxItem>();
    }


    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];

        for(int i = 0; i < pix.Length; i++)
            pix[i] = col;

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();

        return result;
    }
}
