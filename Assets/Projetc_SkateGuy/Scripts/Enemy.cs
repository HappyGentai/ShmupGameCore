using UnityEngine;
using SkateGuy.States;
using UnityEngine.Events;

namespace SkateGuy.GameElements
{
    public abstract class Enemy : MonoBehaviour, IRecycleable, IDamageable
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
                    _OnPlayerDie.Invoke();
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

        protected StateController StateController = null;

        protected UnityEvent<float> _OnHPChange = new UnityEvent<float>();
        public abstract UnityEvent<float> OnHPChange { get; protected set; }
        protected UnityEvent _OnPlayerDie = new UnityEvent();
        public abstract UnityEvent OnPlayerDie { get; protected set; }

        public virtual void WakeUpObject()
        {
            HP = MaxHP;
        }

        public virtual void SleepObject()
        {

        }

        protected virtual void Start()
        {
            WakeUpObject();
            StateController = new StateController();
        }

        protected virtual void Update()
        {
            StateController.Track();
        }

        public virtual void Recycle()
        {
            m_HitBox.enabled = false;
            StateController.SetState(null);
        }

        public virtual void GetHit(float dmg)
        {
            HP -= dmg;
        }

        protected abstract void Die();
    }
}
