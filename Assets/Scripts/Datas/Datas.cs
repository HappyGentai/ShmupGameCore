using System.Collections;
using UnityEngine;

namespace STG
{
    [System.Serializable]
    public class EnemyStorageData
    {
        [SerializeField]
        private string m_ID = "";
        public string ID
        {
            get { return m_ID; }
        }

        [SerializeField]
        private GameObject m_EnemyPrefab = null;
        public GameObject EnemyPrefab
        {
            get { return m_EnemyPrefab; }
        }
    }

    [System.Serializable]
    public class BasicEnemyData
    {   
        public BasicEnemyData(float maxHp)
        {
            m_MaxHP = maxHp;
        }
        [SerializeField]
        private float m_MaxHP = 0;
        public float MaxHP
        {
            get { return m_MaxHP; }
        }
    }
}