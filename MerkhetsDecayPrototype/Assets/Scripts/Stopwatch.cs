using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stopwatch : MonoBehaviour
{
    [SerializeField] private float _stopwatchDuration = 4;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private LayerMask _projectileLayer;

    private List<EnemyAI> _enemies = new List<EnemyAI>();
    private List<EnemyProjectile> _projectiles = new List<EnemyProjectile>();

    private PlayerControls _playerControls;


    // Start is called before the first frame update
    void Start()
    {
        //Invoke the restarttime after x sec
        Invoke("RestartTime", _stopwatchDuration);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
             _playerControls = player.GetComponent<PlayerControls>();
            _playerControls._isFrozen = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
     
       // Debug.Log(other.gameObject.layer);


        //Get enemies
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
           // Debug.Log(other);
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy == null)
                return;

            _enemies.Add(enemy);
    
            enemy.StopTime();


        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            Debug.Log(other);
            EnemyProjectile projectile = other.GetComponent<EnemyProjectile>();
            if (projectile == null)
                return;

            _projectiles.Add(projectile);

            projectile.StopTime();
        }

        //Get projectiles

    }

    void RestartTime()
    {

        GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().RestartHealthDecay();


        foreach (EnemyAI enemy in _enemies)
        {
            enemy.RestartTime();
        }

        foreach(EnemyProjectile projectile in _projectiles)
        {
            projectile.RestartTime();
        }

        _playerControls.UnFreeze();

        Destroy(this.gameObject);
    }

}
