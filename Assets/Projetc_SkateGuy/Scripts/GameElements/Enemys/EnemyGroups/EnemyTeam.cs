using UnityEngine;
using SkateGuy.GameElements.Factory;
using UnityEngine.Events;
using System.Collections;

namespace SkateGuy.GameElements.EnemyGroup
{
    public class EnemyTeam : MonoBehaviour
    {
        [SerializeField]
        private EnemyTeamMemberData[] m_MemberDatas = null;
        public EnemyTeamMemberData[] MemberDatas
        {
            get { return m_MemberDatas; }
            set
            {
                m_MemberDatas = value;
            }
        }
        [SerializeField]
        private bool m_SummonWhenStart = false;
        private int memberLiveCount = 0;
        private UnityEvent<Enemy> onMemberCreate = new UnityEvent<Enemy>();
        public UnityEvent<Enemy> OnMemberCreate
        {
            get { return onMemberCreate; }
        }
        private UnityEvent onAllMemberGone = new UnityEvent();
        public UnityEvent OnAllMemberGone
        {
            get { return onAllMemberGone; }
        }

        private int memberCount = 0;
        private int summonIndex = 0;
        private Coroutine summonRoutine = null;

        private void Start()
        {
            if (m_SummonWhenStart)
            {
                SummonMember();
            }
        }

        public void SummonMember()
        {
            memberCount = m_MemberDatas.Length;
            memberLiveCount = memberCount;
            summonIndex = 0;
            Summon();
        }

        public void StopSummon()
        {
            if (summonRoutine != null)
            {
                StopCoroutine(summonRoutine);
            }
        }

        private void Summon()
        {
            if (summonIndex >= memberCount)
            {
                return;
            }
            var memberData = m_MemberDatas[summonIndex];
            StopSummon();
            summonRoutine = StartCoroutine(Summoning(memberData));
            summonIndex++;
        }

        private void OnTeamMemberRelease(Enemy enemy)
        {
            enemy.OnRecycle.RemoveListener(OnTeamMemberRelease);
            memberLiveCount--;
            if (memberLiveCount == 0)
            {
                //  Member all dead or be recycle
                OnAllMemberGone.Invoke();
            }
        }

        private IEnumerator Summoning(EnemyTeamMemberData memberData)
        {
            var delayTime = new WaitForSeconds(memberData.DelaySpawnTime);
            yield return delayTime;
            var getEnemy = EnemyFactory.GetEnemy(memberData.EnemyPrefab);
            getEnemy.MoveTarget.localPosition = memberData.SetPosition;
            //  Check have logic or not
            if (getEnemy is ILogicDataSetable iDataSetable)
            {
                iDataSetable.SetLogicData(memberData.LogicData);
            }
            getEnemy.StartAction();
            getEnemy.CanShoot(false);
            getEnemy.SetInvincible(true);
            getEnemy.OnRecycle.AddListener(OnTeamMemberRelease);
            OnMemberCreate?.Invoke(getEnemy);
            Summon();
        }
    }
}
