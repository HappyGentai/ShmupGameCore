using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpaceDrop : MonoBehaviour
{
    [SerializeField]
    private Gear m_CarryGear = null;
    [SerializeField]
    private float m_StartSpeed = 10f;
    [SerializeField]
    private float m_SpeedLossingPerFrame = 0.1f;
    [SerializeField]
    private TriggerListener m_HitChecker = null;

    private Coroutine m_FlyRoutine = null;

    public void StartFly()
    {
        m_HitChecker.AddCollisionEnterEvent(HitCheck);
        m_FlyRoutine = StartCoroutine(Working());
    }

    private void HitCheck(Collider2D collider2D )
    {
        ICollision collision = collider2D.gameObject.GetComponent<ICollision>();
        if (collision != null)
        {
            collision.CollisionHit(this.transform);
        }
    }

    IEnumerator Working()
    {
        float speed = m_StartSpeed;
        while(true)
        {
            this.transform.position += Vector3.right * Time.deltaTime * speed;
            speed -= m_SpeedLossingPerFrame;
            if (speed <= 0)
            {
                m_HitChecker.RemoveCollisionEnterEvent(HitCheck);
                Gear gear = Instantiate(m_CarryGear, this.transform.position, Quaternion.identity);
                Destroy(this.gameObject);
                m_FlyRoutine = null;
                break;
            }

            yield return null;
        }
    }
}
