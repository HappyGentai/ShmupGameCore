using UnityEngine;
using GrazerCore.GameElements;
using GrazerCore.GameElements.PlayerPlus;
using GrazerCore.GameElements.EnemyGroup;
using GrazerCore.Factories;
using GrazerCore.TriggerEvents;
using System.Collections;
using UnityEngine.InputSystem;

namespace GrazerCore.Test
{
    public class TestGameCore : MonoBehaviour
    {
        [SerializeField]
        private float m_NowGameTime = 0;
        [SerializeField]
        private int m_GameFPS = 60;
        [SerializeField]
        private BasicPlayer m_Player = null;
        [SerializeField]
        private Vector2 m_BirthPoint = Vector2.zero;
        [SerializeField]
        private TestPlayerUI m_PlayerUI = null;

        [SerializeField]
        private EnemyTeamData[] m_EnemyTeamDatas = null;
        [SerializeField]
        private int waveIndex = 0;
        private int enemyTeamCount = 0;
        private EnemyTeam currentEnemyTeam = null;
        private EnemyTeam lastEnemyTeam = null;
        private Coroutine waveCoroutine = null;

        [SerializeField]
        private StageBackGround m_BackGround = null;
        //[SerializeField]
        //private DamagedHint m_DamagedHint = null;

        [SerializeField]
        private InputActionReference m_CloseActionRef = null;
        protected InputAction m_CloseAction;

        [Header("Player plug-in")]
        [SerializeField]
        private DamageResponesType m_DamageResponesType = DamageResponesType.ClearBullet;
        private DamageResponse currentDamageResponse = null;
        [SerializeField]
        private DamageResponseTypeClear m_DamageResponseTypeClear = null;
        [SerializeField]
        private DamageResponseTypeProtect DamageResponseTypeProtect = null;

        void Start()
        {
            Application.targetFrameRate = m_GameFPS;
            m_Player.Initialization();
            m_Player.OnPlayerDie.AddListener(GameOver);
            //m_Player.OnHitBoxCollision.AddListener(m_DamagedHint.StartDamagedHint);
            StartGame();
            m_CloseAction = m_CloseActionRef.action;
            m_CloseAction.Enable();
            m_CloseAction.performed += (ctx) => {
                Application.Quit();
            };
        }

        protected void StartGame()
        {
            ClearOldGameStage();
            //  Set player damaged plug-in
            switch (m_DamageResponesType)
            {
                case DamageResponesType.ClearBullet:
                    currentDamageResponse = m_DamageResponseTypeClear;
                    m_DamageResponseTypeClear.Install(m_Player);
                    break;
                case DamageResponesType.ProtectHitBox:
                    currentDamageResponse = DamageResponseTypeProtect;
                    DamageResponseTypeProtect.Install(m_Player);
                    break;
            }

            enemyTeamCount = m_EnemyTeamDatas.Length;

            m_Player.WakeUpObject();
            //  ReSet UI
            if (!m_PlayerUI.IsInitialization)
            {
                m_PlayerUI.Initialization();
                m_PlayerUI.OnReStart.AddListener(StartGame);
            }
            m_PlayerUI.StartUP();

            //  StartGame
            m_BackGround.MoveBackGround();
            m_Player.MoveTarget.localPosition = m_BirthPoint;
            waveIndex = 0;
            CallWave();
        }

        private void Update()
        {
            m_NowGameTime = Time.time;
        }

        private void CallWave()
        {
            var teamCount = m_EnemyTeamDatas.Length;
            if (waveIndex >= teamCount)
            {
                return;
            }
            var teamData = m_EnemyTeamDatas[waveIndex];
            waveCoroutine = StartCoroutine(WaveCalling(teamData));
            waveIndex++;
        }

        private void CloseCallWave()
        {
            if (waveCoroutine != null)
            {
                StopCoroutine(waveCoroutine);
            }
        }

        private void GameOver()
        {
            CloseCallWave();
            //  Stop all alive enemys
            var aliveEnemys = EnemyFactory.GetAliveEnemys();
            var enemyCount = aliveEnemys.Count;
            for (int index = 0; index < enemyCount; ++index)
            {
                var enemy = aliveEnemys[index];
                enemy.SleepObject();
            }
            //  Show game over UI
            m_PlayerUI.GameOver();
        }

        private void GameClearCheck(EnemyTeam clearedTeam)
        {
            enemyTeamCount--;
            if (enemyTeamCount <= 0)
            {
                CloseCallWave();
                //  Stop player move
                m_Player.SleepObject();
                m_Player.Invincible = true;
                //  Call game clear UI
                m_PlayerUI.GameClear();
            }
            else if (lastEnemyTeam == clearedTeam)
            {
                CloseCallWave();
                currentEnemyTeam.SummonMember();
                CallWave();
            }
        }

        private void ClearOldGameStage()
        {
            //  Uninstall old damage response
            if (currentDamageResponse != null)
            {
                currentDamageResponse.UnInstall();
            }
            //  Recycle all old bullet 
            BulletFactory.ReleaseAll();
            //  Recycle all old enemy
            EnemyFactory.ReleaseAll();
            //  Recycle all old team
            EnemyTeamFactory.ReleaseAll();
        }

        IEnumerator WaveCalling(EnemyTeamData teamData)
        {
            var team = EnemyTeamFactory.GetEnemyTeam(teamData);
            team.OnAllMemberGone.AddListener(GameClearCheck);
            lastEnemyTeam = currentEnemyTeam;
            currentEnemyTeam = team;
            yield return new WaitForSeconds(teamData.WaveWaitTime);
            currentEnemyTeam.SummonMember();
            CallWave();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(m_BirthPoint, 0.5f);
        }
    }
}
