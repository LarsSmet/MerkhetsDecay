using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    

     [SerializeField] float _maxHealth = 50.0f;
    private float _currHealth;

    private Slider _healthBar;

    [SerializeField] private float _depletionSpeed = 0.1f;

    bool _stopHealthDecay = false;

    // Start is called before the first frame update
    void Start()
    {
        _healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Slider>();

        _currHealth = _maxHealth;
        _healthBar.maxValue = _maxHealth;
        _healthBar.minValue = 0;
        _healthBar.value = _healthBar.maxValue;
       
    }


    // Update is called once per frame
    void Update()
    {
        if (!_stopHealthDecay)
        {

            _currHealth -= Time.deltaTime * _depletionSpeed;

        }

        if(_currHealth < 0)
        {
            //LOSE
            Application.Quit();
        }

        _healthBar.value = _currHealth;
    }


    public void Heal(float health)
    {
        _currHealth += health;
        if(_currHealth > _maxHealth) 
            _currHealth = _maxHealth;
    }

    public void DealDamage(float damage)
    {
        _currHealth -= damage;
    }



    public void StopHealthDecay()
    {
        _stopHealthDecay=true;
    }

    public void RestartHealthDecay()
    {
        _stopHealthDecay = false;
    }

    public void SaveHealthBeforeRewind()
    {


    }

    public float GetCurrHealth()
    {
        return _currHealth;
    }

    public void SetCurrHealth(float health)
    {
        _currHealth=health;
    }
}
