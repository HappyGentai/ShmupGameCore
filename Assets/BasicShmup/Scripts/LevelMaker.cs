using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShmupCore.Level
{
    public class LevelMaker : MonoBehaviour
    {
        [SerializeField]
        private EnemyLibrary m_EnemyLibrary = null;
        [SerializeField]
        private EnemySpawner m_EnemySpawner = null;

        [SerializeField]
        private List<EnemySpawnData> m_EnemySpawnDatas = new List<EnemySpawnData>();
        public List<EnemySpawnData> EnemySpawnDatas
        {
            get { return m_EnemySpawnDatas; }
            set { m_EnemySpawnDatas = value; }
        }

        private float gameTime = 0;
        public float GameTime
        {
            get { return gameTime; }
        }
        private Coroutine gameCoroutine = null;

        public void PlayLevel()
        {
            Time.timeScale = 1;
            if (gameCoroutine == null)
            {
                gameCoroutine = StartCoroutine(GamePlaying());
            }
        }

        public void StopLevel()
        {
            Time.timeScale = 0;
        }

        public void ReSetLevel()
        {
            gameTime = 0;
            if (gameCoroutine != null)
            {
                StopCoroutine(gameCoroutine);
            }
            gameCoroutine = null;
            EnemyFactory.ReleaseAll();
        }

        public void SetEnemy(string id, int laneNo)
        {
            var enemyPrefab = m_EnemyLibrary.GetEnemyPrefab(id);
            if (enemyPrefab != null)
            {
                var enemy = m_EnemySpawner.SpawnEnemy(enemyPrefab, laneNo);
                enemy.m_MoveDirection = Vector2.left;
            }
        }

        private IEnumerator GamePlaying()
        {
            int setEnemyIndex = 0;
            EnemySpawnData enemySpawnData = null;
            if (setEnemyIndex < m_EnemySpawnDatas.Count)
            {
                enemySpawnData = m_EnemySpawnDatas[setEnemyIndex];
            }
                
            while (true)
            {
                yield return null;
                gameTime += Time.deltaTime;
                if (Time.timeScale != 0 && enemySpawnData != null)
                {
                    if (gameTime >= enemySpawnData.SetTiming)
                    {
                        //  Set enemy
                        var enemyPrefab = m_EnemyLibrary.GetEnemyPrefab(enemySpawnData.EnemyID);
                        if (enemyPrefab != null)
                        {
                            var enemy = m_EnemySpawner.SpawnEnemy(enemyPrefab, enemySpawnData.SetLaneNumber);
                            setEnemyIndex++;
                            if (setEnemyIndex >= m_EnemySpawnDatas.Count)
                            {
                                enemySpawnData = null;
                            }
                            else
                            {
                                enemySpawnData = m_EnemySpawnDatas[setEnemyIndex];
                            }
                        }
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class EnemySpawnData
    {
        [SerializeField]
        private string m_EnemyID = "";
        public string EnemyID
        {
            get { return m_EnemyID; }
        }

        [SerializeField]
        private int m_SetLaneNumber = 0;
        public int SetLaneNumber
        {
            get { return m_SetLaneNumber; }
        }

        [SerializeField]
        private float m_SetTiming = 0;
        public float SetTiming
        {
            get { return m_SetTiming; }
        }

        public EnemySpawnData(string enemyID, int laneNo, float timing)
        {
            m_EnemyID = enemyID;
            m_SetLaneNumber = laneNo;
            m_SetTiming = timing;
        }
    }

    [System.Serializable]
    public class EnemySpawnDatas
    {
        [SerializeField]
        private List<EnemySpawnData> enemySpawnDatas = null;
        public List<EnemySpawnData> EnemySpawnData
        {
            get { return enemySpawnDatas; }
        }

        public EnemySpawnDatas(List<EnemySpawnData> datas)
        {
            enemySpawnDatas = datas;
        }
    }
}
