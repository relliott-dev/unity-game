using UnityEngine;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages the enemy behaviors in a ScriptableObject
    /// This script handles the stats for each enemy behavior
    /// 
    /// </summary>
    [CreateAssetMenu(fileName = "New Enemy Behavior", menuName = "Character/EnemyBehavior", order = 1)]
    public class EnemyBehavior : ScriptableObject
    {
        [Header("Detection Settings")]
        [Tooltip("How often the enemy attempts to find targets")]
        public float findTargetInterval = 4f;
        [Tooltip("The range within which the enemy can detect targets")]
        public float visibleRange = 15f;
        [Tooltip("The angle within which targets are detectable in front of the enemy")]
        public float visibleAngle = 60f;

        [Header("Idle Settings")]
        [Tooltip("Time spent idling between actions when no targets are present")]
        public float idleInterval = 4f;

        [Header("Patrol Settings")]
        [Tooltip("Time between changing patrol points or direction")]
        public float patrolInterval = 4f;
        [Tooltip("Radius within which the enemy can wander while patrolling")]
        public float wanderRadius = 10f;

        [Header("Combat Settings")]
        [Tooltip("Time between combat actions")]
        public float combatInterval = 4f;
        [Tooltip("Health threshold at which the enemy decides to retreat (proportion of max health)")]
        public float retreatThreshold = 0.25f;

        [Header("Movement Settings")]
        [Tooltip("Maximum speed the enemy can move")]
        public float maxSpeed = 5f;
        [Tooltip("Rate at which the enemy accelerates to max speed")]
        public float accelerationSpeed = 3f;
        [Tooltip("Speed at which the enemy can rotate")]
        public float rotationSpeed = 5f;
    }
}