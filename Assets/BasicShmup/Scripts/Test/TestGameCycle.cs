using System.Collections;
using UnityEngine;
using ShmupCore.GameElement;
using ShmupCore.Factory;

namespace ShmupCore.Test
{
    public class TestGameCycle : MonoBehaviour
    {
        [SerializeField]
        private PlayableObject m_Player = null;
        private Vector2 reBirthPos = Vector2.zero;
        [SerializeField]
        private EnemySpawner m_EnemySpawner = null;
        [SerializeField]
        private Enemy m_SetEnemy = null;
        [SerializeField]
        private float m_EnemySpawnCoolDown = 1;
        private Coroutine m_EnemySpawnRoutine = null;
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
            WaitForSeconds waitForSeconds = new WaitForSeconds(m_EnemySpawnCoolDown);
            while (true)
            {
                int laneTopNumber = m_EnemySpawner.LanePairCount;
                int laneIndex = Random.Range(-laneTopNumber, laneTopNumber + 1);
                Enemy enemy = m_EnemySpawner.SpawnEnemy(m_SetEnemy, laneIndex);
                enemy.m_MoveDirection = Vector2.left;
                yield return waitForSeconds;
            }
        }
    }
}
