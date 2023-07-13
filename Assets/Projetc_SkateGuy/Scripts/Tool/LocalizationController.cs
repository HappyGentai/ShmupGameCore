using UnityEngine;
using Assets.SimpleLocalization;

namespace SkateGuy.Tool
{
    public class LocalizationController : MonoBehaviour
    {
        [SerializeField]
        private LanguageType m_StartLanguage = LanguageType.ENGLISH;
        [SerializeField]
        private TextAsset[] m_LocalizationFiles = null;

        private void Awake()
        {
            LocalizationManager.Read(m_LocalizationFiles);
            LocalizationManager.Language = GetLanguageName(m_StartLanguage);
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
