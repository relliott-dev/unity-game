using FishNet.Object;
using UnityEngine;

namespace RDE
{
    /// <summary>
    /// 
    /// Handles the networking aspects of the character, managing the synchronization of character states and actions across the network
    /// It synchronizes various character attributes such as movement, stats, and status effects
    /// 
    /// @TODO:
    /// - Add RPCs
    /// - Add server-side validation
    /// - Test networking and lag
    /// 
    /// </summary>
    public class CharacterNetworkManager : NetworkBehaviour
    {
        #region Variables

        [Header("Flags")]
        [HideInInspector] public bool canMove = true;
        [HideInInspector] public bool isBoosting = false;
        [HideInInspector] public bool isDead = false;

        [Header("Stats")]
        [HideInInspector] public float currentHealth;
        [HideInInspector] public float currentEnergy;
        [HideInInspector] public float currentHeat;

        [Header("Status")]
        [HideInInspector] public bool isInterruptible;
        [HideInInspector] public bool isStunned;
        [HideInInspector] public bool isRooted;
        [HideInInspector] public bool isInvincible;

        #endregion
    }
}