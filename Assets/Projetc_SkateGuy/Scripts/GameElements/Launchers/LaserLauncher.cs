using UnityEngine;
using UnityEngine.Events;
using SkateGuy.Factories;

namespace SkateGuy.GameElements
{

    public class LaserLauncher : Launcher
    {
        [SerializeField]
        private UnityEvent m_OnStopFire = new UnityEvent();
        private Laser[] lasers = null;

        public override void StartTrigger()
        {
            m_OnFiring?.Invoke();
            //  Start shoot laser
            int spotCount = m_FireSpots.Length;
            var selfPos = this.transform.position;
            lasers = new Laser[spotCount];
            for (int index = 0; index < spotCount; ++index)
            {
                var fireSpot = m_FireSpots[index];
                var firDir = Quaternion.AngleAxis(fireSpot.FireAngle, Vector3.forward) * m_BaseFireDirection;
                var laser = BulletFactory.GetBullet(fireSpot.FireBullet);
                laser.m_BulletBelong = m_LauncherBelong;
                laser.MoveDir = firDir;
                laser.transform.parent = this.transform;
                laser.transform.localPosition = Vector3.zero + (Vector3)fireSpot.FirePoint;
                laser.WakeUpBullet();
                lasers[index] = (Laser)laser;
            }
        }

        public override void HoldTrigger()
        {
            
        }

        public override void ReleaseTrigger()
        {
            if (lasers == null)
            {
                return;
            }
            m_OnStopFire?.Invoke();
            //  stop shoot laser
            var laserCount = lasers.Length;
            for (int index = 0; index < laserCount; ++index)
            {
                var laser = lasers[index];
                laser.transform.parent = null;
                laser.StopLaser();
            }
        }

        public override void StopLauncher()
        {
            base.StopLauncher();
            ReleaseTrigger();
        }
    }
}
