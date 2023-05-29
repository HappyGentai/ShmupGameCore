using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using SkateGuy.GameElements.EnemyGroup;

namespace SkateGuy.GameElements.Factory
{
    public class EnemyTeamFactory
    {
        private static Dictionary<EnemyTeamData, ObjectPool<EnemyTeam>> enemyTeamPools 
            = new Dictionary<EnemyTeamData, ObjectPool<EnemyTeam>>();

        public static EnemyTeam GetEnemyTeam(EnemyTeamData teamDataPrefab)
        {
            return CheckPoolExist(teamDataPrefab);
        }

        private static EnemyTeam CheckPoolExist(EnemyTeamData teamDataPrefab)
        {
            if (enemyTeamPools.ContainsKey(teamDataPrefab))
            {
                var targetPool = enemyTeamPools[teamDataPrefab];
                return targetPool.Get();
            } else
            {
                //  Not  exist, create one
                var newPool = new ObjectPool<EnemyTeam>(CreatePoolItem, OnTakeFormPool,
                    OnReturnToPool, OnDestroyPoolObject, false);
                enemyTeamPools.Add(teamDataPrefab, newPool);
                return newPool.Get();
            }

            #region About pool event
            EnemyTeam CreatePoolItem()
            {
                var newEnemyTeam = GameObject.Instantiate<EnemyTeam>(teamDataPrefab.EnemyTeam);
                return newEnemyTeam;
            }

            void OnTakeFormPool(EnemyTeam enemyTeam)
            {
                enemyTeam.gameObject.SetActive(true);
            }

            void OnReturnToPool(EnemyTeam enemyTeam)
            {
                enemyTeam.gameObject.SetActive(false);
            }

            void OnDestroyPoolObject(EnemyTeam enemyTeam)
            {
                GameObject.Destroy(enemyTeam.gameObject);
            }
            #endregion
        }
    }
}
