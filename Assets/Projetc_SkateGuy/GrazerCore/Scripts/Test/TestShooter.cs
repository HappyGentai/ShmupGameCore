using UnityEngine;
using Gental14.GrazerCore.GameElements;

namespace Gental14.GrazerCore.Test
{
    public class TestShooter : MonoBehaviour
    {
        [SerializeField]
        private Launcher[] m_Launchers = null;

        void Update()
        {
            int launcherCount = m_Launchers.Length;
            for (int index = 0; index < launcherCount; ++index)
            {
                Launcher launcher = m_Launchers[index];
                if (!launcher.IsWorking)
                {
                    launcher.AwakeLauncher();
                }
                launcher.HoldTrigger();
            }
        }
    }
}
