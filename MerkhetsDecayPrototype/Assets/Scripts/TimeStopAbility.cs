using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopAbility : MonoBehaviour
{
    [SerializeField] private GameObject _stopWatchPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StopTime()
    {
        GameObject stopWatch = GameObject.Instantiate(_stopWatchPrefab, gameObject.transform.position, gameObject.transform.rotation);
        stopWatch.transform.parent = null;
        GetComponent<Health>().StopHealthDecay();

    }

}
