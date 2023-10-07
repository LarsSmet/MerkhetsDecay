using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private float _projectileSpeed = 10.0f;
    private Vector3 _oldVelocity = Vector3.zero;
    [SerializeField] float _projectileDamage = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = transform.forward * _projectileSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopTime()
    {
        _oldVelocity = _rb.velocity;
        _rb.velocity = Vector3.zero;
    }

    public void RestartTime()
    {
       _rb.velocity = _oldVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Health health = collision.gameObject.GetComponent<Health>();

                if (health == null)
                {
                    Destroy(this.gameObject);
                    return;
                }
                health.DealDamage(_projectileDamage);
                Destroy(this.gameObject);
            }

            if(collision.gameObject.layer == LayerMask.NameToLayer("Level"))
                Destroy(this.gameObject);
        }
      
    }
}
