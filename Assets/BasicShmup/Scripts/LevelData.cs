using UnityEngine;

namespace ShmupCore.Level
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ShmupCore/Stagge/LevelData")]
    public class LevelData : ScriptableObject
    {
        [SerializeField]
        private SpawnData[] m_SpawnDatas = null;
        public SpawnData[] SpawnDatas
        {
            get { return m_SpawnDatas; }
        }
    }
    
    [System.Serializable]
    public class SpawnData
    {
        [SerializeField]
        private string m_ID = "";
        public string ID
        {
            get { return m_ID; }
        }

        [SerializeField]
        private int m_LaneNumber = 0;
        public int LaneNumber
        {
            get { return m_LaneNumber; }
        }

        [SerializeField]
        private float m_SpawnTiming = 0;
        public float SpawnTiming
        {
            get { return m_SpawnTiming; }
        }
    }
}
