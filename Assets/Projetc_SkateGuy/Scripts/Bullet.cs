using UnityEngine;
using UnityEngine.Events;

namespace SkateGuy.GameElements
{
    public class Bullet : MonoBehaviour, IRecycleable
    {
        /// <summary>
        /// To define fire owner, if bullet collide with owner,
        /// will not trigger hit event.
        /// </summary>
        [HideInInspector]
        public string m_BulletBelong = "";
        /// <summary>
        /// If bullet need to rotate, set to this.
        /// </summary>
        [SerializeField]
        private Transform m_RotateTarget = null;
        [SerializeField]
        private float m_MoveSpeed = 5f;
        [SerializeField]
        private float m_Damage = 1;

        private Vector2 _MoveDir = Vector2.right;
        public Vector2 MoveDir
        {
            get { return _MoveDir; }
            set
            {
                //  Rotate object to move dir
                _MoveDir = value;
                var angle = Mathf.Atan2(_MoveDir.y, _MoveDir.x) * Mathf.Rad2Deg;
                if (m_RotateTarget != null)
                {
                    m_RotateTarget.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                }
            }
        }

        [SerializeField]
        private UnityEvent m_OnBulletHit = null;

        public UnityAction eventWhenBulletDead = null;

        void Update()
        {
            MoveDir = MoveDir.normalized;
            this.transform.localPosition += (Vector3)(MoveDir * m_MoveSpeed) * Time.deltaTime;
        }

        public void Recycle()
        {
            BulletDead();
        }

        private void BulletDead()
        {
            if (eventWhenBulletDead != null)
            {
                eventWhenBulletDead.Invoke();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //  Check can deal damage or not
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
                BulletDead();
            }
        }
    }
}
