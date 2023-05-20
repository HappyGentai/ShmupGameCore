using UnityEngine;

namespace SkateGuy.Fields
{
    public class ShootLockArea : MonoBehaviour
    {
        [SerializeField]
        private LayerMask m_TargetMask = 0;
        [SerializeField]
        private bool m_Reverse = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & m_TargetMask) == 0)
            {
                return;
            }
            var invincibleObject = collision.gameObject.GetComponent<IShootable>();
            if (invincibleObject != null)
            {
                var lockWeapon = m_Reverse;
                invincibleObject.CanShoot(lockWeapon);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & m_TargetMask) == 0)
            {
                return;
            }
            var invincibleObject = collision.gameObject.GetComponent<IShootable>();
            if (invincibleObject != null)
            {
                var lockWeapon = !m_Reverse;
                invincibleObject.CanShoot(lockWeapon);
            }
        }
    }
}
