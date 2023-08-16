using UnityEngine;
using TMPro;
using Assets.SimpleLocalization;

namespace GrazerCore.Tool
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedTextForTextMeshPro : MonoBehaviour
    {
        [SerializeField]
        private string LocalizationKey;

        public void Start()
        {
            Localize();
            LocalizationManager.LocalizationChanged += Localize;
        }

        public void OnDestroy()
        {
            LocalizationManager.LocalizationChanged -= Localize;
        }

        private void Localize()
        {
            GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize(LocalizationKey);
        }
    }
}
