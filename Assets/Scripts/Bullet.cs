using UnityEngine;

public class Bullet : MonoBehaviour, ICollision
{
    [SerializeField]
    private float m_BulletSpeed = 5;
    [SerializeField]
    private int m_BulletDmmg = 1;
    private Vector2 flyDirection = Vector2.zero;
    public Vector2 FlyDirection
    {
        get { return flyDirection; }
        set
        {
            flyDirection = value;
            float angle = Mathf.Atan2(flyDirection.y, flyDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }


    private void Update()
    {
        this.transform.position += (Vector3)FlyDirection * m_BulletSpeed * Time.deltaTime;
    }

    public int CollisionDNG()
    {
        return m_BulletDmmg;
    }

    public void CollisionHit(Transform hitTransform)
    {
        Destroy(this.gameObject);
    }
}
