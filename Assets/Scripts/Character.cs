using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [SerializeField]
    private int m_MaxHp = 100;
    public int MaxHP
    {
        get { return m_MaxHp; }
    }
    [SerializeField]
    private int hp = 0;
    [SerializeField]
    private float m_MoveSpeed = 5;
    [SerializeField]
    private Vector2 m_MoveRangeX = Vector2.zero;    // L R
    [SerializeField]
    private Vector2 m_MoveRangeY = Vector2.zero;    // U D
    [SerializeField]
    private float m_GrazeGainEnergy = 0.1f;
    [SerializeField]
    private float m_GearCallMaxEnergy = 100;
    public float GearCallMaxEnergy
    {
        get { return m_GearCallMaxEnergy; }
    }
    [SerializeField]
    private float m_GearCallEnergy = 0;
    private float GearCallEnergy
    {
        get { return m_GearCallEnergy; }
        set
        {
            if (value >= m_GearCallMaxEnergy)
            {
                m_GearCallEnergy = m_GearCallMaxEnergy;
                m_GearCallEvent.Invoke(true);
            }
            else
            {
                m_GearCallEnergy = value;
            }
            m_GrazeCounterChangeEvent.Invoke(m_GearCallEnergy);
        }
    }
    [SerializeField]
    private TriggerListener heartListener = null;
    [SerializeField]
    private TriggerListener GrazeListener = null;
    [SerializeField]
    private Transform m_GearSetPoint = null;
    [SerializeField]
    private SpaceDrop m_SpaceDrop = null;

    private Gear gear = null;
    private bool canControl = false;
    private UnityEvent<float> m_GrazeCounterChangeEvent = new UnityEvent<float>();
    private UnityEvent<bool> m_GearCallEvent = new UnityEvent<bool>();
    private UnityEvent<int> m_HpChangeEvent = new UnityEvent<int>();

    void Start()
    {
        Initialization();
        //  Add grze and got hit call back event
        heartListener.AddCollisionEnterEvent(GetHurt);
        GrazeListener.AddTriggerStayEvent(Graze);
    }

    private void Update()
    {
        if (!canControl)
        {
            return;
        }
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float newPosX = transform.position.x + x * Time.deltaTime * m_MoveSpeed;
        float newPosY = transform.position.y + y * Time.deltaTime * m_MoveSpeed;
        if (newPosX < m_MoveRangeX.x)
        {
            newPosX = m_MoveRangeX.x;
        }
        else if (newPosX > m_MoveRangeX.y)
        {
            newPosX = m_MoveRangeX.y;
        }
        if (newPosY > m_MoveRangeY.x)
        {
            newPosY = m_MoveRangeY.x;
        }
        else if (newPosY < m_MoveRangeY.y)
        {
            newPosY = m_MoveRangeY.y;
        }
        transform.position = new Vector3(newPosX, newPosY, 0);

        if (m_GearCallEnergy >= m_GearCallMaxEnergy && Input.GetKeyUp(KeyCode.Space))
        {
            Vector3 pos = this.transform.position;
            SpaceDrop spaceDrop = Instantiate<SpaceDrop>(m_SpaceDrop, pos - Vector3.right * 2, Quaternion.identity);
            spaceDrop.StartFly();
            GearCallEnergy = 0;
            m_GearCallEvent.Invoke(false);
        }

        if (gear != null)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                gear.Fire();
            }
            else if (Input.GetKeyUp(KeyCode.Z))
            {
                gear.StopFire();
            }
            else if (Input.GetKeyUp(KeyCode.X))
            {
                gear.PushUp();
            }
        }
    }

    private void Graze(Collider2D collision2D)
    {
        if (!canControl || gear != null)
        {
            return;
        }

        GearCallEnergy += m_GrazeGainEnergy;
    }

    private void GetHurt(Collider2D collision2D)
    {
        if (!canControl)
        {
            return;
        }

        ICollision collision = collision2D.gameObject.GetComponent<ICollision>();
        if (collision != null) {
            int dmg = collision.CollisionDNG();
            hp -= dmg;
            m_HpChangeEvent.Invoke(hp);
            if (hp <= 0)
            {
                Debug.Log("Player is dead :{");
                canControl = false;
            }
        }

        Gear _gear = collision2D.gameObject.GetComponent<Gear>();
        if (_gear != null)
        {
            gear = _gear;
            gear.transform.SetParent(m_GearSetPoint);
            gear.transform.localPosition = Vector3.zero;
            gear.AddPushUpEventListener(GearPushUpListener);
        }
    }

    private void GearPushUpListener()
    {
        gear.transform.SetParent(null);
        gear.RemovePushUpEventListener(GearPushUpListener);
        gear = null;
    }

    private void Initialization()
    {
        hp = m_MaxHp;
        m_GearCallEnergy = 0;
        canControl = true;
    }

    #region Event Listener
    public void AddGearCounterChangeEvent(UnityAction<float> @event) {
        m_GrazeCounterChangeEvent.AddListener(@event);
    }
    public void RemoveGearCounterChangeEvent(UnityAction<float> @event)
    {
        m_GrazeCounterChangeEvent.RemoveListener(@event);
    }

    public void AddGearCallEvent(UnityAction<bool> @event)
    {
        m_GearCallEvent.AddListener(@event);
    }
    public void RemoveGearCallEvent(UnityAction<bool> @event)
    {
        m_GearCallEvent.RemoveListener(@event);
    }

    public void AddHpCallEvent(UnityAction<int> @event)
    {
        m_HpChangeEvent.AddListener(@event);
    }
    public void RemoveGearCallEvent(UnityAction<int> @event)
    {
        m_HpChangeEvent.RemoveListener(@event);
    }
    #endregion
}
