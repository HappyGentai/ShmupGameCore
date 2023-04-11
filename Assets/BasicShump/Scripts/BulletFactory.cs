using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace ShumpCore
{
    public class BulletFactory
    {
        private static List<BulletPool> bulletPools = new List<BulletPool>();

        public static Bullet GetBullet(Bullet _CoreBullet)
        {
            //  Search in list
            int poolsCount = bulletPools.Count;
            for (int index = 0; index < poolsCount; ++index)
            {
                BulletPool bulletPool =  bulletPools[index];
                Bullet checkBullet = bulletPool.CoreBullet;
                if (checkBullet == _CoreBullet)
                {
                    return bulletPool.GetBullet();
                }
            }

            //  If no bullet pool in list, create one and take
            BulletPool newBulletPool = new BulletPool(_CoreBullet);
            bulletPools.Add(newBulletPool);
            return newBulletPool.GetBullet();
        }

        public static void DisposeAll()
        {
            int poolsCount = bulletPools.Count;
            for (int index = 0; index < poolsCount; ++index)
            {
                BulletPool bulletPool = bulletPools[index];
                bulletPool.Dispose();
            }
        }
    }

    public class BulletPool
    {
        private Bullet m_CoreBullet = null;
        public Bullet CoreBullet
        {
            get { return m_CoreBullet; }
        }

        private ObjectPool<Bullet> bulletPool = null;

        public BulletPool(Bullet _CoreBullet)
        {
            m_CoreBullet = _CoreBullet;
            bulletPool = new ObjectPool<Bullet>(CreatePoolItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);
        }

        public void Dispose()
        {
            bulletPool.Dispose();
        }

        public Bullet GetBullet()
        {
            Bullet bullet = bulletPool.Get();
            return bullet;
        }

        private Bullet CreatePoolItem()
        {
            Bullet newBullet = GameObject.Instantiate<Bullet>(m_CoreBullet);
            newBullet.eventWhenBulletDead += () =>
            {
                bulletPool.Release(newBullet);
            };
            return newBullet;
        }

        private void OnReturnedToPool(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
        }

        private void OnTakeFromPool(Bullet bullet)
        {
            bullet.gameObject.SetActive(true);
            bullet.MoveDir = Vector2.zero;
        }

        private void OnDestroyPoolObject(Bullet bullet)
        {
            GameObject.Destroy(bullet.gameObject);
        }
    }
}
