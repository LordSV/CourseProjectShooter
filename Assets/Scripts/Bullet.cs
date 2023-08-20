using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 5f;
    [SerializeField] private Rigidbody _rigidbody;
    private int _damage;

    public void Init(Vector3 velocity, int damage = 0)
    {
        _damage = damage;
        _rigidbody.velocity = velocity;
        StartCoroutine(DestroyDelay());
    }

    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSecondsRealtime(_lifeTime);
        Destroy(gameObject);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.TryGetComponent(out Damager enemy))
        {
            enemy.ApplyDamage(_damage);
        }
        Destroy(gameObject);
    }
}
