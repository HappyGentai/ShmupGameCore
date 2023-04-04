using UnityEngine;

namespace TestCode
{
    public class AutoRotate : MonoBehaviour
    {
        [SerializeField]
        private float m_RotateSpeed = 1;

        // Update is called once per frame
        void Update()
        {
            Vector3 angle = this.transform.eulerAngles;
            angle.z += m_RotateSpeed * Time.deltaTime;
            this.transform.eulerAngles = angle;
        }
    }
}
