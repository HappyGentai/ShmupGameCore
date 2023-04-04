using System.Collections;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField]
    private Bullet m_Bullet = null;
    [SerializeField]
    private float m_FireDelay = 0.1f;
    [SerializeField]
    private Vector2 m_FireAdjustValue = Vector2.zero;
    [SerializeField]
    private int m_MultiFireCount = 1;
    [SerializeField]
    private float m_MultiFireDistence = 1;
    [SerializeField]
    private bool m_TestAuto = false;
    private Coroutine fireRoutine = null;

    private void Start()
    {
        if (m_TestAuto)
        {
            UnlimitedFire();
        }
    }

    public void Fire(int fireTime)
    {
        ReSetFireRoutine();
        fireRoutine = StartCoroutine(Firing(fireTime));
    }

    public void UnlimitedFire()
    {
        ReSetFireRoutine();
        fireRoutine = StartCoroutine(UnlimitedFiring());
    }

    public void StopFire()
    {
        ReSetFireRoutine();
    }

    private void ReSetFireRoutine()
    {
        if (fireRoutine != null)
        {
            StopCoroutine(fireRoutine);
            fireRoutine = null;
        }
    }

    private Bullet CreateBullet()
    {
        return Instantiate(m_Bullet);
    }

    private Vector3 GetFirePoint()
    {
        return this.transform.position + 
            this.transform.right * m_FireAdjustValue.x + 
            this.transform.up * m_FireAdjustValue.y;
    }

    private void FireBullet()
    {
        Bullet bullet = CreateBullet();
        bullet.transform.position = GetFirePoint();
        Vector3 flyDir = this.transform.right;
        bullet.FlyDirection = flyDir;
    }

    private void MultiFireBullet()
    {
        if (m_MultiFireCount%2 != 0)
        {
            bool positivee = true;
            int posiviteLV = 1;
            int reverseLV = 1;
            for (int index = 0; index < m_MultiFireCount; ++index)
            {
                Bullet bullet = CreateBullet();
                bullet.transform.position = GetFirePoint();
                Vector3 flyDir = this.transform.right;
                if (index != 0)
                {
                    if (positivee)
                    {
                        flyDir += this.transform.up * m_MultiFireDistence * posiviteLV;
                        posiviteLV++;
                        positivee = !positivee;
                    }
                    else
                    {
                        flyDir -= this.transform.up * m_MultiFireDistence * reverseLV;
                        reverseLV++;
                        positivee = !positivee;
                    }
                }
                bullet.FlyDirection = flyDir;
            }
        }
        else
        {
            bool positivee = true;
            int posiviteLV = 1;
            int reverseLV = 1;
            for (int index = 0; index < m_MultiFireCount; ++index)
            {
                Bullet bullet = CreateBullet();
                bullet.transform.position = GetFirePoint();
                Vector3 flyDir = this.transform.right;
                if (positivee)
                {
                    flyDir += this.transform.up * (m_MultiFireDistence / (m_MultiFireDistence * posiviteLV));
                    posiviteLV++;
                    positivee = !positivee;
                }
                else
                {
                    flyDir -= this.transform.up * (m_MultiFireDistence / (m_MultiFireDistence * posiviteLV));
                    reverseLV++;
                    positivee = !positivee;
                }
                bullet.FlyDirection = flyDir;
            }
        }
    }

    public virtual IEnumerator Firing(int fireTime)
    {
        int totalTime = 0;
        while(totalTime < fireTime)
        {
            yield return new WaitForSeconds(m_FireDelay);
            if (m_MultiFireCount >1)
            {
                MultiFireBullet();
            }
            else
            {
                FireBullet();
            }
            totalTime++;
        }

        yield return null;
        fireRoutine = null;
    }

    public virtual IEnumerator UnlimitedFiring()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_FireDelay);
            if (m_MultiFireCount > 1)
            {
                MultiFireBullet();
            }
            else
            {
                FireBullet();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 adjustFirePoint = GetFirePoint();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(adjustFirePoint, 0.5f);
    }
}
