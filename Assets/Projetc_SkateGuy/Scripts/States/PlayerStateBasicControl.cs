using UnityEngine;
using SkateGuy.GameElements;
using UnityEngine.InputSystem;

namespace SkateGuy.States.PlayerStates
{
    public class PlayerStateBasicControl: BasicState
    {
        private PlayableObject player = null;
        private Transform moveTarget = null;
        private InputAction FireAction = null;
        private InputAction MoveAction = null;
        private Launcher[] launchers = null;
        private Coroutine grazeCheckRoutine = null;

        public PlayerStateBasicControl(StateController _stateController, PlayableObject _Player) : base(_stateController)
        {
            stateController = _stateController;
            player = _Player;
            moveTarget = player.transform;
            FireAction = player.FireAction;
            MoveAction = player.MoveAction;
            launchers = player.Launchers;
        }

        public override void OnEnter()
        {
            //  Call launches awake
            int launcherCount = launchers.Length;
            for (int index = 0; index < launcherCount; ++index)
            {
                var launcher = launchers[index];
                launcher.AwakeLauncher();
            }
            //  Start graze check
            grazeCheckRoutine = player.StartCoroutine(player.GrazeChecking());
        }

        public override void OnExit()
        {
            //  Call launches stop
            int launcherCount = launchers.Length;
            for (int index = 0; index < launcherCount; ++index)
            {
                var launcher = launchers[index];
                launcher.StopLauncher();
            }
            //  Stop graze check
            player.StopCoroutine(grazeCheckRoutine);
        }

        public override void Track()
        {
            if (FireAction.IsPressed())
            {
                Fire();
            }

            var moveValue = MoveAction.ReadValue<Vector2>();
            Move(moveValue.x, moveValue.y, player.IsFocusMode);
        }


        /// <summary>
        /// Call all launcher fire(if have)
        /// </summary>
        private void Fire()
        {
            int launcherCount = launchers.Length;
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

        private void Move(float hValue, float vValue, bool focusMode)
        {
            var moveValue = Vector2.zero;
            moveValue.x = hValue;
            moveValue.y = vValue;

            //  Normalize move value
            moveValue = moveValue.normalized;
            moveValue *= player.MoveSpeed;
            if (focusMode)
            {
                moveValue *= player.FocusModeScaleRate;
            }

            moveTarget.localPosition += (Vector3)moveValue * Time.deltaTime;

            //  Boder Check
            player.BoderCheck(moveTarget.transform.localPosition);
        }
    }
}
