using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Events;

namespace ShmupCore
{
    public class EnemyFactory
    {
        private static List<EnemyPool> enemyPools = new List<EnemyPool>();

        /// <summary>
        /// If want know enemy class get by some script, can subscribe this event to get enemy class. 
        /// That will trigger when some script call Function-EnemyFactory.GetEnemy.
        /// </summary>
        public static UnityAction<Enemy> onEnemyGet = null;

        /// <summary>
        /// If want know enemy class when return to pool, can subscribe this event to get enemy class. 
        /// </summary>
        public static UnityAction<Enemy> onEnemyReturnToPool = null;

        public static Enemy GetEnemy(Enemy _CoreEnemy)
        {
            //  Search in list
            int poolCount = enemyPools.Count;
            for (int index = 0; index < poolCount; ++index)
            {
                var enemyPool = enemyPools[index];
                var checkEnemy = enemyPool.CoreEnemy;
                if (checkEnemy == _CoreEnemy)
                {
                    return SetOnEnemyGetInvoke(enemyPool.GetEnemy());
                }
            }

            //  If no enemy pool in list, create one and take
            var newEnemyPool = new EnemyPool(_CoreEnemy);
            enemyPools.Add(newEnemyPool);
            return SetOnEnemyGetInvoke(newEnemyPool.GetEnemy());

            Enemy SetOnEnemyGetInvoke(Enemy enemy)
            {
                if (onEnemyGet != null)
                {
                    onEnemyGet.Invoke(enemy);
                }
                return enemy;
            }
        }

        public static void ReleaseAll()
        {
            int poolsCount = enemyPools.Count;
            for (int index = 0; index < poolsCount; ++index)
            {
                EnemyPool enemyPool = enemyPools[index];
                enemyPool.ReleaseAll();
            }
        }

        public static void DisposeAll()
        {
            int poolsCount = enemyPools.Count;
            for (int index = 0; index < poolsCount; ++index)
            {
                EnemyPool enemyPool= enemyPools[index];
                enemyPool.Dispose();
            }
        }
    }

    public class EnemyPool
    {
        private Enemy m_CoreEnemy = null;
        public Enemy CoreEnemy
        {
            get { return m_CoreEnemy; }
        }

        private ObjectPool<Enemy> enemyPool = null;
        private List<Enemy> aliveObject = new List<Enemy>();

        public EnemyPool(Enemy _CoreEnemy)
        {
            m_CoreEnemy = _CoreEnemy;
            enemyPool = new ObjectPool<Enemy>(CreatePoolItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);
        }

        public void Dispose()
        {
            enemyPool.Dispose();
        }

        public Enemy GetEnemy()
        {
            var enemy = enemyPool.Get();
            return enemy;
        }

        public void ReleaseAll()
        {
            int aliveEnemyCount = aliveObject.Count;
            for (int index = 0; index < aliveEnemyCount; index++)
            {
                enemyPool.Release(aliveObject[0]);
            }
            aliveObject.Clear();
        }

        private Enemy CreatePoolItem()
        {
            var newEnemy = GameObject.Instantiate<Enemy>(m_CoreEnemy);
            newEnemy.eventWhenEnemyDead = () =>
            {
                enemyPool.Release(newEnemy);
            };
            return newEnemy;
        }

        private void OnReturnedToPool(Enemy enemy)
        {
            enemy.Recycle();
            enemy.gameObject.SetActive(false);
            aliveObject.Remove(enemy);
            if (EnemyFactory.onEnemyReturnToPool != null)
            {
                EnemyFactory.onEnemyReturnToPool.Invoke(enemy);
            }
        }

        private void OnTakeFromPool(Enemy enemy)
        {
            enemy.gameObject.SetActive(true);
            enemy.ReSetData();
            aliveObject.Add(enemy);
        }

        private void OnDestroyPoolObject(Enemy enemy)
        {
            GameObject.Destroy(enemy.gameObject);
        }
    }
}
