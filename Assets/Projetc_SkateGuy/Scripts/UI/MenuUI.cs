using UnityEngine;
using SkateGuy.GameFlow;

namespace SkateGuy.UIs
{
    public class MenuUI : BasicUI
    {
        [SerializeField]
        private GameSettingUI m_GameSettingUIPrefab = null;
        private GameSettingUI _GameSettingUI = null;
        public GameFlow.States.GameState m_StateWhenGameStart = null;

        protected override void DoInitialize()
        {
            _GameSettingUI = Instantiate<GameSettingUI>(m_GameSettingUIPrefab);
            _GameSettingUI.Initialize();
            _GameSettingUI.Close();
        }

        #region For ui button on click event.
        public void StartGame()
        {
            GameController.ChangeState(m_StateWhenGameStart);
        }

        public void OpenSettingPage()
        {
            _GameSettingUI.Open();
        }

        public void CloseGame()
        {
            Application.Quit();
        }
        #endregion

        public override void Close()
        {
            _GameSettingUI.Close();
            base.Close();
        }
    }
}
