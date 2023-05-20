using UnityEngine;
using SkateGuy.GameElements;

namespace SkateGuy.Test
{
    public class TestGameCore : MonoBehaviour
    {
        [SerializeField]
        private PlayableObject m_Player = null;
        [SerializeField]
        private TestPlayerUI m_PlayerUI = null;

        void Start()
        {
            m_Player.Initialization();
            m_PlayerUI.Initialization();
        }
    }
}
