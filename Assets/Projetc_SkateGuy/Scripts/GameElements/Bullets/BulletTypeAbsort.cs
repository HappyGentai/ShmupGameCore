using System.Collections;
using UnityEngine;

namespace SkateGuy.GameElements
{
    public class BulletTypeAbsort : Bullet
    {
        [SerializeField]
        private CircleCollider2D m_Collider = null;
        [SerializeField]
        private LayerMask m_AbsortTarget = 0;
        [SerializeField]
        private float m_CheckUpdateTime = 0.1f;
        [SerializeField]
        private float m_BulletLifeTime = 3f;
        [SerializeField]
        private int m_MaxAbsortCount = 50;
        [SerializeField]
        private float m_DamageUpRatePerAbsort = 0.5f;
        [SerializeField]
        private float m_SizeUpRatePerAbsort = 0.1f;

        private Coroutine absortCheckRoutine = null;
        private float originalDamage = 0;
        private Vector3 originalSize = Vector3.zero;

        public override void WakeUpBullet()
        {
            originalDamage = m_Damage;
            originalSize = this.transform.localScale;
            absortCheckRoutine = StartCoroutine(AbsortChecking());
        }

        protected override void BulletDead()
        {
            base.BulletDead();
            m_Damage = originalDamage;
            this.transform.localScale = originalSize;
            if (absortCheckRoutine != null)
            {
                StopCoroutine(absortCheckRoutine);
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_BulletBelong == collision.tag)
            {
                return;
            }
            var damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                if (m_OnBulletHit != null)
                {
                    m_OnBulletHit.Invoke();
                }
                damageable.GetHit(m_Damage);
            }
        }

        private IEnumerator AbsortChecking()
        {
            var waitTime = new WaitForSeconds(m_CheckUpdateTime);
            var totalTime = 0f;
            var absortCount = 0;
            while (true)
            {
                yield return waitTime;
                totalTime += m_CheckUpdateTime;

                var targets = Physics2D.OverlapCircleAll
                    (this.transform.position,
                    m_Collider.radius * this.transform.localScale.x,
                    m_AbsortTarget);
                int targetCount = targets.Length;
                for (int index = 0; index < targetCount; ++index)
                {
                    var recycleable = targets[index].GetComponent<IRecycleable>();
                    if (targets[index] != this.m_Collider)
                    {
                        recycleable?.Recycle();
                        absortCount++;
                    }
                }
                if (absortCount > m_MaxAbsortCount)
                {
                    absortCount = m_MaxAbsortCount;
                }
                m_Damage = originalDamage + absortCount * m_DamageUpRatePerAbsort;
                this.transform.localScale = originalSize + (absortCount * m_SizeUpRatePerAbsort) *Vector3.one;

                if (totalTime >= m_BulletLifeTime)
                {
                    BulletDead();
                    break;
                }
            }
        }
    }
}
