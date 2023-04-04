using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace STG
{
    public class EnemyFactory : MonoBehaviour
    {
        [SerializeField]
        private EnemyStorageData[] m_EnemyStorageDatas = null;
        [SerializeField]
        private bool m_CollectiWonChecks = true;
        [SerializeField]
        private int m_DefaultCapacity = 10;
        [SerializeField]
        private int m_MaxSize = 50;

        private Dictionary<string, ObjectPool<BasicEnemy>> enemyPools = new Dictionary<string, ObjectPool<BasicEnemy>>();

        public void Start()
        {
            
        }

        private void initialization()
        {
            int dataCount = m_EnemyStorageDatas.Length;
            for (int index = 0; index < dataCount; ++index)
            {
                EnemyStorageData data = m_EnemyStorageDatas[index];
                string id = data.ID;
                GameObject gameObject = data.EnemyPrefab;
                BasicEnemy basicEnemy = gameObject.GetComponent<BasicEnemy>();
                if (basicEnemy != null)
                {
                    //  Create pool
                    ObjectPool<BasicEnemy> enemyPool = null;
                    enemyPools.Add(id, enemyPool);
                    enemyPool = new ObjectPool<BasicEnemy>(createEnemy, OnTakeFromPool, OnReturnedToPool,
                        OnDestroyPoolObject, m_CollectiWonChecks, m_DefaultCapacity, m_MaxSize);

                    #region Pool callback function
                    BasicEnemy createEnemy()
                    {
                        BasicEnemy _basicEnemy = Instantiate(basicEnemy);
                        _basicEnemy.eventWhenDie = () => {
                            Debug.Log("Release");
                            enemyPool.Release(_basicEnemy);
                        };
                        return _basicEnemy;
                    }

                    void OnTakeFromPool(BasicEnemy enemy)
                    {
                        enemy.PickFromPool();
                        enemy.gameObject.SetActive(true);
                    }

                    void OnReturnedToPool(BasicEnemy enemy)
                    {
                        enemy.ReleaseToPool();
                        enemy.gameObject.SetActive(false);
                    }

                    void OnDestroyPoolObject(BasicEnemy enemy)
                    {
                        Destroy(enemy.gameObject);
                    }
                    #endregion
                }
                else
                {
                    Debug.Log("Game prefab no have BasicEnemy component, id are "+id);
                }
            }
        }

        private BasicEnemy GetEnemy(string enemyID)
        {
            BasicEnemy basicEnemy = null;
            if (enemyPools.ContainsKey(enemyID)) {
                ObjectPool<BasicEnemy> enemyPool = enemyPools[enemyID];
                basicEnemy = enemyPool.Get();
            }
            return basicEnemy;
        }
    }
}
