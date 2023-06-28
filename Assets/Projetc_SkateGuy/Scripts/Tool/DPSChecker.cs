using UnityEngine;
using SkateGuy.GameElements;

namespace SkateGuy.Tool
{
    public class DPSChecker : MonoBehaviour
    {
        [SerializeField]
        private Enemy m_Target = null;

        private bool counting = false;
        [SerializeField]
        private float totalDamage = 0;
        [SerializeField]
        private float totalTime = 0;
        [SerializeField]
        private float DPS = 0;
        
        void Start()
        {
            m_Target.OnGetDamaged.AddListener(DamageEvent);
        }

        private void Update()
        {
            if (counting)
            {
                totalTime += Time.deltaTime;
            }
            DPS = totalDamage / totalTime;
        }

        private void DamageEvent(float dmg)
        {
            totalDamage += dmg;
            if (!counting)
            {
                counting = true;
            }
            
            if (m_Target.HP <= 0)
            {
                m_Target.OnGetDamaged.RemoveListener(DamageEvent);
                counting = false;
            }
        }
    }
}
