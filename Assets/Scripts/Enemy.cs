using UnityEngine;

namespace STG
{
    public class Enemy : BasicEnemy
    {
        [SerializeField]
        private TriggerListener hitListener = null;

        private float speed = 5;
        private float lifeTime = 5;
        private float lifeTimeCounter = 0;

        void Start()
        {
            Initialization();
        }

        private void GetHurt(Collider2D collision2D)
        {
            ICollision collision = collision2D.gameObject.GetComponent<ICollision>();
            int dmg = collision.CollisionDNG();

            HP -= dmg;
            if (HP <= 0)
            {
                Die();
            }
        }

        public override void Action(float dt)
        {
            this.transform.Translate(Vector3.left * speed * dt);
            if (lifeTimeCounter >= lifeTime)
            {
                Die();
            } else
            {
                lifeTimeCounter += dt;
            }
        }

        public override void Initialization()
        {
            HP = MaxHP;
            hitListener.AddCollisionEnterEvent(GetHurt);
            lifeTimeCounter = 0;
        }

        public override void SetData(object enemyData)
        {
            BasicEnemyData basicEnemyData = (BasicEnemyData)enemyData;
            if (basicEnemyData != null)
            {
                MaxHP = basicEnemyData.MaxHP;
            }
        }

        public void Die()
        {
            if (eventWhenDie != null)
            {
                eventWhenDie.Invoke();
            }
        }

        public override void PickFromPool()
        {
            Initialization();
        }

        public override void ReleaseToPool()
        {
            hitListener.RemoveCollisionEnterEvent(GetHurt);
        }
    }
}
