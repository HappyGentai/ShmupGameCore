using UnityEngine;
using SkateGuy.GameElements;
using SkateGuy.GameElements.PlayerPlus;
using SkateGuy.GameElements.EnemyGroup;
using SkateGuy.GameElements.Factory;
using System.Collections;

namespace SkateGuy.Test
{
    public class TestGameCore : MonoBehaviour
    {
        [SerializeField]
        private float m_NowGameTime = 0;
        [SerializeField]
        private PlayableObject m_Player = null;
        [SerializeField]
        private TestPlayerUI m_PlayerUI = null;

        [SerializeField]
        private EnemyTeamData[] m_EnemyTeamDatas = null;
        private int waveIndex = 0;
        private Coroutine waveCoroutine = null;

        [Header("Player plug-in")]
        [SerializeField]
        private DamageResponesType m_DamageResponesType = DamageResponesType.ClearBullet;
        [SerializeField]
        private DamageResponseTypeClear m_DamageResponseTypeClear = null;
        [SerializeField]
        private DamageResponseTypeProtect DamageResponseTypeProtect = null;

        void Start()
        {
            m_Player.Initialization();
            m_PlayerUI.Initialization();

            //  Set player damaged plug-in
            switch(m_DamageResponesType)
            {
                case DamageResponesType.ClearBullet:
                    m_DamageResponseTypeClear.Install(m_Player);
                    break;
                case DamageResponesType.ProtectHitBox:
                    DamageResponseTypeProtect.Install(m_Player);
                    break;
            }

            //  StartGame
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

        IEnumerator WaveCalling(EnemyTeamData teamData)
        {
            yield return new WaitForSeconds(teamData.WaveWaitTime);
            var team = EnemyTeamFactory.GetEnemyTeam(teamData);
            team.SummonMember();
            CallWave();
        }
    }
}
