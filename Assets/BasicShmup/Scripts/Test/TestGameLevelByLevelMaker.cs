using System.Collections;
using UnityEngine;
using ShmupCore.Level;

namespace ShmupCore.Test
{
    public class TestGameLevelByLevelMaker : MonoBehaviour
    {
        [SerializeField]
        private PlayableObject m_Player = null;
        private Vector2 reBirthPos = Vector2.zero;
        [SerializeField]
        private EnemyLibrary m_EnemyLibrary = null;
        [SerializeField]
        private TextAsset m_LevelData = null;
        private EnemySpawnDatas levelData = null;
        [SerializeField]
        private EnemySpawner m_EnemySpawner = null;
        private Coroutine m_EnemySpawnRoutine = null;
        private float gameTime = 0;
        [SerializeField]
        private GameObject m_GameOverUI = null;
        private ShmupObserver shmupObserver = null;

        void Start()
        {
            m_Player.EventWhenDead = PlayerDead;
            reBirthPos = m_Player.transform.localPosition;
            levelData = JsonUtility.FromJson<EnemySpawnDatas>(m_LevelData.text);
            m_EnemySpawnRoutine = StartCoroutine(EnemySpawning());
            m_GameOverUI.SetActive(false);

            //  Create ShmupObserver
            shmupObserver = new ShmupObserver(m_Player);
            EnemyFactory.onEnemyGet = shmupObserver.OnEnemyEnemyCreate;
            EnemyFactory.onEnemyReturnToPool = shmupObserver.OnEnemyEnemyDie;
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
            if (levelData == null)
            {
                Debug.LogError("Level data are null");
            } else
            {
                var spawnDatas = levelData.EnemySpawnData;
                int spawnIndex = 0;

                var targetSpawnData = spawnDatas[spawnIndex];
                while (true)
                {
                    yield return null;
                    gameTime += Time.deltaTime;
                    if (gameTime >= targetSpawnData.SetTiming)
                    {
                        var enemyPrefab = m_EnemyLibrary.GetEnemyPrefab(targetSpawnData.EnemyID);
                        if (enemyPrefab != null)
                        {
                            var enemy = m_EnemySpawner.SpawnEnemy(enemyPrefab, targetSpawnData.SetLaneNumber);
                            enemy.m_MoveDirection = Vector2.left;
                        }
                        else
                        {
                            Debug.LogError("Enemy prefab not find!ID are " + targetSpawnData.EnemyID);
                        }
                        spawnIndex++;
                        if (spawnIndex >= spawnDatas.Count)
                        {
                            break;
                        }
                        else
                        {
                            targetSpawnData = spawnDatas[spawnIndex];
                        }
                    }
                }
            }
        }
    }
}
