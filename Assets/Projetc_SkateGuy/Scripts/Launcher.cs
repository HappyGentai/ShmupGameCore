using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using SkateGuy.Factories;

namespace SkateGuy.GameElements
{
    public class Launcher : MonoBehaviour
    {
        public Bullet m_FireBullet = null;
        [SerializeField]
        private string m_LauncherBelong = "";
        [SerializeField]
        private Vector3 m_BaseFireDirection = Vector3.right;
        [SerializeField]
        private FireSpot[] m_FireSpots = null;
        [SerializeField]
        private float m_FireRecall = 0.1f;
        [SerializeField]
        private UnityEvent m_OnFiring = null;

        private float fireCounter = 0;
        private Coroutine workingRoutine = null;

        private bool isWorking = false;
        public bool IsWorking
        {
            get { return isWorking; }
        }

        private bool launcherLock = false;
        public bool LauncherLock
        {
            set
            {
                launcherLock = value;
            }
        }

        public void AwakeLauncher()
        {
            StopLauncher();
            fireCounter = m_FireRecall;
            workingRoutine = StartCoroutine(LauncherWorking());
            isWorking = true;
            launcherLock = false;
        }

        public void StopLauncher()
        {
            if (workingRoutine != null)
            {
                StopCoroutine(workingRoutine);
                workingRoutine = null;
            }
            isWorking = false;
        }

        public void Fire()
        {
            if (fireCounter >= m_FireRecall && !launcherLock)
            {
                if (m_OnFiring != null)
                {
                    m_OnFiring.Invoke();
                }
                fireCounter = 0;
                int spotCount = m_FireSpots.Length;
                var selfPos = this.transform.position;
                for (int index = 0; index < spotCount; ++index)
                {
                    var fireSpot = m_FireSpots[index];
                    var firDir = Quaternion.AngleAxis(fireSpot.FireAngle, Vector3.forward) * m_BaseFireDirection;
                    var bullet = BulletFactory.GetBullet(m_FireBullet);
                    bullet.m_BulletBelong = m_LauncherBelong;
                    bullet.MoveDir = firDir;
                    bullet.transform.position = this.transform.position + (Vector3)fireSpot.FirePoint;
                }
            }
        }

        private IEnumerator LauncherWorking()
        {
            while (true)
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
            Gizmos.DrawSphere(this.transform.position, 0.25f);

            //  Draw fire spot(If have)
            var selfPos = this.transform.position;
            Gizmos.color = Color.green;
            int spotCount = m_FireSpots.Length;
            for (int index = 0; index < spotCount; ++index)
            {
                var fireSpot = m_FireSpots[index];
                var firePoint = selfPos + (Vector3)fireSpot.FirePoint;
                var firDir = Quaternion.AngleAxis(fireSpot.FireAngle, Vector3.forward) * m_BaseFireDirection;
                Gizmos.DrawLine(firePoint, firePoint + firDir * 5);
            }
        }
    }

    [System.Serializable]
    public class FireSpot
    {
        [SerializeField]
        private Vector2 m_FirePoint = Vector2.zero;
        public Vector2 FirePoint
        {
            get { return m_FirePoint; }
        }
        [SerializeField]
        private float m_FireAngle = 0;
        public float FireAngle
        {
            get { return m_FireAngle; }
        }
    }
}
