using UnityEngine;

namespace ShmupCore
{
    [CreateAssetMenu(fileName = "EnemyLibrary", menuName = "ShmupCore/Stagge/EnemyLibrary")]
    public class EnemyLibrary : ScriptableObject
    {
        [SerializeField]
        private EnemyData[] m_EnemyDatas = null;

        public Enemy GetEnemyPrefab(string id)
        {
            int dataCount = m_EnemyDatas.Length;
            for (int index = 0; index < dataCount; ++index)
            {
                var enemyData = m_EnemyDatas[index];
                if (enemyData.ID == id)
                {
                    return enemyData.Prefab;
                }
            }
            return null;
        }
    }
}
