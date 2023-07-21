using UnityEngine;
using UnityEngine.UI;
using SkateGuy.Tool;

namespace SkateGuy.UIs
{
    public class GameSettingUI : BasicUI
    {
        [SerializeField]
        private ControlRebindingUI m_ControlRebindingUIprefab = null;
        private ControlRebindingUI _ControlRebindingUI = null;
        [SerializeField]
        private AudioVolumeUI m_AudioVolumeUIPrefab = null;
        private AudioVolumeUI _AudioVolumeUI = null;
        [SerializeField]
        private Image m_ENSelectedBG = null;
        [SerializeField]
        private Image m_TCSelectedBG = null;

        protected override void DoInitialize()
        {
            _ControlRebindingUI = Instantiate<ControlRebindingUI>(m_ControlRebindingUIprefab);
            _AudioVolumeUI = Instantiate<AudioVolumeUI>(m_AudioVolumeUIPrefab);
            _ControlRebindingUI.Initialize();
            _AudioVolumeUI.Initialize();
            _ControlRebindingUI.OnUIClose.AddListener(() => {
                SetSelectedGameObject(m_SelectedUIOnOpen);
            });
            _AudioVolumeUI.OnUIClose.AddListener(() => {
                SetSelectedGameObject(m_SelectedUIOnOpen);
            });
            _ControlRebindingUI.Close();
            _AudioVolumeUI.Close();
        }

        public override void Open()
        {
            base.Open();
            var currentLanguage = LocalizationController.Language;
            if (currentLanguage == LanguageType.ENGLISH)
            {
                m_ENSelectedBG.enabled = true;
                m_TCSelectedBG.enabled = false;
            } else if (currentLanguage == LanguageType.TRADITIONALCHINESE)
            {
                m_TCSelectedBG.enabled = true;
                m_ENSelectedBG.enabled = false;
            }
        }

        #region For ui button on click event.
        public void OpenControlRebindPage()
        {
            _ControlRebindingUI.Open();
        }

        public void OpenAudioVolumePage()
        {
            _AudioVolumeUI.Open();
        }

        public void SetLanguageToEN()
        {
            LocalizationController.SetLanguage(LanguageType.ENGLISH);
            m_ENSelectedBG.enabled = true;
            m_TCSelectedBG.enabled = false;
        }

        public void SetLanguageToTC()
        {
            LocalizationController.SetLanguage(LanguageType.TRADITIONALCHINESE);
            m_TCSelectedBG.enabled = true;
            m_ENSelectedBG.enabled = false;
        }
        #endregion

        public override void Close()
        {
            // optional, save setting from audio and control setting UI.
            if (_ControlRebindingUI.IsOpen)
            {
                _ControlRebindingUI.Close();
            }
            if (_AudioVolumeUI.IsOpen)
            {
                _AudioVolumeUI.Close();
            }
            base.Close();
        }
    }
}
