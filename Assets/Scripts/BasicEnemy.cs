using UnityEngine;

namespace STG
{
    public abstract class BasicEnemy : MonoBehaviour, IPoolObject
    {
        private float _MaxHP = 0;
        public float MaxHP
        {
            get { return _MaxHP; }
            protected set { _MaxHP = value; }
        }

        private float _HP = 0;
        public float HP
        {
            get { return _HP; }
            protected set { _HP = value; }
        }

        public virtual void Update()
        {
            Action(Time.deltaTime);
        }

        public UnityEngine.Events.UnityAction eventWhenDie = null;

        public abstract void Initialization();

        public abstract void Action(float dt);

        public abstract void SetData(object enemyData);

        public abstract void PickFromPool();

        public abstract void ReleaseToPool();
    }
}
