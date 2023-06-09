using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ShmupCore.GameElement
{
    public class Enemy : MonoBehaviour, IDamageable, IRecycleable, IInvincible
    {
        [SerializeField]
        private SpriteRenderer m_RenderTarget = null;
        [SerializeField]
        private Collider2D m_HitBox = null;
        [SerializeField]
        private float m_Hp = 100;
        [SerializeField]
        private float hp = 0;
        private float HP
        {
            get { return hp; }
            set
            {
                if (!isInvincible)
                {
                    hp = value;
                }
                
                if (hp <= 0)
                {
                    EnemyDead();
                } else
                {
                    showDamageFlash();
                }

                void showDamageFlash()
                {
                    if (damagedFlash == null)
                    {
                        damagedFlash = StartCoroutine(DamagedFlashing());
                    }
                }
            }
        }
        [SerializeField]
        private bool isInvincible = false;

        public Vector2 m_MoveDirection = Vector2.left;
        [SerializeField]
        private float m_Speed = 1f;

        [SerializeField]
        private Launcher[] m_Launchers = null;
        /// <summary>
        /// Delay sec
        /// </summary>
        [SerializeField]
        private float m_AttackDelay = 0;
        private Coroutine attackRoutine = null;
        private Coroutine damagedFlash = null;
        [SerializeField]
        private UnityEvent m_OnPlayerDead = null;
        [HideInInspector]
        public UnityAction eventWhenEnemyDead = null;

        // Update is called once per frame
        void Update()
        {
            Move();
        }

        public void Move()
        {
            this.transform.localPosition += (Vector3)m_MoveDirection * m_Speed * Time.deltaTime;
        }

        public void StopFire()
        {
            if (attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
            }
        }

        public void ReSetData()
        {
            hp = m_Hp;
            StopFire();
            attackRoutine = StartCoroutine(Attacking());
            m_HitBox.enabled = true;
            isInvincible = false;
        }

        public void EnemyDead()
        {
            if (m_OnPlayerDead != null)
            {
                m_OnPlayerDead.Invoke();
            }
            if (eventWhenEnemyDead != null)
            {
                eventWhenEnemyDead();
            }
        }

        public void GetHit(float dmg)
        {
            HP -= dmg;
        }

        public void Recycle()
        {
            m_HitBox.enabled = false;
            StopFire();
            if (damagedFlash != null)
            {
                StopCoroutine(damagedFlash);
                damagedFlash = null;
            }
            m_RenderTarget.color = Color.white;
            int launcherCount = m_Launchers.Length;
            for (int index = 0; index < launcherCount; ++index)
            {
                Launcher launcher = m_Launchers[index];
                launcher.StopLauncher();
            }
        }

        public void SetInvincible(bool _isInvincible)
        {
            isInvincible = _isInvincible;
        }

        IEnumerator Attacking()
        {
            yield return new WaitForSeconds(m_AttackDelay);

            int launcherCount = m_Launchers.Length;
            while (true)
            {
                yield return null;
                for (int index = 0; index < launcherCount; ++index)
                {
                    Launcher launcher = m_Launchers[index];
                    if (!launcher.IsWorking)
                    {
                        launcher.AwakeLauncher();
                    }
                    launcher.Fire();
                }
            }
        }

        IEnumerator DamagedFlashing()
        {
            m_RenderTarget.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            m_RenderTarget.color = Color.white;
            damagedFlash = null;
        }
    }
}
