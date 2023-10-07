using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class TimeForwardAtack : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _rangedAtackRange = 10.0f;



    [SerializeField] private Transform _laserOrigin;
    [SerializeField] private float laserDuration = 0.075f;
    private LineRenderer _laserLine;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponentInChildren<Camera>();
        _laserLine = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ForwardAtack()
    {

        _laserLine.SetPosition(0, _laserOrigin.position);
      //  Vector3 rayOrigin = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));


        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo, _rangedAtackRange, _enemyLayer))
        {

            _laserLine.SetPosition(1, hitInfo.point);
        }
        else
        {
            _laserLine.SetPosition(1, _camera.transform.position + (_camera.transform.forward * _rangedAtackRange));
        }

        StartCoroutine(ShootLaser());

        if (hitInfo.collider == null)
            return;

        EnemyAI enemy = hitInfo.collider.gameObject.GetComponent<EnemyAI>();
        if (enemy != null)
            enemy.DealDamage(20);


  

    }

    IEnumerator ShootLaser()
    {
        _laserLine.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        _laserLine.enabled = false;
    }

}
