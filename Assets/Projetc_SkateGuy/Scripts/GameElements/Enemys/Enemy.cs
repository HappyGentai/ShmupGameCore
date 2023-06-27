using UnityEngine;
using SkateGuy.States;
using SkateGuy.Effects;
using SkateGuy.Factories;
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
                }
            }
        }
        [SerializeField]
        protected float m_MoveSpeed = 1f;
        public abstract float MoveSpeed { get; protected set; }
        [SerializeField]
        protected float m_CloseDamage = 1;
        [SerializeField]
        protected LayerMask m_CloseDamageTarget = 0;
        [Header("Launcher")]
        [SerializeField]
        protected Launcher[] m_Launchers = null;
        public abstract Launcher[] Launchers { get; set; }

        protected bool isInvincible = false;

        protected StateController StateController = null;

        [Header("DieEffect")]
        [SerializeField]
        protected SFXEffecter m_DieEffect = null;

        [Header("Events")]
        protected UnityEvent<float> _OnHPChange = new UnityEvent<float>();
        public abstract UnityEvent<float> OnHPChange { get; protected set; }
        [SerializeField]
        protected UnityEvent m_OnGetDamaged = new UnityEvent();
        [SerializeField]
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
            m_HitBox.enabled = true;
        }

        public virtual void SleepObject()
        {
            m_HitBox.enabled = false;
            StateController.SetState(null);
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
            m_OnGetDamaged?.Invoke();
        }

        public abstract void ReSetData();

        protected virtual void Die()
        {
            if (m_DieEffect != null)
            {
                var dieEffect = EffectFactory.GetEffect(m_DieEffect);
                dieEffect.transform.localPosition = this.MoveTarget.localPosition;
                dieEffect.StartSFX();
            }
            OnEnemyDie.Invoke();
            Recycle();
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            var targetGO = collision.gameObject;
            if (((1 << targetGO.layer) & m_CloseDamageTarget) == 0)
            {
                return;
            }

            var damageable = targetGO.GetComponent<IDamageable>();
            damageable?.GetHit(m_CloseDamage);
        }
    }
}
