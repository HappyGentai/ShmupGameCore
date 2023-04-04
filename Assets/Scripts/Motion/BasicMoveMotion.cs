using UnityEngine;

namespace Motion.Move
{
    public class BasicMoveMotion
    {
        protected Vector3 _MoveDirection = Vector3.zero;
        public Vector3 MoveDirection
        {
            get { return _MoveDirection; }
        }

        protected float _Force = 0;
        public float Force
        {
            get { return _Force; }
        }

        public virtual Vector3 Move(Vector3 currentPos, float dt)
        {
            Vector3 newMovePoint = currentPos + MoveDirection * dt * _Force;

            return newMovePoint;
        }
    }
}
