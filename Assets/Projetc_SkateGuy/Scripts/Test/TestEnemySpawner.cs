
using UnityEngine;
using SkateGuy.GameElements;
using SkateGuy.GameElements.Factory;

namespace SkateGuy.Test
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

        // Update is called once per frame
        void Update()
        {

        }
    }
}
