using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class TriggerListener : MonoBehaviour
{
    [SerializeField]
    private LayerMask m_TargetLayer = 0;
    private UnityEvent<Collider2D> eventWhenCollisionEnter = new UnityEvent<Collider2D>();
    private UnityEvent<Collider2D> eventWhenTriggerStay = new UnityEvent<Collider2D>();

    public void AddCollisionEnterEvent(UnityAction<Collider2D> @event)
    {
        eventWhenCollisionEnter.AddListener(@event);
    }

    public void RemoveCollisionEnterEvent(UnityAction<Collider2D> @event)
    {
        eventWhenCollisionEnter.RemoveListener(@event);
    }

    public void AddTriggerStayEvent(UnityAction<Collider2D> @event)
    {
        eventWhenTriggerStay.AddListener(@event);
    }

    public void RemoveTriggerStayEvent(UnityAction<Collider2D> @event)
    {
        eventWhenTriggerStay.RemoveListener(@event);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((m_TargetLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            eventWhenCollisionEnter.Invoke(collision);
        }  
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((m_TargetLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            eventWhenTriggerStay.Invoke(collision);
        }
    }
}
