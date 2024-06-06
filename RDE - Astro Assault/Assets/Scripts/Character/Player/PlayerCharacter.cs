using UnityEngine;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages the character classes in a ScriptableObject
    /// This script handles the stats for each character class
    /// 
    /// </summary>
    [CreateAssetMenu(fileName = "New Player Character", menuName = "Character/PlayerCharacter", order = 1)]
    public class PlayerCharacter : ScriptableObject
    {
        public enum ClassType
        {
            SwiftwingScout,
            VanguardFighter,
            IroncladBehemoth,
            PhantomSniper,
            AegisSupport
        }

        [Header("Class Details")]
        [Tooltip("Character class")]
        public ClassType classType;
        [Tooltip("Icon representing the character class")]
        public Sprite characterIcon;
        [TextArea(3, 10)]
        [Tooltip("Detailed description of the character class")]
        public string description;

        [Header("Health Stats")]
        [Tooltip("Maximum health the character can have")]
        public float maxHealth;
        [Tooltip("Health recovery rate per second")]
        public float healthRegen;

        [Header("Energy Stats")]
        [Tooltip("Maximum energy capacity")]
        public float maxEnergy;
        [Tooltip("Energy recovery rate per second")]
        public float energyRegen;

        [Header("Heat Stats")]
        [Tooltip("Maximum heat before overheating occurs")]
        public float maxHeat;
        [Tooltip("Heat dissipation rate per second")]
        public float heatRegen;

        [Header("Mobility Stats")]
        [Tooltip("Movement speed of the character")]
        public float movementSpeed;
        [Tooltip("Attack speed of the character")]
        public float attackSpeed;
    }
}