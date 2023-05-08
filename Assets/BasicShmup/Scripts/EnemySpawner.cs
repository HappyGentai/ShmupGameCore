using UnityEngine;
using ShmupCore.GameElement;
using ShmupCore.Factory;

namespace ShmupCore
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        private Vector2 m_CenterPos = Vector3.zero;
        [SerializeField]
        private int m_LanePairCount = 0;
        public int LanePairCount
        {
            get { return m_LanePairCount; }
        }
        [SerializeField]
        private float m_LaneDistance = 0;
        [SerializeField]
        private LaneExpandDirectionm m_LaneExpandDir = LaneExpandDirectionm.Horizontal;

        public Enemy SpawnEnemy(Enemy enemyPrefab, int laneIndex)
        {
            var enemy = EnemyFactory.GetEnemy(enemyPrefab);
            enemy.transform.localPosition = GetLanePosition(laneIndex);
            return enemy;
        }

        private Vector2 GetLanePosition(int laneIndex)
        {
            var centerPos = m_CenterPos;
            if (laneIndex == 0)
            {
                return centerPos;
            } else
            {
                if (Mathf.Abs(laneIndex) > m_LanePairCount)
                {
                    return Vector2.zero;
                }

                var expandDir = centerPos;
                if (m_LaneExpandDir == LaneExpandDirectionm.Horizontal)
                {
                    expandDir.y = 0;
                    expandDir.x = laneIndex * m_LaneDistance;
                    return centerPos + expandDir;
                } else
                {
                    expandDir.x = 0;
                    expandDir.y = laneIndex * m_LaneDistance;
                    return centerPos + expandDir;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            int totalLanes = (m_LanePairCount * 2)+1;
            int oddNoIndex = 1;
            int evenNoIndex = -1;
            Gizmos.color = Color.green;
            for (int index = 0; index < totalLanes; ++index)
            {
                var pos = Vector2.zero;
                if (index == 0)
                {
                    pos = GetLanePosition(index);
                } else
                {
                    if (index%2 == 1)
                    {
                        pos = GetLanePosition(oddNoIndex);
                        oddNoIndex++;
                    } else
                    {
                        pos = GetLanePosition(evenNoIndex);
                        evenNoIndex--;
                    }
                }
                Gizmos.DrawSphere(pos, 0.5f);
            }
        }
    }

    public enum LaneExpandDirectionm
    {
        Horizontal = 0,
        Vertical = 1,
    }
}
