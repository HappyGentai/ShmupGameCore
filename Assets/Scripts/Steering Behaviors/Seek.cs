using UnityEngine;

namespace SteeringBehaviors
{
    public class Seek : MonoBehaviour
    {
        [SerializeField]
        private Transform m_Target = null;
        [SerializeField]
        private float m_MaxSpeed = 3;
        [SerializeField]
        private float m_MaxForce = 2;
        [SerializeField]
        private float m_Mass = 1;

        private Vector3 velocity = Vector3.zero;
        private Vector3 desiredVelocity = Vector3.zero;
        private bool move = false;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                move = true;
                velocity = Vector3.up * m_MaxSpeed;
            }

            if (move)
            {
                /*
                *  研究資料:
                *  https://gamedevelopment.tutsplus.com/tutorials/understanding-steering-behaviors-seek--gamedev-849?_ga=2.21687539.912125127.1675784210-1739809873.1675784210
                *  https://www.youtube.com/watch?v=4zhJlkGQTvU
                *  
                *  研究感想:
                *  基本上照抄會有些難理解的功能，簡單來說就是追蹤目標
                *  由於目標有可能會進行移動導致原本計算的移動向量不會到達目標點，因此再利用自身及目標的位置推算出新的向量(desiredVelocity)
                *  並把新向量與當前向量相減得到新的牽引向量(steering)，牽引向量再經過限速以及重量權重(可用可不用)後與當前向量相加並經過限速
                *  將會成為全新的向量。
                *  
                *  為何要限速?
                *  因為此設計將會把當前向量與牽引向量相結合，利用此項量計算出來的移動速度很有可能超出原始設計移動速度(畢竟方向最終會重和)
                *  因此需要多個參數來進行限速。
                */
                desiredVelocity = Vector3.Normalize(m_Target.position - this.transform.position) * m_MaxSpeed;
                Vector3 steering = desiredVelocity - velocity;

                steering = Truncate(steering, m_MaxForce);
                steering = steering / m_Mass;
                Debug.Log((velocity + steering).magnitude +" "+ m_MaxSpeed);
                velocity = Truncate(velocity + steering, m_MaxSpeed);
                this.transform.position += velocity * Time.deltaTime;
            }

            //this.transform.position += (m_Target.position - this.transform.position) * m_MaxVelocity * Time.deltaTime;
        }

        private Vector3 Truncate(Vector3 vel, float maxLength)
        {
            return Vector3.ClampMagnitude(vel, maxLength);
        }
    }
}
