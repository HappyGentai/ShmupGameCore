using UnityEngine;
using Assets.SimpleLocalization;

namespace SkateGuy.Tool
{
    public class LocalizationController : MonoBehaviour
    {
        private static LocalizationController Inti;

        [SerializeField]
        private LanguageType m_StartLanguage = LanguageType.ENGLISH;
        private LanguageType currentLanguage = LanguageType.ENGLISH;
        public static LanguageType Language
        {
            get { return Inti.currentLanguage; }
        }
        [SerializeField]
        private TextAsset[] m_LocalizationFiles = null;

        private void Awake()
        {
            if (Inti == null)
            {
                Inti = this;
            } else if (Inti != null)
            {
                Destroy(this.gameObject);
            }

            LocalizationManager.Read(m_LocalizationFiles);
            currentLanguage = m_StartLanguage;
            LocalizationManager.Language = GetLanguageName(currentLanguage);
        }

        public static void SetLanguage(LanguageType type)
        {
            Inti.currentLanguage = type;
            LocalizationManager.Language = Inti.GetLanguageName(Inti.currentLanguage);
        }

        private string GetLanguageName(LanguageType languageType)
        {
            switch(languageType)
            {
                case LanguageType.TRADITIONALCHINESE:
                    return "TraditionalChinese";
                case LanguageType.ENGLISH:
                default:
                    return "English";
            }
        }
    }

    public enum LanguageType
    {
        ENGLISH,
        TRADITIONALCHINESE
    }
}
