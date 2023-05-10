using UnityEngine;
using SkateGuy.States.PlayerStates;
using UnityEngine.Events;
using SkateGuy.Skills;

namespace SkateGuy.GameElements
{
    public class BasicPlayer : PlayableObject
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
        public override float MaxGrazeCounter
        {
            get { return m_MaxGrazeCounter; }
            protected set { }
        }
        public override float MoveSpeed
        {
            get { return m_MoveSpeed; }
            protected set { }
        }
        public override float FocusModeScaleRate
        {
            get { return m_FocusModeScaleRate; }
            protected set { }
        }
        public override bool IsFocusMode
        {
            get { return isFocusMode; }
            set { isFocusMode = value; }
        }
        public override Launcher[] Launchers
        {
            get { return m_Launchers; }
            set { m_Launchers = value; }
        }

        public override UnityEvent<float> OnHPChange {
            get { return _OnHPChange; }
            protected set { }
        }

        public override UnityEvent<float> OnGrazeCounterChange
        {
            get { return _OnGrazeCounterChange; }
            protected set { }
        }

        public override UnityEvent<Collider2D[]> OnGraze
        {
            get { return _OnGraze; }
            protected set { }
        }

        public override UnityEvent OnPlayerDie
        {
            get { return _OnPlayerDie; }
            protected set { }
        }

        [Header("Skills")]
        [SerializeField]
        private SkillTrigger[] m_SkillTriggers = null;
        public SkillTrigger[] SkillTriggers
        {
            get { return m_SkillTriggers; }
            private set { }
        }

        protected void Awake()
        {
            Initialization();
            WakeUpObject();
            var basicPlayerControl = new PlayerStateBasicControl(StateController, this);
            StateController.SetState(basicPlayerControl);
        }

        public override void WakeUpObject()
        {
            base.WakeUpObject();
            //  Wake up skill triggers
            var skillTriggerCount = m_SkillTriggers.Length;
            for (int index = 0; index < skillTriggerCount; ++index)
            {
                var skilltrigger = m_SkillTriggers[index];
                skilltrigger.AwakeTrigger();
            }
        }

        public override void SleepObject()
        {
            base.SleepObject();
            //  Sleep skill triggers
            var skillTriggerCount = m_SkillTriggers.Length;
            for (int index = 0; index < skillTriggerCount; ++index)
            {
                var skilltrigger = m_SkillTriggers[index];
                skilltrigger.SleepTrigger();
            }
        }

        protected override void Die()
        {
            //  Do die event, can call WakeUpObject to re set data
        }
    }
}