using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidWall : MonoBehaviour
{
    [SerializeField]
    private TriggerListener m_HitDetect = null;
    
    void Start()
    {
        m_HitDetect.AddCollisionEnterEvent(Crash);
    }

    private void Crash(Collider2D collider2D)
    {
        Destroy(collider2D.gameObject);
    }
}
