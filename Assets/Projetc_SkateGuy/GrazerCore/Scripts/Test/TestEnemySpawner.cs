
using UnityEngine;
using Gental14.GrazerCore.GameElements;
using Gental14.GrazerCore.Factories;

namespace Gental14.GrazerCore.Test
{
    public class TestEnemySpawner : MonoBehaviour
    {
        [SerializeField]
        private Enemy m_EnemyPrefab = null;

        // Start is called before the first frame update
        void Start()
        {
            EnemyFactory.GetEnemy(m_EnemyPrefab);
        }
    }
}
