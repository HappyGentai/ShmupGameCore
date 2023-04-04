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
                *  ��s���:
                *  https://gamedevelopment.tutsplus.com/tutorials/understanding-steering-behaviors-seek--gamedev-849?_ga=2.21687539.912125127.1675784210-1739809873.1675784210
                *  https://www.youtube.com/watch?v=4zhJlkGQTvU
                *  
                *  ��s�P�Q:
                *  �򥻤W�ӧ۷|�������z�Ѫ��\��A²��ӻ��N�O�l�ܥؼ�
                *  �ѩ�ؼЦ��i��|�i�沾�ʾɭP�쥻�p�⪺���ʦV�q���|��F�ؼ��I�A�]���A�Q�Φۨ��ΥؼЪ���m����X�s���V�q(desiredVelocity)
                *  �ç�s�V�q�P��e�V�q�۴�o��s���o�ަV�q(steering)�A�o�ަV�q�A�g�L���t�H�έ��q�v��(�i�Υi����)��P��e�V�q�ۥ[�øg�L���t
                *  �N�|�������s���V�q�C
                *  
                *  ����n���t?
                *  �]�����]�p�N�|���e�V�q�P�o�ަV�q�۵��X�A�Q�Φ����q�p��X�Ӫ����ʳt�׫ܦ��i��W�X��l�]�p���ʳt��(������V�̲׷|���M)
                *  �]���ݭn�h�ӰѼƨӶi�歭�t�C
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
