using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    public static BulletFactory Instance { get; private set; }

    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private int _initialAmount;

    private Pool<Bullet> _myBulletsPool;

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _myBulletsPool = new Pool<Bullet>(CreateObject, Bullet.TurnOn, Bullet.TurnOff, _initialAmount);
    }

    Bullet CreateObject()
    {
        return Instantiate(_bulletPrefab);
    }

    public Bullet GetObjectFromPool()
    {
        return _myBulletsPool.GetObject();
    }

    public void ReturnObjectToPool(Bullet bullet)
    {
        _myBulletsPool.ReturnObject(bullet);
    }

}
