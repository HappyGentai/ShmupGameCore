using UnityEngine;
using UnityEngine.UI;
using SkateGuy.GameElements;

namespace SkateGuy.Test {
    public class TestPlayerUI : MonoBehaviour
    {
        [SerializeField]
        private BasicPlayer m_Player = null;
        [SerializeField]
        private Image m_PlayerHP = null;
        [SerializeField]
        private Image m_GrazeCounter = null;
        [SerializeField]
        private Image m_SkillImage = null;
        [SerializeField]
        private Image m_SkillImage2 = null;
        [SerializeField]
        private Image m_SkillImage3 = null;
        [SerializeField]
        private Color m_SkillWhenCanUse= Color.white;
        [SerializeField]
        private Color m_SkillWhenCantUse = Color.gray;
        [SerializeField]
        private Color m_SkillWhenUsing = Color.red;

        public void Initialization()
        {
            var maxHP = m_Player.MaxHP;
            m_PlayerHP.fillAmount = m_Player.HP / maxHP; ;
            m_Player.OnHPChange.AddListener((float currentHp) => {
                m_PlayerHP.fillAmount = currentHp / maxHP;
            });
            var skillTrigger = m_Player.SkillTriggers[0];
            var skillData = skillTrigger.SkillData;
            var skillUsing = false;
            skillData.AddOnSkillCastingChangeEvent((bool _skillUsing) =>
            {
                skillUsing = _skillUsing;
            });
            var skillTrigger2 = m_Player.SkillTriggers[1];
            var skillData2 = skillTrigger2.SkillData;
            var skillUsing2 = false;
            skillData2.AddOnSkillCastingChangeEvent((bool _skillUsing) =>
            {
                skillUsing2 = _skillUsing;
            });
            var skillTrigger3 = m_Player.SkillTriggers[2];
            var skillData3 = skillTrigger3.SkillData;
            var skillUsing3 = false;
            skillData3.AddOnSkillCastingChangeEvent((bool _skillUsing) =>
            {
                skillUsing3 = _skillUsing;
            });
            var maxGrazeCounter = m_Player.MaxGrazeCounter;
            m_GrazeCounter.fillAmount = m_Player.GrazeCounter / m_Player.MaxGrazeCounter;
            m_Player.OnGrazeCounterChange.AddListener((float currentGrazeCounter) => {
                m_GrazeCounter.fillAmount = currentGrazeCounter / maxGrazeCounter;
                if (skillUsing)
                {
                    m_SkillImage.color = m_SkillWhenUsing;
                }
                else if (m_Player.GrazeCounter >= skillData.GrazeEnergyCost)
                {
                    m_SkillImage.color = m_SkillWhenCanUse;
                }
                else
                {
                    m_SkillImage.color = m_SkillWhenCantUse;
                }
                if (skillUsing2)
                {
                    m_SkillImage2.color = m_SkillWhenUsing;
                }
                else if (m_Player.GrazeCounter >= skillData.GrazeEnergyCost)
                {
                    m_SkillImage2.color = m_SkillWhenCanUse;
                }
                else
                {
                    m_SkillImage2.color = m_SkillWhenCantUse;
                }
                if (skillUsing3)
                {
                    m_SkillImage3.color = m_SkillWhenUsing;
                }
                else if (m_Player.GrazeCounter >= skillData.GrazeEnergyCost)
                {
                    m_SkillImage3.color = m_SkillWhenCanUse;
                }
                else
                {
                    m_SkillImage3.color = m_SkillWhenCantUse;
                }
            });
        }
    }
}
