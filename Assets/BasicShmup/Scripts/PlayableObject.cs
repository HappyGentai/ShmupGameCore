using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ShmupCore
{
    public class PlayableObject : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private Transform m_MoveTarget = null;

        [SerializeField]
        private float m_MoveSpeed = 3f;

        [SerializeField]
        [Range(0, 1f)]
        private float m_FocusModeScaleRate = 0.6f;
        private bool isFocusMode = false;

        [SerializeField]
        private InputAction m_MoveAction;
        [SerializeField]
        private InputAction m_FocusModeAction;
        [SerializeField]
        private InputAction m_FireAction;

        [SerializeField]
        private Launcher[] m_Launchers = null;

        public UnityAction EventWhenDead = null;

        public void OnEnable()
        {
            m_MoveAction.Enable();
            m_FocusModeAction.Enable();
            m_FireAction.Enable();
        }

        public void OnDisable()
        {
            m_MoveAction.Disable();
            m_FocusModeAction.Disable();
            m_FireAction.Disable();
        }

        private void Start()
        {
            m_FocusModeAction.started += (ctx) => {
                isFocusMode = true;
            };
            m_FocusModeAction.canceled += (ctx) => {
                isFocusMode = false;
            };
        }

        void Update()
        {
            if (m_FireAction.IsPressed())
            {
                Fire();
            }

            Vector2 moveValue = m_MoveAction.ReadValue<Vector2>();
            Move(moveValue.x, moveValue.y, isFocusMode);
        }

        public void GetHit(float dmg)
        {
            this.gameObject.SetActive(false);
            int launcherCount = m_Launchers.Length;
            for (int index = 0; index < launcherCount; ++index)
            {
                Launcher launcher = m_Launchers[index];
                launcher.StopLauncher();
            }
            if (EventWhenDead != null)
            {
                EventWhenDead.Invoke();
            }
        }

        public void ResetState()
        {
            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// Call all launcher fire(if have)
        /// </summary>
        private void Fire()
        {
            int launcherCount = m_Launchers.Length;
            for (int index = 0; index < launcherCount; ++index)
            {
                Launcher launcher = m_Launchers[index];
                if (!launcher.IsWorking)
                {
                    launcher.AwakeLauncher();
                }
                launcher.Fire();
            }
        }

        /// <summary>
        /// Contrall playableObject move
        /// </summary>
        /// <param name="hValue">horizontal move value</param>
        /// <param name="vValue">vertical move value</param>
        /// <param name="focusMode">is in focus mode?</param>
        private void Move(float hValue, float vValue, bool focusMode)
        {
            Vector2 moveValue = Vector2.zero;
            moveValue.x = hValue;
            moveValue.y = vValue;

            //  Normalize move value
            moveValue = moveValue.normalized;
            moveValue *= m_MoveSpeed;
            if (focusMode)
            {
                moveValue *= m_FocusModeScaleRate;
            }

            m_MoveTarget.transform.localPosition += (Vector3)moveValue * Time.deltaTime;
        }
    }
}
