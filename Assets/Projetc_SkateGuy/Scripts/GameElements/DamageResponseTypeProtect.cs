using System.Collections;
using UnityEngine;

namespace SkateGuy.GameElements.PlayerPlus
{
    [System.Serializable]
    public class DamageResponseTypeProtect : DamageResponse
    {
        [SerializeField]
        private float m_ProtectDuration = 1.5f;
        private Coroutine protectRoutine = null;

        public override void Install(PlayableObject player)
        {
            base.Install(player);
            targetPlayer.OnPlayerDie.AddListener(OnPlayerDie);
        }

        public override void UnInstall()
        {
            base.UnInstall();
            targetPlayer.OnPlayerDie.RemoveListener(OnPlayerDie);
        }

        public override void OnDamaged(PlayableObject player, float dmg)
        {
            if (targetPlayer.HP <= 0)
            {
                return;
            }
            targetPlayer.Invincible = true;
            protectRoutine = targetPlayer.StartCoroutine(Protecting());
        }

        private IEnumerator Protecting()
        {
            yield return new WaitForSeconds(m_ProtectDuration);
            targetPlayer.Invincible = false;
        }

        private void OnPlayerDie()
        {
            if (protectRoutine != null)
            {
                targetPlayer.StopCoroutine(Protecting());
            }
        }
    }
}
