using UnityEngine;
using UnityEngine.Pool;

namespace STG
{
    public class EnemySpawner : MonoBehaviour
    {
        /*
         *  參考資料:
         *  https://docs.unity3d.com/ScriptReference/Pool.ObjectPool_1-ctor.html
         */

        [Header("ObjectPool")]
        [SerializeField]
        private Vector2 m_StartPos = Vector2.zero;
        [SerializeField]
        private Enemy m_EnemyPrefab = null;
        [SerializeField]
        private bool m_CollectiWonChecks = true;
        [SerializeField]
        private int m_DefaultCapacity = 10;
        [SerializeField]
        private int m_MaxSize = 50;
        private ObjectPool<Enemy> enemyPools = null;

        private void Start()
        {
            enemyPools = new ObjectPool<Enemy>(CreateEnemy, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, m_CollectiWonChecks, m_DefaultCapacity, m_MaxSize);
        }

        Enemy CreateEnemy()
        {
            Enemy enemy = Instantiate(m_EnemyPrefab);
            BasicEnemyData basicEnemyData = new BasicEnemyData(5);
            enemy.SetData(basicEnemyData);
            enemy.eventWhenDie = () => {
                Debug.Log("Release");
                enemyPools.Release(enemy);
            };
            return enemy;
        }

        void OnTakeFromPool(Enemy enemy)
        {
            Debug.Log("Take");
            enemy.PickFromPool();
            enemy.transform.position = m_StartPos;
            enemy.gameObject.SetActive(true);
        }

        void OnReturnedToPool(Enemy enemy)
        {
            enemy.ReleaseToPool();
            enemy.gameObject.SetActive(false);
        }

        void OnDestroyPoolObject(Enemy enemy)
        {
            Destroy(enemy.gameObject);
        }

        void OnGUI()
        {
            GUILayout.Label("Pool size: " + enemyPools.CountInactive);
            if (GUILayout.Button("Create Particles"))
            {
                for (int i = 0; i < 3; ++i)
                {
                    var ps = enemyPools.Get();
                    ps.transform.position += Random.insideUnitSphere * 2;
                }
            }
        }
    }
}
