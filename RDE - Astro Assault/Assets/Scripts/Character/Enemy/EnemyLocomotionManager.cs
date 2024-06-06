using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages enemy locomotion including idle behaviors, patrolling routines, and combat actions
    /// This manager uses state-driven behavior to dynamically adjust enemy actions based on game conditions, such as proximity to the player, health levels, and strategic decisions
    /// 
    /// @TODO:
    /// - Implement the "Alert" state
    /// - Implement the "Who Attacked Me" system for defensive behavior mode
    /// - Integrate health-driven decisions into combat states more deeply, e.g., "Take Cover" when health is low
    /// - Possibly rewrite?
    /// - Finish combat stage
    /// 
    /// </summary>
    public class EnemyLocomotionManager : CharacterLocomotionManager
    {
        #region Enums

        public enum TargetSelectionStrategy
        {
            Random,
            Closest,
            LowestHealth,
            LeastAttentive, //Player facing away?
            Strategic, //Target primary objective?
            MostDangerous, //Player dealing most damage?
            Support //Target supporting characters?
        }

        public enum AIState
        {
            Idle,
            Patrolling,
            Alert, //Enemy noticed player?
            Combat
        }

        public enum IdleState
        {
            Wait,
            RotateRandomly
        }

        public enum PatrolState
        {
            Wander,
            Patrol,
            PatrolReverse,
            PatrolRandom
        }

        public enum CombatState
        {
            Chasing,
            Strafing,
            Repositioning,
            Suppress,
            TakingCover,
            Retreating
        }

        [System.Serializable]
        public struct CombatStateProbability
        {
            public CombatState state;
            public float probability;
        }

        public enum BehaviorMode
        {
            Passive,
            Defensive,
            Aggressive,
            Stealth,
            Hunter,
            Guard,
            Berserker,
            Tactical
        }

        #endregion

        #region Variables

        [Header("Main Settings")]
        [SerializeField] private EnemyBehavior enemyBehavior;

        [Header("Detection Settings")]
        [SerializeField] BehaviorMode behaviorMode = BehaviorMode.Passive;
        private TargetSelectionStrategy selectionStrategy = TargetSelectionStrategy.Random;
        private Transform target;

        [Header("Patrol Settings")]
        [SerializeField] private PatrolState patrolState = PatrolState.Wander;
        private Transform waypointGroup;
        private List<Transform> patrolPoints = new List<Transform>();
        private int currentPatrolIndex;

        [Header("Combat Settings")]
        private CombatState combatState = CombatState.Chasing;
        private List<CombatStateProbability> combatProbabilities;

        [Header("Helper Variables")]
        private AIState currentState = AIState.Idle;
        private Transform modelTransform;
        private Vector2 startPosition;
        private Vector2 wanderTarget;
        private Vector3 lastRaycastDirection;
        private bool clearLineOfSight;

        #endregion

        #region Base Functions

        private void Start()
        {
            enemyBehavior = GetComponent<EnemyBehavior>();

            modelTransform = transform.Find("Model");
            waypointGroup = transform.Find("Waypoints");

            InitializeEnemy();

            startPosition = modelTransform.position;

            if(patrolState != PatrolState.Wander)
            {
                foreach (Transform child in waypointGroup)
                {
                    patrolPoints.Add(child);
                }
            }

            StartCoroutine(IdleBehaviorManager());
        }

        private void InitializeEnemy()
        {
            switch (behaviorMode)
            {
                case BehaviorMode.Passive:
                    combatProbabilities = new List<CombatStateProbability>
                    {
                        new CombatStateProbability { state = CombatState.Retreating, probability = 1f }
                    };
                    break;
                case BehaviorMode.Defensive:
                    combatProbabilities = new List<CombatStateProbability>
                    {
                        new CombatStateProbability { state = CombatState.Chasing, probability = 0.5f },
                        new CombatStateProbability { state = CombatState.Retreating, probability = 0.5f }
                    };
                    break;
                case BehaviorMode.Aggressive:
                    combatProbabilities = new List<CombatStateProbability>
                    {
                        new CombatStateProbability { state = CombatState.Chasing, probability = 0.2f },
                        new CombatStateProbability { state = CombatState.Strafing, probability = 0.2f },
                        new CombatStateProbability { state = CombatState.Repositioning, probability = 0.15f },
                        new CombatStateProbability { state = CombatState.Suppress, probability = 0.15f },
                        new CombatStateProbability { state = CombatState.TakingCover, probability = 0.15f },
                        new CombatStateProbability { state = CombatState.Retreating, probability = 0.15f }
                    };
                    break;
                case BehaviorMode.Stealth:
                    combatProbabilities = new List<CombatStateProbability>
                    {
                        new CombatStateProbability { state = CombatState.Chasing, probability = 0.1f },
                        new CombatStateProbability { state = CombatState.Strafing, probability = 0.1f },
                        new CombatStateProbability { state = CombatState.Repositioning, probability = 0.15f },
                        new CombatStateProbability { state = CombatState.Suppress, probability = 0.15f },
                        new CombatStateProbability { state = CombatState.TakingCover, probability = 0.4f },
                        new CombatStateProbability { state = CombatState.Retreating, probability = 0.1f }
                    };
                    break;
                case BehaviorMode.Hunter:
                    combatProbabilities = new List<CombatStateProbability>
                    {
                        new CombatStateProbability { state = CombatState.Chasing, probability = 0.4f },
                        new CombatStateProbability { state = CombatState.Strafing, probability = 0.2f },
                        new CombatStateProbability { state = CombatState.Repositioning, probability = 0.1f },
                        new CombatStateProbability { state = CombatState.Suppress, probability = 0.1f },
                        new CombatStateProbability { state = CombatState.TakingCover, probability = 0.1f },
                        new CombatStateProbability { state = CombatState.Retreating, probability = 0.1f }
                    };
                    break;
                case BehaviorMode.Guard:
                    combatProbabilities = new List<CombatStateProbability>
                    {
                        new CombatStateProbability { state = CombatState.Chasing, probability = 0.15f },
                        new CombatStateProbability { state = CombatState.Strafing, probability = 0.15f },
                        new CombatStateProbability { state = CombatState.Repositioning, probability = 0.1f },
                        new CombatStateProbability { state = CombatState.Suppress, probability = 0.3f },
                        new CombatStateProbability { state = CombatState.TakingCover, probability = 0.1f },
                        new CombatStateProbability { state = CombatState.Retreating, probability = 0.2f }
                    };
                    break;
                case BehaviorMode.Berserker:
                    combatProbabilities = new List<CombatStateProbability>
                    {
                        new CombatStateProbability { state = CombatState.Chasing, probability = 0.6f },
                        new CombatStateProbability { state = CombatState.Strafing, probability = 0.1f },
                        new CombatStateProbability { state = CombatState.Suppress, probability = 0.3f },
                    };
                    break;
                case BehaviorMode.Tactical:
                    combatProbabilities = new List<CombatStateProbability>
                    {
                        new CombatStateProbability { state = CombatState.Chasing, probability = 0.1f },
                        new CombatStateProbability { state = CombatState.Strafing, probability = 0.1f },
                        new CombatStateProbability { state = CombatState.Repositioning, probability = 0.25f },
                        new CombatStateProbability { state = CombatState.Suppress, probability = 0.2f },
                        new CombatStateProbability { state = CombatState.TakingCover, probability = 0.25f },
                        new CombatStateProbability { state = CombatState.Retreating, probability = 0.1f }
                    };
                    break;
            }

            if (behaviorMode != BehaviorMode.Passive && behaviorMode != BehaviorMode.Defensive)
            {
                StartCoroutine(FindNewTargetRoutine());
            }
        }

        private void ChangeState(AIState newState)
        {
            if (currentState == newState)
            {
                return;
            }
            
            switch (currentState)
            {
                case AIState.Idle:
                    StopCoroutine(IdleBehaviorManager());
                    break;
                case AIState.Patrolling:
                    StopCoroutine(PatrollingBehaviorManager());
                    break;
                case AIState.Combat:
                    StopCoroutine(CombatBehaviorManager());
                    break;
            }

            currentState = newState;

            Debug.Log(currentState);

            switch (newState)
            {
                case AIState.Idle:
                    StartCoroutine(IdleBehaviorManager());
                    break;
                case AIState.Patrolling:
                    StartCoroutine(PatrollingBehaviorManager());
                    break;
                case AIState.Combat:
                    StartCoroutine(CombatBehaviorManager());
                    break;
            }
        }

        #endregion

        #region Detection Functions

        private IEnumerator FindNewTargetRoutine()
        {
            while (true)
            {
                FindNewTarget();
                yield return new WaitForSeconds(Random.Range(enemyBehavior.findTargetInterval * 0.5f, enemyBehavior.findTargetInterval * 1.5f));
            }
        }

        private void FindNewTarget()
        {
            List<Transform> visibleTargets = GetVisibleTargets();
            if (visibleTargets.Count > 0)
            {
                switch (selectionStrategy)
                {
                    case TargetSelectionStrategy.Random:
                        target = visibleTargets[Random.Range(0, visibleTargets.Count)];
                        break;
                    case TargetSelectionStrategy.Closest:
                        target = GetClosestTarget(visibleTargets);
                        break;
                    case TargetSelectionStrategy.LowestHealth:
                        target = GetLowestHealthTarget(visibleTargets);
                        break;
                }
                ChangeState(AIState.Combat);
            }
            else
            {
                target = null;
                if(currentState != AIState.Patrolling)
                {
                    ChangeState(AIState.Idle);
                }
            }
        }

        private List<Transform> GetVisibleTargets()
        {
            List<Transform> visibleTargets = new List<Transform>();
            Collider2D[] hits = Physics2D.OverlapCircleAll(modelTransform.position, enemyBehavior.visibleRange);
            foreach (var hit in hits)
            {
                if (hit.transform.parent.CompareTag("Player") && IsTargetWithinLineOfSight(hit.transform))
                {
                    visibleTargets.Add(hit.transform);
                }
            }
            return visibleTargets;
        }

        private Transform GetClosestTarget(List<Transform> visibleTargets)
        {
            Transform closestTarget = null;
            float minDistance = float.MaxValue;
            foreach (var target in visibleTargets)
            {
                float distance = Vector3.Distance(modelTransform.position, target.position);
                if (distance < minDistance)
                {
                    closestTarget = target;
                    minDistance = distance;
                }
            }
            return closestTarget;
        }

        private Transform GetLowestHealthTarget(List<Transform> visibleTargets)
        {
            Transform lowestHealthTarget = null;
            float minHealth = float.MaxValue;
            foreach (var target in visibleTargets)
            {
                var player = target.GetComponent<CharacterManager>();
                if (player != null && player.currentHealth < minHealth)
                {
                    minHealth = player.currentHealth;
                    lowestHealthTarget = target;
                }
            }
            return lowestHealthTarget;
        }

        private bool IsTargetWithinLineOfSight(Transform potentialTarget)
        {
            Vector2 directionToTarget = (potentialTarget.position - modelTransform.position).normalized;
            float angleToTarget = Vector2.Angle(modelTransform.up, directionToTarget);
            if (angleToTarget > enemyBehavior.visibleAngle)
            {
                return false;
            }

            int enemyLayerMask = 1 << gameObject.layer;
            int layerMask = ~enemyLayerMask;

            RaycastHit2D[] hits = Physics2D.RaycastAll(modelTransform.position, directionToTarget, enemyBehavior.visibleRange, layerMask);
            foreach (var hit in hits)
            {
                if (hit.collider.transform == potentialTarget || hit.collider.transform.IsChildOf(potentialTarget))
                {
                    clearLineOfSight = true;
                    lastRaycastDirection = directionToTarget;
                    return true;
                }
                else
                {
                    clearLineOfSight = false;
                    lastRaycastDirection = directionToTarget;
                    return false;
                }
            }
            return false;
        }

        #endregion

        #region Idle Functions

        private IEnumerator IdleBehaviorManager()
        {
            float timeToSwitch = Random.Range(10f, 20f);
            float timeSpentIdle = 0f;

            while (currentState == AIState.Idle)
            {
                IdleState chosenAction = (IdleState)Random.Range(0, System.Enum.GetValues(typeof(IdleState)).Length);
                float actionDuration = Random.Range(enemyBehavior.idleInterval * 0.5f, enemyBehavior.idleInterval * 1.5f);

                switch (chosenAction)
                {
                    case IdleState.Wait:
                        yield return new WaitForSeconds(actionDuration);
                        break;
                    case IdleState.RotateRandomly:
                        yield return StartCoroutine(RotateRandomly(actionDuration));
                        break;
                }

                timeSpentIdle += actionDuration;
                if (timeSpentIdle >= timeToSwitch)
                {
                    ChangeState(AIState.Patrolling);
                }
            }
        }

        private IEnumerator RotateRandomly(float duration)
        {
            float randomAngle = Random.Range(0f, 360f);
            Quaternion targetRotation = Quaternion.Euler(0, 0, randomAngle);

            float timeElapsed = 0f;
            Quaternion startRotation = modelTransform.rotation;

            while (timeElapsed < duration)
            {
                if (currentState != AIState.Idle)
                {
                    yield break;
                }

                modelTransform.rotation = Quaternion.Lerp(startRotation, targetRotation, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            modelTransform.rotation = targetRotation;
        }

        #endregion

        #region Patrol Functions

        private IEnumerator PatrollingBehaviorManager()
        {
            while (currentState == AIState.Patrolling)
            {
                switch (patrolState)
                {
                    case PatrolState.Patrol:
                        yield return StartCoroutine(Patrol());
                        break;
                    case PatrolState.PatrolReverse:
                        yield return StartCoroutine(PatrolReverse());
                        break;
                    case PatrolState.PatrolRandom:
                        yield return StartCoroutine(PatrolRandom());
                        break;
                    case PatrolState.Wander:
                        yield return StartCoroutine(Wander());
                        break;
                }
            }
        }

        private IEnumerator Patrol()
        {
            Vector2 targetPosition = patrolPoints[currentPatrolIndex].position;
            while (Vector2.Distance(modelTransform.position, targetPosition) > 0.1f)
            {
                if (currentState != AIState.Patrolling)
                {
                    yield break;
                }

                MoveToTarget(targetPosition);
                yield return null;
            }

            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;

            yield return new WaitForSeconds(Random.Range(enemyBehavior.patrolInterval * 0.5f, enemyBehavior.patrolInterval * 1.5f));
        }

        private IEnumerator PatrolReverse()
        {
            Vector2 targetPosition = patrolPoints[currentPatrolIndex].position;
            while (Vector2.Distance(modelTransform.position, targetPosition) > 0.1f)
            {
                if (currentState != AIState.Patrolling)
                {
                    yield break;
                }

                MoveToTarget(targetPosition);
                yield return null;
            }

            currentPatrolIndex = (currentPatrolIndex - 1 + patrolPoints.Count) % patrolPoints.Count;

            yield return new WaitForSeconds(Random.Range(enemyBehavior.patrolInterval * 0.5f, enemyBehavior.patrolInterval * 1.5f));
        }

        private IEnumerator PatrolRandom()
        {
            Transform randomTarget = patrolPoints[Random.Range(0, patrolPoints.Count)];
            Vector2 targetPosition = randomTarget.position;
            while (Vector2.Distance(modelTransform.position, targetPosition) > 0.1f)
            {
                if (currentState != AIState.Patrolling)
                {
                    yield break;
                }

                MoveToTarget(targetPosition);
                yield return null;
            }

            yield return new WaitForSeconds(Random.Range(enemyBehavior.patrolInterval * 0.5f, enemyBehavior.patrolInterval * 1.5f));
        }

        private IEnumerator Wander()
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            wanderTarget = startPosition + randomDirection * Random.Range(2f, enemyBehavior.wanderRadius);

            while (Vector2.Distance(modelTransform.position, wanderTarget) > 1f)
            {
                if (currentState != AIState.Patrolling)
                {
                    yield break;
                }

                MoveToTarget(wanderTarget);
                yield return null;
            }

            yield return new WaitForSeconds(Random.Range(enemyBehavior.patrolInterval * 0.5f, enemyBehavior.patrolInterval * 1.5f));
        }

        private void MoveToTarget(Vector2 targetPosition)
        {
            Vector2 directionToTarget = targetPosition - (Vector2)modelTransform.position;
            float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            modelTransform.rotation = Quaternion.Lerp(modelTransform.rotation, targetRotation, Time.deltaTime * enemyBehavior.rotationSpeed);

            float angleDifference = Quaternion.Angle(modelTransform.rotation, targetRotation);
            float speedFactor;
            if (angleDifference > 180)
            {
                speedFactor = 0;
            }
            else if (angleDifference > 90)
            {
                speedFactor = 0.25f;
            }
            else if (angleDifference > 30)
            {
                speedFactor = 0.5f;
            }
            else
            {
                speedFactor = 1;
            }

            Vector2 moveDirection = modelTransform.up * enemyBehavior.accelerationSpeed * speedFactor * Time.deltaTime;
            velocity = Vector2.ClampMagnitude(moveDirection, enemyBehavior.maxSpeed * speedFactor);
            modelTransform.position += (Vector3)velocity;
        }

        #endregion

        #region Combat Functions

        private IEnumerator CombatBehaviorManager()
        {
            while (currentState == AIState.Combat)
            {
                if (target == null)
                {
                    if(behaviorMode == BehaviorMode.Defensive)
                    {
                        ChangeState(AIState.Idle);
                        yield break;
                    }
                    else
                    {
                        FindNewTarget();
                    }
                }

                SelectCombatState();

                switch (combatState)
                {
                    case CombatState.Chasing:
                        yield return StartCoroutine(ChasingBehavior());
                        break;
                    case CombatState.Strafing:
                        yield return StartCoroutine(StrafingBehavior());
                        break;
                    case CombatState.Repositioning:
                        yield return StartCoroutine(RepositioningBehavior());
                        break;
                    case CombatState.Suppress:
                        yield return StartCoroutine(SuppressBehavior());
                        break;
                    case CombatState.TakingCover:
                        yield return StartCoroutine(TakingCoverBehavior());
                        break;
                    case CombatState.Retreating:
                        yield return StartCoroutine(RetreatingBehavior());
                        break;
                }

                yield return new WaitForSeconds(enemyBehavior.combatInterval);
            }
        }

        private void SelectCombatState()
        {
            float randomValue = Random.Range(0.0f, 1.0f);
            float cumulativeProbability = 0.0f;

            foreach (var combatStateProbability in combatProbabilities)
            {
                cumulativeProbability += combatStateProbability.probability;
                if (randomValue <= cumulativeProbability)
                {
                    combatState = combatStateProbability.state;
                    break;
                }
            }

            /*if (currenthealth / maxhealth <= retreatThreshold)
            {
                combatState = CombatState.Retreating;
            }*/
        }

        private IEnumerator ChasingBehavior()
        {
            Debug.Log("Chasing target...");
            yield return null;
        }

        private IEnumerator StrafingBehavior()
        {
            Debug.Log("Strafing around target...");
            yield return null;
        }

        private IEnumerator RepositioningBehavior()
        {
            Debug.Log("Repositioning...");
            yield return null;
        }

        private IEnumerator SuppressBehavior()
        {
            Debug.Log("Suppressing fire...");
            yield return null;
        }

        private IEnumerator TakingCoverBehavior()
        {
            Debug.Log("Taking cover...");
            yield return null;
        }

        private IEnumerator RetreatingBehavior()
        {
            Debug.Log("Retreating...");
            yield return null;
        }

        #endregion

        #region Gizmos

        private void OnDrawGizmos()
        {
            if (!enabled || enemyBehavior == null)
            {
                return;
            }

            DrawLineOfSight();
            DrawLineToTarget();
            DrawWaypoints();
        }

        private void DrawGizmoArc(Vector3 center, Quaternion rotation, float angle, float radius)
        {
            int stepCount = 10;
            float stepAngleSize = angle * 2 / stepCount;

            for (int i = 0; i <= stepCount; i++)
            {
                float currentAngle = stepAngleSize * i - angle;
                Vector3 direction = Quaternion.Euler(0, 0, currentAngle) * rotation * Vector3.up;
                Vector3 previousDirection = Quaternion.Euler(0, 0, currentAngle - stepAngleSize) * rotation * Vector3.up;

                if (i > 0)
                {
                    Gizmos.DrawLine(center + previousDirection * radius, center + direction * radius);
                }
            }
        }

        private void DrawLineOfSight()
        {
            if(modelTransform == null)
            {
                return;
            }

            Gizmos.color = Color.green;

            Vector3 forward = modelTransform.up * enemyBehavior.visibleRange;
            Vector3 leftBoundary = Quaternion.Euler(0, 0, enemyBehavior.visibleAngle) * forward;
            Vector3 rightBoundary = Quaternion.Euler(0, 0, -enemyBehavior.visibleAngle) * forward;

            Gizmos.DrawLine(modelTransform.position, modelTransform.position + leftBoundary);
            Gizmos.DrawLine(modelTransform.position, modelTransform.position + rightBoundary);

            DrawGizmoArc(modelTransform.position, modelTransform.rotation, enemyBehavior.visibleAngle, enemyBehavior.visibleRange);
        }

        private void DrawLineToTarget()
        {
            if(modelTransform == null)
            {
                return;
            }

            if (clearLineOfSight)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawLine(modelTransform.position, modelTransform.position + lastRaycastDirection * enemyBehavior.visibleRange);
        }

        private void DrawWaypoints()
        {
            if (patrolState == PatrolState.Wander)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(startPosition, enemyBehavior.wanderRadius);
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(wanderTarget, 0.5f);
            }

            if (patrolPoints != null)
            {
                Gizmos.color = Color.blue;
                for (int i = 0; i < patrolPoints.Count; i++)
                {
                    Vector3 nextPoint = patrolPoints[(i + 1) % patrolPoints.Count].position;
                    Gizmos.DrawLine(patrolPoints[i].position, nextPoint);
                    Gizmos.DrawSphere(patrolPoints[i].position, 0.3f);
                }
            }
        }

        #endregion
    }
}