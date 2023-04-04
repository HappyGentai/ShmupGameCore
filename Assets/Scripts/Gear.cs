using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gear : MonoBehaviour
{
    [SerializeField]
    private GearWeapon m_Weapon = null;
    [SerializeField]
    private TriggerListener m_HitChecker = null;
    [SerializeField]
    private float m_PushUpSpeed = 5f;
    private UnityEvent eventWhenPushUp = new UnityEvent();

    private Coroutine pushingUpRoutine = null;

    private void Start()
    {
        m_Weapon.AddAmmoOutListener(AmmoOut);
        m_HitChecker.enabled = false;
    }

    public void Fire()
    {
        m_Weapon.Fire();
    }

    public void StopFire()
    {
        m_Weapon.StopFire();
    }

    public void AddPushUpEventListener(UnityAction @event)
    {
        eventWhenPushUp.AddListener(@event);
    }

    public void RemovePushUpEventListener(UnityAction @event)
    {
        eventWhenPushUp.RemoveListener(@event);
    }

    public void PushUp()
    {
        eventWhenPushUp.Invoke();
        m_HitChecker.enabled = true;
        m_HitChecker.AddCollisionEnterEvent(CollisionHit);
        pushingUpRoutine = StartCoroutine(PushingUp());
    }

    private void AmmoOut()
    {
        m_Weapon.RemoveAmmoOutListener(AmmoOut);
        PushUp();
    }

    private void CollisionHit(Collider2D collider)
    {
        ICollision collision = collider.gameObject.GetComponent<ICollision>();
        if (collision != null)
        {
            collision.CollisionHit(this.transform);
        }
    }

    IEnumerator PushingUp()
    {
        float counter = 5f;
        while(true)
        {
            if (counter <= 0)
            {
                Destroy(this.gameObject);
            }
            float dt = Time.deltaTime;
            counter -= dt;
            this.transform.Translate(Vector3.right * m_PushUpSpeed * dt);
            yield return null;
        }
    }

    private void OnDestroy()
    {
        if (pushingUpRoutine != null)
        {
            m_HitChecker.RemoveCollisionEnterEvent(CollisionHit);
            StopCoroutine(pushingUpRoutine);
        }
    }
}
