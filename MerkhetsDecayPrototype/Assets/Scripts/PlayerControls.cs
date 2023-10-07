using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{

    [SerializeField] private CharacterController _controller;



    public InputActions _playerInputActions;

    [SerializeField] private float _moveSpeed = 15.0f;

    [SerializeField] private float sensitivity = 0.1f;

    private float _lookRotation;
    [SerializeField] private GameObject _camHolder;

    [SerializeField] private float _dashDistance = 12.0f;

    private TimeRewind _timeRewind;
    private MeleeAtackPlayer _meleePlayerAtack;
    private TimeForwardAtack _timeForwardAtack;
    private TimeStopAbility _timeStopAbility;



    //Cooldown stuff
    private int _timeForwardCharges = 2;
    private float _maxTimeForwardRechargeTime = 8.0f;
    private float _currTimeForwardChargeTime = 8.0f;
    private bool _canTimeForward = true;
    private Image _timeForwardCharge1Visual;
    private Image _timeForwardCharge2Visual;

    private int _dashCharges = 2;
    private float _maxDashCRechargeTime = 9.0f;
    private float _currDashRechargeTime = 9.0f;
    private bool _canDash = true;
    private Image _dashCharge1Visual;
    private Image _dashCharge2Visual;

    private float _timeStopMaxCooldown = 12.0f;
    private float _timeStopCurrCooldown = 12.0f;
    private bool _canStopTime = true;
   

    private float _timeRewindMaxCooldown = 10.0f;
    private float _timeRewindCurrCooldown = 10.0f;
    private bool _canRewindTime = true;

    private Text _textTimerForward;
    private Text _textTimerDash;
    private Text _textTimerTimeStop;
    private Text _textTimerRewind;

    [SerializeField] private GameObject _clonePrefab;
    [SerializeField] private Transform _cloneSpawnPos;

    private List<GameObject> _cloneList = new List<GameObject>();
    public bool _isFrozen = false;

    public float _gravity = -9.81f; 
    Vector3 _velocity; 

    private void Awake()
    {
        _timeRewind = GetComponent<TimeRewind>();
        _meleePlayerAtack = GetComponent<MeleeAtackPlayer>();
        _timeForwardAtack = GetComponent<TimeForwardAtack>();
        _timeStopAbility = GetComponent<TimeStopAbility>();


        Cursor.lockState = CursorLockMode.Locked;
        
        _playerInputActions = new InputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Dash.performed += Dash;
        _playerInputActions.Player.Rewind.performed += Rewind;
        _playerInputActions.Player.Atack.performed += MeleeAtack;
        _playerInputActions.Player.ForwardTime.performed += TimeForward;
        _playerInputActions.Player.Stopwatch.performed += StopTime;

        _dashCharge1Visual = GameObject.FindGameObjectWithTag("DashCharge1").GetComponent<Image>();
        _dashCharge2Visual = GameObject.FindGameObjectWithTag("DashCharge2").GetComponent<Image>();
        _timeForwardCharge1Visual = GameObject.FindGameObjectWithTag("ForwardCharge1").GetComponent<Image>();
        _timeForwardCharge2Visual = GameObject.FindGameObjectWithTag("ForwardCharge2").GetComponent<Image>();


        _textTimerForward = GameObject.FindGameObjectWithTag("CooldownForward").GetComponent<Text>();
        _textTimerDash = GameObject.FindGameObjectWithTag("CooldownDash").GetComponent<Text>();
        _textTimerTimeStop = GameObject.FindGameObjectWithTag("CooldownStop").GetComponent<Text>();
        _textTimerRewind = GameObject.FindGameObjectWithTag("CooldownRewind").GetComponent<Text>();

    }

    private void Update()
    {
        //timestop
        if (!_canStopTime)
        {
            _timeStopCurrCooldown -= Time.deltaTime;

            if(_timeStopCurrCooldown <= 0)
            {
                _canStopTime = true;
                _timeStopCurrCooldown = _timeStopMaxCooldown;
            }
        }

        

        //Rewind
        if(!_canRewindTime)
        {
            _timeRewindCurrCooldown -= Time.deltaTime;

            if(_timeRewindCurrCooldown <= 0)
            {
                _canRewindTime = true;
                _timeRewindCurrCooldown = _timeRewindMaxCooldown;
            }
        }



        //Dash

        if (_dashCharges >= 1)
            _canDash = true;
        else _canDash = false;

        if (_dashCharges <= 1)
        {
            _currDashRechargeTime -= Time.deltaTime;

            if(_currDashRechargeTime <= 0) 
            {
                ++_dashCharges;
                _currDashRechargeTime = _maxDashCRechargeTime;

            }


        }


        //Timeforward

        if (_timeForwardCharges >= 1)
            _canTimeForward = true;
        else _canTimeForward = false;

        if(_timeForwardCharges <= 1)
        {
            _currTimeForwardChargeTime -= Time.deltaTime;

            if(_currTimeForwardChargeTime <= 0)
            {
                ++_timeForwardCharges;
                _currTimeForwardChargeTime = _maxTimeForwardRechargeTime;
            }

        }


        switch (_dashCharges) 
        {
            case 0:
                _dashCharge1Visual.enabled = false;
                _dashCharge2Visual.enabled = false;
                break;
            case 1:
                _dashCharge1Visual.enabled = true;
                _dashCharge2Visual.enabled = false;
                break;
            case 2:
                _dashCharge1Visual.enabled = true;
                _dashCharge2Visual.enabled = true;
                break;
        }

        switch (_timeForwardCharges)
        {
            case 0:
                _timeForwardCharge1Visual.enabled = false;
                _timeForwardCharge2Visual.enabled = false;
                break;
            case 1:
                _timeForwardCharge1Visual.enabled = true;
                _timeForwardCharge2Visual.enabled = false;
                break;
            case 2:
                _timeForwardCharge1Visual.enabled = true;
                _timeForwardCharge2Visual.enabled = true;
                break;
        }


     


        _textTimerForward.text = Mathf.Floor(_currTimeForwardChargeTime).ToString("F0");
        _textTimerDash.text = Mathf.Floor(_currDashRechargeTime).ToString("F0");
        _textTimerTimeStop.text = Mathf.Floor(_timeStopCurrCooldown).ToString("F0");
        _textTimerRewind.text = Mathf.Floor(_timeRewindCurrCooldown).ToString("F0");

    }

    

    private void FixedUpdate()
    {
        if (_playerInputActions.Player.enabled)
        {
            Vector2 moveInputVec = _playerInputActions.Player.Move.ReadValue<Vector2>();
            Vector3 move = transform.right * moveInputVec.x + transform.forward * moveInputVec.y;

            // Apply gravity
            if (_controller.isGrounded)
            {
                _velocity.y = 0; // Reset vertical velocity when on the ground

                //if (_playerInputActions.Player.Jump.triggered)
                //{
                //    _velocity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
                //}
            }

            _velocity.y += _gravity * Time.fixedDeltaTime;

            // Apply movement
            _controller.Move((move * _moveSpeed + _velocity) * Time.fixedDeltaTime);
        }
    }



    private void LateUpdate()
    {
        if (_playerInputActions.Player.enabled)
        {
            Vector2 lookInputVec = _playerInputActions.Player.Look.ReadValue<Vector2>();

            //Horizontal
            transform.Rotate(Vector3.up * lookInputVec.x * sensitivity);

            //Vertical
            _lookRotation += (-lookInputVec.y * sensitivity);
            _lookRotation = Mathf.Clamp(_lookRotation, -90, 90);
            _camHolder.transform.eulerAngles = new Vector3(_lookRotation, _camHolder.transform.eulerAngles.y, _camHolder.transform.eulerAngles.z);

        }
     
    }

    public void MeleeAtack(InputAction.CallbackContext context)
    {


       // Debug.Log("MeleeAtack");
        _meleePlayerAtack.Atack();

        if (_isFrozen)
            _cloneList.Add(Instantiate(_clonePrefab, _cloneSpawnPos.transform.position, _cloneSpawnPos.transform.rotation));


        //eventually replace te code in atack or do if else statement?


    }

    public void TimeForward(InputAction.CallbackContext context)
    {
        if (!_canTimeForward)
            return;

        //   Debug.Log("TimeForward");
        _timeForwardAtack.ForwardAtack();
        --_timeForwardCharges;

        if (_isFrozen)
            _cloneList.Add(Instantiate(_clonePrefab, _cloneSpawnPos.transform.position, _cloneSpawnPos.transform.rotation));



    }

    public void Dash(InputAction.CallbackContext context)
    {

        if (!_canDash)
            return;

        //Debug.Log("Dash");

        Vector3 dashDirection = _camHolder.transform.forward;
        dashDirection.y = 0;
        dashDirection.Normalize();
        dashDirection *= _dashDistance;

        _controller.Move(dashDirection);

        --_dashCharges;

        //}
    }

    public void Rewind(InputAction.CallbackContext context)
    {
        if (!_canRewindTime)
            return;

      //  Debug.Log("Rewind");
        _timeRewind.Rewind();
        _canRewindTime = false;
    
    } 
    
    public void StopTime(InputAction.CallbackContext context)
    {
        if (!_canStopTime)
            return;

     //   Debug.Log("Stop Time");
        _timeStopAbility.StopTime();


        _canStopTime = false;
    }

    public void UnFreeze()
    {

        _isFrozen = false;

        Invoke("DeleteClones", 0.3f);
    }

    private void DeleteClones()
    {
        foreach(GameObject clone in _cloneList)
        {
            Destroy(clone);
        }
    }
}
