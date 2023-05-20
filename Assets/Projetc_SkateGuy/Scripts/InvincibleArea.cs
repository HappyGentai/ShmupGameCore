using UnityEngine;

namespace SkateGuy.Fields
{
    /// <summary>
    /// Any object which have collider(2D) enter this area will set value-Invincible to true
    /// which object have interface-IInvincible,
    /// also when exit this area, will set value-Invincible to false.
    /// </summary>
    public class InvincibleArea : MonoBehaviour
    {
        [SerializeField]
        private LayerMask m_TargetMask = 0;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & m_TargetMask) == 0)
            {
                return;
            }
            var invincibleObject = collision.gameObject.GetComponent<IInvincible>();
            if (invincibleObject != null)
            {
                invincibleObject.SetInvincible(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & m_TargetMask) == 0)
            {
                return;
            }
            var invincibleObject = collision.gameObject.GetComponent<IInvincible>();
            if (invincibleObject != null)
            {
                invincibleObject.SetInvincible(false);
            }
        }
    }
}