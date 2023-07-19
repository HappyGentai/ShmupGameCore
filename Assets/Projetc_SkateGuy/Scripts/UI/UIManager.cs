using System.Collections.Generic;
using UnityEngine;

namespace SkateGuy.UIs
{
    public class UIManager
    {
        private static List<BasicUI> OpenUis = null;
        private static bool isInitialize = false;

        public static void Initialize()
        {
            if (isInitialize)
            {
                return;
            }
            OpenUis = new List<BasicUI>();
            isInitialize = true;
        }

        public static void AddOpenUI(BasicUI basicUI)
        {
            if (!CheckInitialize())
            {
                return;
            }

            if (!OpenUis.Contains(basicUI))
            {
                OpenUis.Add(basicUI);
            }
        }

        /// <summary>
        /// If only one open ui, then will no function.
        /// </summary>
        public static void RemoveNewestOpenUI()
        {
            if (!CheckInitialize())
            {
                return;
            }

            var uiCount = OpenUis.Count;
            if (uiCount == 0 || uiCount == 1)
            {
                return;
            }
            var newest = OpenUis[uiCount - 1];
            newest.Close();
            OpenUis.Remove(newest);
        }

        private static bool CheckInitialize()
        {
            if (!isInitialize)
            {
                Debug.LogError("UIManager not Initialize yet.");
            }
            return isInitialize;
        }
    }
}
