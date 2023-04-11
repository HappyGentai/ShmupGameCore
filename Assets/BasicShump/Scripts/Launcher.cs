using System.Collections;
using UnityEngine;

namespace ShumpCore
{
    public class Launcher : MonoBehaviour
    {
        public Bullet m_FireBullet = null;
        [SerializeField]
        private float m_FireRecall = 0.1f;

        private float fireCounter = 0;
        private Coroutine workingRoutine = null;

        public void AwakeLauncher()
        {
            StopLauncher();
            fireCounter = m_FireRecall;
            workingRoutine = StartCoroutine(LauncherWorking());
        }

        public void StopLauncher()
        {
            if (workingRoutine != null)
            {
                StopCoroutine(workingRoutine);
                workingRoutine = null;
            }
        }

        public void Fire()
        {
            if (workingRoutine == null)
            {
                AwakeLauncher();
            }

            if (fireCounter >= m_FireRecall)
            {
                fireCounter = 0;
                Bullet bullet = BulletFactory.GetBullet(m_FireBullet);
                bullet.MoveDir = this.transform.right;
                bullet.transform.position = this.transform.position; 
            }
        }

        private IEnumerator LauncherWorking()
        {
            while(true)
            {
                yield return null;

                if (fireCounter < m_FireRecall)
                {
                    fireCounter += Time.deltaTime;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 0, 0, 0.25f);
            Gizmos.DrawSphere(this.transform.position, 0.5f);
        }
    }
}
