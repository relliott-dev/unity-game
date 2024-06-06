using UnityEngine;

namespace RDE
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Pick-Up/Weapon")]
    public class Weapon : ScriptableObject
    {
        public enum WeaponType
        {
            Homing,
            Shotgun,
            Beam,
            Mine,
            Railgun,
            AreaOfEffect,
            Burst,
            Single,
            MachineGun,
            Sniper,
            EnergyPulse,
            ChainLightning
        }

        [Header("Weapon Type")]
        public WeaponType type;

        [Header("Visuals")]
        [Tooltip("Icon representing the weapon")]
        public Sprite weaponIcon;
        [TextArea(3, 10)]
        [Tooltip("Detailed description of the weapon")]
        public string description;


        [Header("Weapon Properties")]
        public GameObject weaponPrefab;
        public AudioClip attackSound;
        public float attackSpeed;
        public float criticalHitChance;
        public float criticalHitMultiplier;

        [Header("Combat Mechanics")]
        public bool useDropPoint;
        public float recoilForce;
        public float energyCost;
        public float heatCost;
    }
}