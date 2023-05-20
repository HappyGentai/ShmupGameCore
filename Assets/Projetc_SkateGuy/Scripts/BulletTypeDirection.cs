using UnityEngine;

namespace SkateGuy.GameElements
{
    public class BulletTypeDirection : Bullet
    {
        [SerializeField]
        private LayerMask m_TargetLayer = 0;
        [SerializeField]
        private float m_SearchRadius = 50f;
        private Transform seekTarget = null;

        protected override void Update()
        {
            if (seekTarget == null)
            {
                SearchTarget();
            } else
            {
                MoveDir = MoveDir.normalized;
                this.transform.localPosition += (Vector3)(MoveDir * m_MoveSpeed) * Time.deltaTime;
            }
        }

        protected override void BulletDead()
        {
            base.BulletDead();
            seekTarget = null;
        }

        private void SearchTarget()
        {
            var selfPos = this.transform.localPosition;
            var findTarget = Physics2D.OverlapCircle(selfPos, m_SearchRadius, m_TargetLayer);
            if (findTarget != null)
            {
                seekTarget = findTarget.transform;
                MoveDir = (seekTarget.localPosition - selfPos).normalized;
            }
        }
    }
}
