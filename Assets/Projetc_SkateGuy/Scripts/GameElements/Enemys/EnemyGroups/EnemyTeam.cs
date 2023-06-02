using UnityEngine;
using SkateGuy.GameElements.Factory;
using UnityEngine.Events;

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
        private UnityEvent onAllMemberGone = new UnityEvent();
        public UnityEvent OnAllMemberGone
        {
            get { return onAllMemberGone; }
        }

        private void Start()
        {
            if (m_SummonWhenStart)
            {
                SummonMember();
            }
        }

        public void SummonMember()
        {
            var memberCount = m_MemberDatas.Length;
            memberLiveCount = memberCount;
            for (int index = 0; index < memberCount; ++index)
            {
                var memberData = m_MemberDatas[index];
                var getEnemy =  EnemyFactory.GetEnemy(memberData.EnemyPrefab);
                getEnemy.MoveTarget.localPosition = memberData.SetPosition;
                getEnemy.StartAction();
                getEnemy.CanShoot(false);
                getEnemy.SetInvincible(true);
                getEnemy.OnRecycle.AddListener(OnTeamMemberRelease);
            }
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
    }
}
