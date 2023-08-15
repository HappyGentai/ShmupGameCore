using System.Collections.Generic;
using UnityEngine;

namespace GrazerCore.Tool
{
    public class DuplicateCharCheck : MonoBehaviour
    {
        [SerializeField]
        private TextAsset m_CheckText = null;

        // Start is called before the first frame update
        void Start()
        {
            DuplicateCheck();
        }

        private void DuplicateCheck()
        {
            var charArray = m_CheckText.text.ToCharArray();
            var charCount = charArray.Length;
            var duplicateChar = new List<char>();
            for (int index = 0; index < charCount; ++index)
            {
                var _char = charArray[index];
                for (int checkIndex = index+1; checkIndex < charCount; ++checkIndex)
                {
                    var checkChar = charArray[checkIndex];
                    if (_char == checkChar)
                    {
                        if (!duplicateChar.Contains(_char))
                        {
                            duplicateChar.Add(_char);
                        }
                        break;
                    }
                }
            }

            var duplicateCount = duplicateChar.Count;
            Debug.Log("Duplicate count: "+ duplicateCount);
            for (int index = 0; index < duplicateCount; ++index)
            {
                Debug.Log(duplicateChar[index]);
            }
        }
    }
}
