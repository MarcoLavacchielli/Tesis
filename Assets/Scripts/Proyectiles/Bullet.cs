using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody _rgb;
    [SerializeField] private float _initialLifeTime;
    //private float _currentLifeTime;

    private void Awake()
    {
        _rgb = GetComponent<Rigidbody>();
    }

    /*void Update()
    {

        _currentLifeTime -= Time.deltaTime;

        if (_currentLifeTime <= 0)
        {
            BulletFactory.Instance.ReturnObjectToPool(this);
        }
    }*/

    public void returnBullet()
    {
        BulletFactory.Instance.ReturnObjectToPool(this);
    }

    public void AddImpulse(Vector3 dir)
    {
        _rgb.AddForce(dir, ForceMode.Impulse);
    }

    public void Reset()
    {
        _rgb.velocity = Vector3.zero;

        //_currentLifeTime = _initialLifeTime;
    }

    public static void TurnOn(Bullet b)
    {
        b.gameObject.SetActive(true);
    }

    public static void TurnOff(Bullet b)
    {
        b.Reset();
        b.gameObject.SetActive(false);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("camerazone"))
        {
            Debug.Log("mate a la c�mara");
            other.gameObject.SetActive(false);

            returnBullet();
        }
    }
    /*private void OnCollisionEnter(Collision collision)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("camerazone"))
        {
            Debug.Log("mate la c�mara");
            collision.gameObject.SetActive(false);
        }
    }*/
}
