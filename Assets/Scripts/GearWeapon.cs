using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GearWeapon : MonoBehaviour
{
    [SerializeField]
    private Launcher m_Launcher = null;
    [SerializeField]
    private float m_FireCoolDown = 1f;
    [SerializeField]
    private int m_Ammo = 40;
    [SerializeField]
    private int m_FireTime = 1;
    private UnityEvent m_AmmoOutEvent = new UnityEvent();

    private float fireCounter = 0;
    private int ammo = 0;

    private Coroutine firingRoutine = null;

    void Start()
    {
        ammo = m_Ammo;
    }

    public void AddAmmoOutListener(UnityAction listener)
    {
        m_AmmoOutEvent.AddListener(listener);
    }

    public void RemoveAmmoOutListener(UnityAction listener)
    {
        m_AmmoOutEvent.RemoveListener(listener);
    }

    public void Fire()
    {
        if (firingRoutine != null || ammo == 0) { return; }
        firingRoutine = StartCoroutine(WeaponFiring());
    }

    public void StopFire()
    {
        if (firingRoutine != null)
        {
            StopCoroutine(firingRoutine);
            firingRoutine = null;
        }
    }

    IEnumerator WeaponFiring()
    {
        while(true)
        {
            if (fireCounter <= 0)
            {
                m_Launcher.Fire(m_FireTime);
                ammo--;
                if (ammo == 0)
                {
                    m_AmmoOutEvent.Invoke();
                    break;
                }
                fireCounter = m_FireCoolDown;
            }
            else
            {
                fireCounter -= Time.deltaTime;
            }

            yield return null;
        }
        firingRoutine = null;
    }
}
