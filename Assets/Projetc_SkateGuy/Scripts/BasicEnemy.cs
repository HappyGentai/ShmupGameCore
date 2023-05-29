using UnityEngine;
using SkateGuy.States.EnemyStates;
using UnityEngine.Events;

namespace SkateGuy.GameElements
{
    public class BasicEnemy : Enemy
    {
        public override Transform MoveTarget {
            get { return m_MoveTarget; }
            protected set { }
        }
        public override float MaxHP
        {
            get { return m_MaxHP; }
            protected set { }
        }
        public override CircleCollider2D HitBox {
            get { return m_HitBox; }
            protected set { }
        }
        public override float MoveSpeed {
            get { return m_MoveSpeed; }
            protected set { }
        }
        public override Launcher[] Launchers {
            get { return m_Launchers; }
            set { m_Launchers = value; }
        }

        public override UnityEvent<float> OnHPChange
        {
            get { return _OnHPChange; }
            protected set { }
        }

        public override UnityEvent OnEnemyDie
        {
            get { return _OnEnemyDie; }
            protected set { }
        }

        [Header("Option")]
        [SerializeField]
        private bool m_PlayWhenStart = false;

        protected void Start()
        {
            if (m_PlayWhenStart)
            {
                Initialization();
                StartAction();
            }
        }

        public override void StartAction()
        {
            WakeUpObject();
            var adiotEnemyState = new EnemyStateAdiotMove(StateController, this, Vector2.left);
            StateController.SetState(adiotEnemyState);
        }

        public override void ReSetData()
        {

        }

        protected override void Die()
        {
            //  Do die event, can call WakeUpObject to re set data
        }
    }
}
