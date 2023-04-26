using System.Collections;
using UnityEngine;
using ShmupCore.Level;

namespace ShmupCore.Test
{
    public class TestGameLeve : MonoBehaviour
    {
        [SerializeField]
        private PlayableObject m_Player = null;
        private Vector2 reBirthPos = Vector2.zero;
        [SerializeField]
        private EnemyLibrary m_EnemyLibrary = null;
        [SerializeField]
        private LevelData m_LevelData = null;
        [SerializeField]
        private EnemySpawner m_EnemySpawner = null;
        private Coroutine m_EnemySpawnRoutine = null;
        private float gameTime = 0;
        [SerializeField]
        private GameObject m_GameOverUI = null;

        void Start()
        {
            m_Player.EventWhenDead = PlayerDead;
            reBirthPos = m_Player.transform.localPosition;
            m_EnemySpawnRoutine = StartCoroutine(EnemySpawning());
            m_GameOverUI.SetActive(false);
        }

        public void ReStart()
        {
            m_Player.ResetState();
            m_Player.transform.localPosition = reBirthPos;
            m_EnemySpawnRoutine = StartCoroutine(EnemySpawning());
            m_GameOverUI.SetActive(false);
            gameTime = 0;
        }

        private void PlayerDead()
        {
            StopCoroutine(m_EnemySpawnRoutine);
            EnemyFactory.ReleaseAll();
            BulletFactory.ReleaseAll();
            m_GameOverUI.SetActive(true);
        }

        IEnumerator EnemySpawning()
        {
            var spawnDatas = m_LevelData.SpawnDatas;
            int spawnIndex = 0;
            
            var targetSpawnData = spawnDatas[spawnIndex];
            while (true)
            {
                yield return null;
                gameTime += Time.deltaTime;
                if (gameTime >= targetSpawnData.SpawnTiming)
                {
                    var enemyPrefab = m_EnemyLibrary.GetEnemyPrefab(targetSpawnData.ID);
                    if (enemyPrefab != null)
                    {
                        var enemy = m_EnemySpawner.SpawnEnemy(enemyPrefab, targetSpawnData.LaneNumber);
                        enemy.m_MoveDirection = Vector2.left;
                    }
                    else
                    {
                        Debug.LogError("Enemy prefab not find!ID are " + targetSpawnData.ID);
                    }
                    spawnIndex++;
                    if (spawnIndex >= spawnDatas.Length)
                    {
                        break;
                    } else
                    {
                        targetSpawnData = spawnDatas[spawnIndex];
                    }
                }
            }
        }
    }
}
