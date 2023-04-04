using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private Image m_HpFill = null;
    [SerializeField]
    private Image m_GrazeFill = null;
    [SerializeField]
    private Image m_GearCall = null;
    private Color canUse = Color.white;
    private Color cantUse = Color.gray;
    private int maxHp = 0;
    private float maxGrazeCounter = 0;

    public void SetMaxHp(int _maxHp)
    {
        maxHp = _maxHp;
    }

    public void SetMaxGrazeCounter(float _maxGrazeCounter)
    {
        maxGrazeCounter = _maxGrazeCounter;
    }

    public void HPChange(int newhp)
    {
        float fill = (float)newhp / maxHp;
        m_HpFill.fillAmount = fill;
    }

    public void GrazeCounterChange(float newGrazeCounter)
    {
        float fill = newGrazeCounter / maxGrazeCounter;
        m_GrazeFill.fillAmount = fill;
    }

    public void CanGearCallCheck(bool can)
    {
        if (can)
        {
            m_GearCall.color = canUse;
        }
        else
        {
            m_GearCall.color = cantUse;
        }
    }
}
