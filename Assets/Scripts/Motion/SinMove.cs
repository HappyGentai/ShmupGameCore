using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Motion.Move
{
    public class SinMove: BasicMoveMotion
    {
        private float _sinCounter = 0;

        public override Vector3 Move(Vector3 currentPos, float dt)
        {
            //_sinCounter += dt;
            //Vector3 newPos = currentPos + Mathf.Sin()

            return base.Move(currentPos, dt);
        }
    }
}
