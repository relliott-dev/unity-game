using System.Collections;
using UnityEngine;

namespace RDE
{
    public class EnemyCombatManager : CharacterCombatManager
    {
        private EnemyManager enemyManager;

        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
        }

        public override void PerformAttack()
        {
            base.PerformAttack();

            if (weaponConfig == null || enemyManager.isDead || !CanAttack() || enemyManager.currentEnergy < weaponConfig.energyCost)
            {
                return;
            }

            StartCoroutine(ResetShootingState(0.5f));
        }

        private IEnumerator ResetShootingState(float delay)
        {
            enemyManager.isShooting = true;
            yield return new WaitForSeconds(delay);
            enemyManager.isShooting = false;
        }
    }
}