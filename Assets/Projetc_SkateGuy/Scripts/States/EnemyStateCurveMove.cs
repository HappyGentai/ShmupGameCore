using UnityEngine;
using SkateGuy.GameElements;
using SkateGuy.Tool;

namespace SkateGuy.States.EnemyStates
{
    public class EnemyStateCurveMove : BasicState
    {
        private Enemy enemy = null;
        private Transform moveTarget = null;
        private Vector2 startPos = Vector2.zero;
        private Vector2 endPos = Vector2.zero;
        private Vector2 aidPosA = Vector2.zero;
        private Vector2 aidPosB = Vector2.zero;
        private float speedScale = 0;
        private float t = 0;
        private bool fireWhenMove = false;
        private Launcher[] launchers = null;

        public EnemyStateCurveMove(StateController _stateController, Enemy _enemy,
            Vector2 _startPos, Vector2 _endPos, Vector2 _aidPosA, Vector2 _aidPosB,
            float _SpeedScale, bool _fireWhenMove) : base(_stateController)
        {
            stateController = _stateController;
            enemy = _enemy;
            moveTarget = enemy.MoveTarget;
            startPos = _startPos;
            endPos = _endPos;
            aidPosA = _aidPosA;
            aidPosB = _aidPosB;
            speedScale = _SpeedScale;
            fireWhenMove = _fireWhenMove;
            launchers = enemy.Launchers;
        }

        public override void OnEnter()
        {
            if (fireWhenMove)
            {
                var launcherCount = launchers.Length;
                for (int index = 0; index < launcherCount; ++index)
                {
                    var launcher = launchers[index];
                    launcher.AwakeLauncher();
                }
            }
        }

        public override void OnExit()
        {
            if (fireWhenMove)
            {
                var launcherCount = launchers.Length;
                for (int index = 0; index < launcherCount; ++index)
                {
                    var launcher = launchers[index];
                    launcher.StopLauncher();
                }
            }
        }

        public override void Track()
        {
            if (fireWhenMove)
            {
                var launcherCount = launchers.Length;
                for (int index = 0; index < launcherCount; ++index)
                {
                    var launcher = launchers[index];
                    launcher.Fire();
                }
            }

            t += Time.deltaTime * speedScale;
            if (t >= 1)
            {
                moveTarget.localPosition = LineLerp.CubicLerp(startPos, aidPosA, aidPosB, endPos, 1);
                SetToNextState();
            } else
            {
                moveTarget.localPosition = LineLerp.CubicLerp(startPos, aidPosA, aidPosB, endPos, t);
            } 
        }
    }
}
