using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace SkateGuy.GameElements
{
    public class PlayableObject : MonoBehaviour
    {
        [SerializeField]
        private Transform m_MoveTarget = null;

        [Header("State")]
        [SerializeField]
        private float m_MaxGrazeCounter = 100;
        [SerializeField]
        private float grazeCounter = 0;
        public float GrazeCounter
        {
            get { return grazeCounter; }
            private set
            {
                grazeCounter = value;
                if (grazeCounter > m_MaxGrazeCounter)
                {
                    grazeCounter  = m_MaxGrazeCounter;
                } else if (grazeCounter < 0)
                {
                    grazeCounter = 0;
                }
            }
        }

        [Header("Move")]
        [SerializeField]
        private float m_MoveSpeed = 3f;

        [SerializeField]
        [Range(0, 1f)]
        private float m_FocusModeScaleRate = 0.6f;
        private bool isFocusMode = false;

        [SerializeField]
        private LayerMask m_BorderLayer = 0;
        [SerializeField]
        private float m_BorderCheckUp = 1;
        [SerializeField]
        private float m_BorderCheckDown = 1;
        [SerializeField]
        private float m_BorderCheckLeft = 1;
        [SerializeField]
        private float m_BorderCheckRight = 1;

        [Header("Control")]
        [SerializeField]
        private InputAction m_MoveAction;
        [SerializeField]
        private InputAction m_FocusModeAction;
        [SerializeField]
        private InputAction m_FireAction;

        [Header("Launcher")]
        [SerializeField]
        private Launcher[] m_Launchers = null;

        [Header("GrazeCheck")]
        [SerializeField]
        private float m_EngryAddPerGraze = 0.1f;
        [SerializeField]
        private float m_GrazeCheckTiming = 0.1f;
        [SerializeField]
        private float m_GrazeCheckRadius = 2f;
        [SerializeField]
        private LayerMask m_GrazeCheckLayer = 0;

        #region Coroutine Value
        private Coroutine grazeCheckRoutine = null;
        #endregion

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

            //  Start graze check
            grazeCheckRoutine = StartCoroutine(GrazeChecking());
        }

        private void Update()
        {
            if (m_FireAction.IsPressed())
            {
                Fire();
            }

            var moveValue = m_MoveAction.ReadValue<Vector2>();
            Move(moveValue.x, moveValue.y, isFocusMode);
        }

        /// <summary>
        /// Call all launcher fire(if have)
        /// </summary>
        private void Fire()
        {
            int launcherCount = m_Launchers.Length;
            for (int index = 0; index < launcherCount; ++index)
            {
                var launcher = m_Launchers[index];
                if (!launcher.IsWorking)
                {
                    launcher.AwakeLauncher();
                }
                launcher.Fire();
            }
        }

        private void Move(float hValue, float vValue, bool focusMode)
        {
            var moveValue = Vector2.zero;
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

            //  Boder Check
            BoderCheck(m_MoveTarget.transform.localPosition);
        }

        private void BoderCheck(Vector3 currentPos)
        {
            var adjustPos = currentPos;
            var upHit = Physics2D.Raycast(currentPos,this.transform.up, m_BorderCheckUp, m_BorderLayer);
            if (upHit.collider != null)
            {
                var hitPoint = upHit.point;
                adjustPos.y = hitPoint.y - m_BorderCheckUp;
            } else
            {
                var downHit = Physics2D.Raycast(currentPos, -this.transform.up, m_BorderCheckDown, m_BorderLayer);
                if (downHit.collider != null)
                {
                    var hitPoint = downHit.point;
                    adjustPos.y = hitPoint.y + m_BorderCheckDown;
                }
            }

            var leftHit = Physics2D.Raycast(currentPos, -this.transform.right, m_BorderCheckLeft, m_BorderLayer);
            if (leftHit.collider != null)
            {
                var hitPoint = leftHit.point;
                adjustPos.x = hitPoint.x + m_BorderCheckLeft;
            } else
            {
                var rightHit = Physics2D.Raycast(currentPos, this.transform.right, m_BorderCheckRight, m_BorderLayer);
                if (rightHit.collider != null)
                {
                    var hitPoint = rightHit.point;
                    adjustPos.x = hitPoint.x - m_BorderCheckRight;
                }
            }

            // Final pos
            m_MoveTarget.transform.localPosition = adjustPos;
        }

        private IEnumerator GrazeChecking()
        {
            var checkDistence = new WaitForSeconds(m_GrazeCheckTiming);

            while(true)
            {
                yield return checkDistence;
                var currentPos = this.transform.localPosition;
                var checkTargets = Physics2D.OverlapCircleAll(currentPos, m_GrazeCheckRadius, m_GrazeCheckLayer);
                var findCount = checkTargets.Length;
                GrazeCounter += findCount * m_EngryAddPerGraze;
            }
        }

        private void OnDrawGizmos()
        {
            var centerPos = this.transform.localPosition;
            Gizmos.color = Color.green;
            //  Border check Up
            Gizmos.DrawLine(centerPos, centerPos + Vector3.up * m_BorderCheckUp);
            //  Border check Down
            Gizmos.DrawLine(centerPos, centerPos + Vector3.down * m_BorderCheckDown);
            //  Border check Left
            Gizmos.DrawLine(centerPos, centerPos + Vector3.left * m_BorderCheckLeft);
            //  Border check Right
            Gizmos.DrawLine(centerPos, centerPos + Vector3.right * m_BorderCheckRight);

            //  Draw graze field
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(centerPos, m_GrazeCheckRadius);
        }
    }
}
