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

        public override UnityEvent OnPlayerDie
        {
            get { return _OnPlayerDie; }
            protected set { }
        }

        protected override void Start()
        {
            base.Start();
            var adiotEnemyState = new EnemyStateAdiotMove(StateController, this, Vector2.left);
            StateController.SetState(adiotEnemyState);
        }

        protected override void Die()
        {
            //  Do die event, can call WakeUpObject to re set data
        }
    }
}
