using UnityEngine;

namespace SkateGuy.GameElements.EnemyGroup
{
    [System.Serializable]
    public class EnemyTeamMemberData
    {
        [SerializeField]
        private Enemy m_EnemyPrefab = null;
        public Enemy EnemyPrefab
        {
            get { return m_EnemyPrefab ; }
        }
        [SerializeField]
        private Vector2 m_SetPosition = Vector2.zero;
        public Vector2 SetPosition
        {
            get { return m_SetPosition; }
        }
    }
}
