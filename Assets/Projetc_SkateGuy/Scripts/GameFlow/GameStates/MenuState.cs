using SkateGuy.UIs;

namespace SkateGuy.GameFlow.States
{
    public class MenuState : GameState
    {
        private MenuUI _menuUI = null;

        public MenuState(MenuStatePackage menuStatePackage)
        {
            NextState = menuStatePackage.StateOnGameStart;
            _menuUI = menuStatePackage.MenuUI;
        }

        public override void OnEnter()
        {
            if (!_menuUI.IsInitialize)
            {
                _menuUI.Initialize();
            }
            _menuUI.Open();
        }

        public override void Track(float dt)
        {
            
        }

        public override void OnExit()
        {
            _menuUI.Close();
        }
    }

    public struct MenuStatePackage
    {
        public MenuUI MenuUI;

        public GameState StateOnGameStart;
    }
}
