using UnityEngine;

namespace ShmupCore.Field
{
    /// <summary>
    /// Any object which have collider(2D) enter this area will set value-Invincible to true
    /// which object have interface-IInvincible,
    /// also when exit this area, will set value-Invincible to false.
    /// </summary>
    public class InvincibleArea : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var invincibleObject = collision.gameObject.GetComponent<IInvincible>();
            if (invincibleObject != null)
            {
                invincibleObject.SetInvincible(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var invincibleObject = collision.gameObject.GetComponent<IInvincible>();
            if (invincibleObject != null)
            {
                invincibleObject.SetInvincible(false);
            }
        }
    }
}
