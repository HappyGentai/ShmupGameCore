using UnityEngine;
using SkateGuy.States;
using UnityEngine.Events;

namespace SkateGuy.GameElements
{
    public abstract class Enemy : MonoBehaviour, IRecycleable, IDamageable, IInvincible, IShootable
    {
        [SerializeField]
        protected Transform m_MoveTarget = null;
        public abstract Transform MoveTarget { get; protected set; }
        [SerializeField]
        protected CircleCollider2D m_HitBox = null; 
        public abstract CircleCollider2D HitBox { get; protected set; }
        [Header("State")]
        [SerializeField]
        protected float m_MaxHP = 100;
        public abstract float MaxHP { get; protected set; }
        [SerializeField]
        protected float m_HP = 0;
        public virtual float HP
        {
            get { return m_HP; }
            set
            {
                m_HP = value;
                if (m_HP >= m_MaxHP)
                {
                    m_HP = m_MaxHP;
                }
                else if (m_HP < 0)
                {
                    m_HP = 0;
                }
                _OnHPChange.Invoke(m_HP);
                if (m_HP <= 0)
                {
                    Die();
                    OnEnemyDie.Invoke();
                }
            }
        }
        [SerializeField]
        protected float m_MoveSpeed = 1f;
        public abstract float MoveSpeed { get; protected set; }
        [Header("Launcher")]
        [SerializeField]
        protected Launcher[] m_Launchers = null;
        public abstract Launcher[] Launchers { get; set; }

        protected bool isInvincible = false;

        protected StateController StateController = null;

        protected UnityEvent<float> _OnHPChange = new UnityEvent<float>();
        public abstract UnityEvent<float> OnHPChange { get; protected set; }
        protected UnityEvent _OnEnemyDie = new UnityEvent();
        public abstract UnityEvent OnEnemyDie { get; protected set; }
        protected UnityEvent<Enemy> onRecycle = new UnityEvent<Enemy>();
        public  UnityEvent<Enemy> OnRecycle
        {
            get { return onRecycle; }
        }

        public virtual void WakeUpObject()
        {
            HP = MaxHP;
        }

        public virtual void SleepObject()
        {

        }

        public virtual void Initialization()
        {
            StateController = new StateController();
        }

        /// <summary>
        /// Used to call enemy to start(Set start state)
        /// </summary>
        public abstract void StartAction();

        protected virtual void Update()
        {
            StateController.Track();
        }

        public virtual void Recycle()
        {
            m_HitBox.enabled = false;
            StateController.SetState(null);
            OnRecycle.Invoke(this);
        }

        public virtual void SetInvincible(bool _isInvincible)
        {
            isInvincible = _isInvincible;
        }

        public virtual void CanShoot(bool _canShoot)
        {
            var launcherCount = Launchers.Length;
            for (int index = 0; index < launcherCount; ++index)
            {
                var launcher = Launchers[index];
                launcher.LauncherLock = !_canShoot;
            }
        }

        public virtual void GetHit(float dmg)
        {
            if (isInvincible)
            {
                return;
            }
            HP -= dmg;
        }

        public abstract void ReSetData();

        protected abstract void Die();
    }
}
