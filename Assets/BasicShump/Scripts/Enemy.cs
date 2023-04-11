using System.Collections;
using UnityEngine;

namespace ShumpCore
{
    public class Enemy : MonoBehaviour, IDamageable, IRecycleable
    {
        [SerializeField]
        private SpriteRenderer m_RenderTarget = null;
        [SerializeField]
        private float m_Hp = 100;
        [SerializeField]
        private float hp = 0;
        private float HP
        {
            get { return hp; }
            set
            {
                hp = value;
                if (hp <= 0)
                {
                    EnemyDead();
                } else
                {
                    if (damagedFlash == null)
                    {
                        damagedFlash = StartCoroutine(DamagedFlashing());
                    }
                }
            }
        }

        private Coroutine damagedFlash = null;

        // Start is called before the first frame update
        void Start()
        {
            ReSetData();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ReSetData()
        {
            hp = m_Hp;
        }

        public  void EnemyDead()
        {
            this.gameObject.SetActive(false);
        }

        public void GetHit(float dmg)
        {
            HP -= dmg;
        }

        public void Recycle()
        {
            this.gameObject.SetActive(false);
        }

        IEnumerator DamagedFlashing()
        {
            m_RenderTarget.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            m_RenderTarget.color = Color.white;
            damagedFlash = null;
        }
    }
}
