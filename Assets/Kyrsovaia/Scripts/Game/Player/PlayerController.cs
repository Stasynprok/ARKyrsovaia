using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public UnityEvent onPlayerFinish = new UnityEvent();
    public UnityEvent onPlayerDead = new UnityEvent();

    [SerializeField] private Transform _graphic;

    [Header("Detectors")]
    [SerializeField] private PlatformDetector _platformDetector;
    [SerializeField] private PlatformDetector _deadZoneDetector;
    [SerializeField] private GameObject _detectionPlatformTrigger;

    [Header("Player Settings")]
    [SerializeField] private float _gravity;
    [SerializeField] private float _forceJump;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _gameScale;

    private UIButtonEvents _leftMoveButton;
    private UIButtonEvents _rightMoveButton;

    private bool _isPlay;

    private Vector3 _velocity = Vector3.zero;
    public bool MoveLocation = false;

    public Vector3 VelocityPlayer
    {
        get
        {
            return _velocity;
        }
    }

    public void Initialize(UIButtonEvents leftMoveButton, UIButtonEvents rightMoveButton)
    {
        _leftMoveButton = leftMoveButton;
        _rightMoveButton = rightMoveButton;
    }

    public void StartGame()
    {
        _isPlay = true;

        ActivateGameController();
        ActivateDetectPlatforms();
    }

    public void StopGame()
    {
        _isPlay = false;
        _velocity = Vector3.zero;

        DeactivateGameController();
        DeactivateDetectPlatforms();
    }

    private void ActivateGameController()
    {
        _leftMoveButton.OnButtonDown.AddListener(OnDownLeftMoveButton);
        _rightMoveButton.OnButtonDown.AddListener(OnDownRightMoveButton);
        
        _leftMoveButton.OnButtonUp.AddListener(OnUpLeftMoveButton);
        _rightMoveButton.OnButtonUp.AddListener(OnUpRightMoveButton);
    }

    private void DeactivateGameController()
    {
        _leftMoveButton.OnButtonDown.RemoveAllListeners();

        _rightMoveButton.OnButtonDown.RemoveAllListeners();

        _leftMoveButton.OnButtonUp.RemoveAllListeners();
        _rightMoveButton.OnButtonUp.RemoveAllListeners();
    }

    private void ActivateDetectPlatforms()
    {
        _platformDetector.onDetectPlatform.AddListener(OnDetectBuffPlatform);
        _platformDetector.onDetectFinishPlatform.AddListener(OnDetectFinishPlatform);

        _platformDetector.onDetectDeadPlatform.AddListener(OnDetectDeadPlatform);
        _deadZoneDetector.onDetectDeadPlatform.AddListener(OnDetectDeadPlatform);
    }
    private void DeactivateDetectPlatforms()
    {
        _platformDetector.onDetectPlatform.RemoveAllListeners();

        _platformDetector.onDetectDeadPlatform.RemoveAllListeners();

        _platformDetector.onDetectFinishPlatform.RemoveAllListeners();

        _deadZoneDetector.onDetectDeadPlatform.RemoveAllListeners();
    }

    private void OnDetectBuffPlatform(int buff)
    {
        if (buff == 0)
        {
            return;
        }
        _velocity.y = Mathf.Sqrt(_forceJump * _gameScale * _gravity * _gameScale * buff  * -1f);
    }

    private void OnDetectFinishPlatform()
    {
        DeactivateDetectPlatforms();
        onPlayerFinish?.Invoke();
    }

    private void OnDetectDeadPlatform()
    {
        DeactivateDetectPlatforms();
        onPlayerDead?.Invoke();
    }

    private void Update()
    {
        if (!_isPlay)
        {
            return;
        }

        float gravityVelocity = _gravity * _gameScale * Time.deltaTime;
        _velocity.y += _gravity * _gameScale * Time.deltaTime;

        bool activeDetectionPlatform = _detectionPlatformTrigger.activeSelf;
        if (_velocity.y <= 0.0f && !activeDetectionPlatform)
        {
            _detectionPlatformTrigger.SetActive(!activeDetectionPlatform);
        }

        if (_velocity.y > 0.0f && activeDetectionPlatform)
        {
            _detectionPlatformTrigger.SetActive(!activeDetectionPlatform);
        }

        if (MoveLocation)
        {
           Vector3 newVelocity = new Vector3(_velocity.x, 0.0f, _velocity.z);
            transform.Translate(newVelocity * Time.deltaTime, Space.Self);
            return;
        }

        transform.Translate(_velocity * Time.deltaTime, Space.Self);
    }

    private void OnDownLeftMoveButton()
    {
        _velocity.z = -_moveSpeed * _gameScale;

        RotateVisual(0.0f);
    }

    private void OnDownRightMoveButton()
    {
        _velocity.z = _moveSpeed * _gameScale;
        RotateVisual(180.0f);
    }
    
    private void OnUpLeftMoveButton()
    {
        _velocity.z = 0.0f;
    }

    private void OnUpRightMoveButton()
    {
        _velocity.z = 0.0f;
    }

    private void RotateVisual(float yValue)
    {
        Vector3 eulerCurrentRotation = _graphic.localRotation.eulerAngles;

        Vector3 newEulerRotation = new Vector3(eulerCurrentRotation.x, yValue, eulerCurrentRotation.z);
        _graphic.localRotation = Quaternion.Euler(newEulerRotation);
    }
}
