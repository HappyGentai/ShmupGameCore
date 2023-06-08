using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using SkateGuy.Datas;
using SkateGuy.GameElements;

namespace SkateGuy.Skills
{
    /// <summary>
    /// Use to cast skill
    /// </summary>
    [System.Serializable]
    public class SkillTrigger
    {
        [SerializeField]
        private InputAction m_TriggerInput = null;
        [SerializeField]
        private SkillData<PlayableObject> m_StorgeSkill = null;
        public SkillData<PlayableObject> SkillData
        {
            get { return m_StorgeSkill; }
            private set { }
        }
        [SerializeField]
        private PlayableObject m_Caster = null;
        private bool setInputEvent = false;
        public UnityEvent OnSkillCast = new UnityEvent();

        public void AwakeTrigger()
        {
            m_TriggerInput.Enable();
            if (!setInputEvent)
            {
                m_TriggerInput.started += (ctx) => {
                    var castSuccess =  m_StorgeSkill.TryCastSkill();
                    if (castSuccess)
                    {
                        OnSkillCast?.Invoke();
                    }
                };
                SkillData.CreateSKillEntity(m_Caster);
                setInputEvent = true;
            }
        }

        public void SleepTrigger()
        {
            m_TriggerInput.Disable();
        }
    }
}
