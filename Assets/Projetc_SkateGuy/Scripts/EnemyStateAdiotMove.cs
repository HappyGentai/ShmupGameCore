using UnityEngine;
using SkateGuy.Staties;
using SkateGuy.GameElements;

namespace SkateGuy.Staties.EnemyState
{
    public class EnemyStateAdiotMove: BasicState
    {
        private Enemy enemy = null;
        private Transform enemyMoveTarget = null;
        private Vector2 moveDir = Vector2.zero;
        private Launcher[] launchers = null;

        public EnemyStateAdiotMove(StateController _stateController, Enemy _enemy, BasicState _nextState, Vector2 _moveDir) : base(_stateController)
        {
            stateController = _stateController;
            nextState = _nextState;
            enemy = _enemy;
            enemyMoveTarget = enemy.MoveTarget;
            moveDir = _moveDir;
            launchers = enemy.Launchers;
            //  Awake all launcher
            var launcherCount = launchers.Length;
            for (int index = 0; index < launcherCount; ++index)
            {
                var launcher = launchers[index];
                launcher.AwakeLauncher();
            }
        }

        public override void OnEnter()
        {
            
        }

        public override void OnExit()
        {
            var launcherCount = launchers.Length;
            for (int index = 0; index < launcherCount; ++index)
            {
                var launcher = launchers[index];
                launcher.StopLauncher();
            }
        }

        public override void Track()
        {
            var enemySpeed = enemy.MoveSpeed;
            enemyMoveTarget.localPosition += (Vector3)(Time.deltaTime * moveDir * enemySpeed);
            Fire();
        }

        private void Fire()
        {
            var launcherCount = launchers.Length;
            for (int index = 0; index < launcherCount; ++index)
            {
                var launcher = launchers[index];
                if (!launcher.IsWorking)
                {
                    launcher.AwakeLauncher();
                }
                launcher.Fire();
            }
        }
    }
}


public class Hola : BasicState
{
    public Hola(StateController _stateController, float ff) : base(_stateController)
    {

    }

    public override void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public override void Track()
    {
        throw new System.NotImplementedException();
    }
}