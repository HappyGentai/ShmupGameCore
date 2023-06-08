using SkateGuy.States.EnemyStates;
using UnityEngine.Events;
using UnityEngine;

namespace SkateGuy.GameElements
{
    public class EnemyTypeSinMove : Enemy
    {
        public override Transform MoveTarget
        {
            get { return m_MoveTarget; }
            protected set { }
        }
        public override float MaxHP
        {
            get { return m_MaxHP; }
            protected set { }
        }
        public override CircleCollider2D HitBox
        {
            get { return m_HitBox; }
            protected set { }
        }
        public override float MoveSpeed
        {
            get { return m_MoveSpeed; }
            protected set { }
        }
        public override Launcher[] Launchers
        {
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

        [Header("Logic value")]
        [SerializeField]
        private Vector2 m_MoveDirection = Vector2.zero;
        [SerializeField]
        private float m_SinHalfHeigh = 2;
        [SerializeField]
        private bool m_AttackWhenMove = false;

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
            var sinMoveState = new EnemyStateSinMove(StateController, this, m_MoveDirection, m_SinHalfHeigh, m_AttackWhenMove);
            StateController.SetState(sinMoveState);
        }

        public override void ReSetData()
        {

        }

        private void OnDrawGizmos()
        {
            var selfPos = this.transform.localPosition;
            var endPoint = selfPos + (Vector3)m_MoveDirection * MoveSpeed; 
            Gizmos.color = Color.green;
            Gizmos.DrawLine(selfPos, endPoint);
            Gizmos.color = Color.red;
            var pV = Vector2.Perpendicular(endPoint - selfPos).normalized * m_SinHalfHeigh;
            Gizmos.DrawLine(endPoint, endPoint + (Vector3)pV);
        }
    }
}
