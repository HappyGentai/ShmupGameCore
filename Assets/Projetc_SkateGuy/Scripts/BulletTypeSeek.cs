using UnityEngine;
using SkateGuy.Tool;
using System.Collections;

namespace SkateGuy.GameElements
{
    public class BulletTypeSeek : Bullet
    {
        [SerializeReference]
        private float m_MaxSeekForce = 0.1f;
        [SerializeField]
        private LayerMask m_TargetLayer = 0;
        [SerializeField]
        private float m_SearchRadius = 50f;
        private Transform seekTarget = null;
        [SerializeField]
        private bool m_DelaySeek = false;
        [SerializeField]
        private float m_DelayTime = 0.5f;

        protected void OnEnable()
        {
            //  Search target
            if (!m_DelaySeek)
            {
                SeekTarget();
            }
            else
            {
                StartCoroutine(DelaySeeking());
            }
        }

        protected override void Update()
        {
            var moveVel = MoveDir;
            if (seekTarget == null)
            {
                moveVel *= m_MoveSpeed;
            } else
            {
                var selfPos = this.transform.localPosition;
                var targetPos = seekTarget.localPosition;
                MoveDir = SteeringBehaviors.Seek(selfPos, targetPos, MoveDir, m_MoveSpeed, m_MaxSeekForce);
                moveVel = MoveDir;
            }
            this.transform.localPosition += (Vector3)moveVel * Time.deltaTime;
        }

        protected override void BulletDead()
        {
            base.BulletDead();
            seekTarget = null;
        }

        protected void SeekTarget()
        {
            var selfPos = this.transform.localPosition;
            var findTarget = Physics2D.OverlapCircle(selfPos, m_SearchRadius, m_TargetLayer);
            if (findTarget != null)
            {
                seekTarget = findTarget.transform;
            }
        }

        protected IEnumerator DelaySeeking()
        {
            yield return new WaitForSeconds(m_DelayTime);
            SeekTarget();
        }
    }
}
