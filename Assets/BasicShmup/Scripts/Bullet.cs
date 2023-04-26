using UnityEngine;
using UnityEngine.Events;

namespace ShmupCore
{
    public class Bullet : MonoBehaviour, IRecycleable
    {
        [HideInInspector]
        public string m_BulletBelong = "";
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
                _MoveDir = value;
                var angle = Mathf.Atan2(_MoveDir.y, _MoveDir.x) * Mathf.Rad2Deg;
                m_RotateTarget.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }

        public UnityAction eventWhenBulletDead = null;

        // Update is called once per frame
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
                damageable.GetHit(m_Damage);
                BulletDead();
            }
        }
    }
}
