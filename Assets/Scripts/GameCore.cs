using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore : MonoBehaviour
{
    [SerializeField]
    private Character m_Character = null;
    [SerializeField]
    private PlayerUI m_PlayerUI = null;

    void Start()
    {
        m_PlayerUI.SetMaxHp(m_Character.MaxHP);
        m_PlayerUI.SetMaxGrazeCounter(m_Character.GearCallMaxEnergy);
        m_Character.AddHpCallEvent(m_PlayerUI.HPChange);
        m_Character.AddGearCounterChangeEvent(m_PlayerUI.GrazeCounterChange);
        m_Character.AddGearCallEvent(m_PlayerUI.CanGearCallCheck);
    }
}
