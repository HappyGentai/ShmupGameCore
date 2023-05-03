using System.Collections.Generic;
using UnityEngine;

namespace ShmupCore
{
    /// <summary>
    /// Storge game's player and enemy info
    /// </summary>
    public class ShmupObserver
    {
        public static ShmupObserver inti = null;

        private PlayableObject player = null;
        private List<Enemy> enemies = null;

        /// <summary>
        /// When create this class, make sure send OnEnemyEnemyCreate/OnEnemyEnemyDie 
        /// to enemy create/die subscribe event, like EnemyFactory.onEnemyGet/onEnemyReturnToPool.
        /// Otherwise ShmupObserver.ClosestEnemy and ShmupObserver.FarthestEnemy
        /// will not effect.
        /// </summary>
        /// <param name="_player"></param>
        public ShmupObserver(PlayableObject _player)
        {
            //  Clear old one
            if (inti != null)
            {
                inti = null; 
            }
            inti = this;
            player = _player;
            enemies = new List<Enemy>();
        }

        public void OnEnemyEnemyCreate(Enemy enemy)
        {
            enemies.Add(enemy);
        }

        public void OnEnemyEnemyDie(Enemy enemy)
        {
            enemies.Remove(enemy);
        }

        public static PlayableObject Player
        {
            get
            {
                if (inti == null)
                {
                    Debug.LogError("ShmupObserver not create yet");
                    return null;
                }
                return inti.player;
            }
        }

        public static Enemy ClosestEnemy
        {
            get {
                if (inti == null)
                {
                    Debug.LogError("ShmupObserver not create yet");
                    return null;
                }

                var player = inti.player;
                var enemies = inti.enemies;
                var enemyCount = enemies.Count;
                Enemy targetEnemy = null;
                float closestDis = -999;
                for (int index = 0; index < enemyCount; ++index)
                {
                    var checkEnemy = enemies[index];
                    var checkDis = Vector2.Distance(player.transform.localPosition,
                            checkEnemy.transform.localPosition);
                    if (targetEnemy == null)
                    {
                        targetEnemy = checkEnemy;
                        closestDis = checkDis;
                    }
                    else if (closestDis > checkDis)
                    {
                        targetEnemy = checkEnemy;
                        closestDis = checkDis;
                    }
                }
                return targetEnemy;
            }
        }

        public static Enemy FarthestEnemy
        {
            get {
                if (inti == null)
                {
                    Debug.LogError("ShmupObserver not create yet");
                    return null;
                }

                var player = inti.player;
                var enemies = inti.enemies;
                var enemyCount = enemies.Count;
                Enemy targetEnemy = null;
                float closestDis = -999;
                for (int index = 0; index < enemyCount; ++index)
                {
                    var checkEnemy = enemies[index];
                    var checkDis = Vector2.Distance(player.transform.localPosition,
                            checkEnemy.transform.localPosition);
                    if (targetEnemy == null)
                    {
                        targetEnemy = checkEnemy;
                        closestDis = checkDis;
                    }
                    else if (closestDis < checkDis)
                    {
                        targetEnemy = checkEnemy;
                        closestDis = checkDis;
                    }
                }
                return targetEnemy;
            }
        }
    }
}
