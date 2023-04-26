using UnityEngine;

namespace ShmupCore
{
    [System.Serializable]
    public class EnemyData
    {
        [SerializeField]
        private string m_ID = "";
        public string ID
        {
            get { return m_ID; }
        }

        [SerializeField]
        private Enemy m_Prefab = null;
        public Enemy Prefab
        {
            get { return m_Prefab; }
        }
    }
}
