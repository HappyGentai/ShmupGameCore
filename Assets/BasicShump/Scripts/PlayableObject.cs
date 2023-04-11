using UnityEngine;

namespace ShumpCore
{
    public class PlayableObject : MonoBehaviour
    {
        [SerializeField]
        private Transform m_MoveTarget = null;

        [SerializeField]
        private float m_MoveSpeed = 3f;

        [SerializeField]
        [Range(0, 1f)]
        private float m_FocusModeScaleRate = 0.6f;

        [SerializeField]
        private Launcher[] m_Launchers = null;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Fire();
            }

            Vector2 moveValue = Vector2.zero;
            if (Input.GetKey(KeyCode.W))
            {
                moveValue.y = 1;
            } else if (Input.GetKey(KeyCode.S))
            {
                moveValue.y = -1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                moveValue.x = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                moveValue.x = 1;
            }

            bool isFocusMode = false;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isFocusMode = true;
            }

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
                Launcher launcher = m_Launchers[index];
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
