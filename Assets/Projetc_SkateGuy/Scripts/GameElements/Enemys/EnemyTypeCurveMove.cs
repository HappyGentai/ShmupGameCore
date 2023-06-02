using UnityEngine;
using UnityEngine.Events;
using SkateGuy.Tool;
using SkateGuy.States.EnemyStates;

namespace SkateGuy.GameElements
{
    public class EnemyTypeCurveMove : Enemy
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
        [Tooltip("Will add current move target local position")]
        [SerializeField]
        private Vector2 m_CurveEndPos = Vector2.zero;
        [SerializeField]
        private Vector2 m_CurveAidPosA = Vector2.zero;
        [SerializeField]
        private Vector2 m_CurveAidPosB = Vector2.zero;
        [SerializeField]
        private float m_SpeedScale = 1;
        [SerializeField]
        private bool m_FireWhenMove = false;

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
            var startPos = MoveTarget.localPosition;
            var endPos = (Vector2)startPos + m_CurveEndPos;
            var curveMoveState = new EnemyStateCurveMove(StateController,
                this, startPos, endPos, m_CurveAidPosA,
                m_CurveAidPosB, m_SpeedScale, m_FireWhenMove);
            var selfDestructionState = new EnemyStateSelfDestruction(StateController, this);
            curveMoveState.nextState = selfDestructionState;
            StateController.SetState(curveMoveState);
        }

        public override void ReSetData()
        {
            
        }

        protected override void Die()
        {
            //  Do die event, can call WakeUpObject to re set data
        }

        private void OnDrawGizmos()
        {
            var startPos = MoveTarget.localPosition;
            var endPos = (Vector2)startPos + m_CurveEndPos;
            Gizmos.color = new Color(1, 0, 0, 0.25f);
            Gizmos.DrawSphere(startPos, 0.25f);
            Gizmos.DrawSphere(endPos, 0.25f);
            for (int index = 1; index < 10; index++)
            {
                Gizmos.color = new Color(0, 1, 0, 1);
                var progressPos = LineLerp.CubicLerp((Vector2)startPos, m_CurveAidPosA, m_CurveAidPosB, endPos, index * 0.1f);
                Gizmos.DrawSphere(progressPos, 0.15f);
            }
        }
    }
}
