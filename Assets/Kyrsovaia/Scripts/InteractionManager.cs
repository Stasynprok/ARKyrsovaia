using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARAnchorManager))]
public class InteractionManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _modeObjects;
    [SerializeField] private int _defaultModeIndex = 0;

    private Camera _arCamera;
    public Camera ARCamera
    {
        get
        {
            return _arCamera;
        }
    }

    private IInteractionManagerMode[] _modes;
    private IInteractionManagerMode _currentMode = null;

    private ARCameraManager _arCameraManager;

    public ARCameraManager ARCameraManager
    {
        get
        {
            return _arCameraManager;
        }
    }

    #region Singleton
    /// <summary>
    /// Instance of our Singleton
    /// </summary>
    public static InteractionManager Instance
    {
        get
        {
            return _instance;
        }
    }
    private static InteractionManager _instance;

    public void InitializeSingleton()
    {
        // Destroy any duplicate instances that may have been created
        if (_instance != null && _instance != this)
        {
            Debug.Log("destroying singleton");
            Destroy(this);
            return;
        }
        _instance = this;
    }
    #endregion

    private void Awake()
    {
        InitializeSingleton();


        _arCameraManager = GetComponentInChildren<ARCameraManager>();
        if (!_arCameraManager)
            throw new MissingComponentException("ARCameraManager component not found!");

        // get interfaces from game objects
        _modes = new IInteractionManagerMode[_modeObjects.Length];
        for (int i = 0; i < _modeObjects.Length; i++)
        {
            _modes[i] = _modeObjects[i].GetComponent<IInteractionManagerMode>();

            if (_modes[i] == null)
            {
                throw new MissingComponentException("Missing mode component on " + _modeObjects[i].name);
            }

            _modes[i].Initialize();
            Debug.Log("[INTERACTION_MANAGER] Found mode = " + _modes[i]);
        }
    }


    private void Start()
    {
        // get camera in children
        _arCamera = GetComponentInChildren<Camera>();
        if (!_arCamera)
            throw new MissingComponentException("[INTERACTION_MANAGER] Camera not found in children of Interaction manager!");
        ReturnToDefaultMode();
    }

    /// <summary>
    /// This method activates the selected mode and deactivates the rest
    /// </summary>
    private void UpdateModes()
    {
        for (int i = 0; i < _modes.Length; i++)
            if (_currentMode != _modes[i])
                _modes[i].Deactivate();

        for (int i = 0; i < _modes.Length; i++)
            if (_currentMode == _modes[i])
                _modes[i].Activate();
    }

    public void SelectMode(int modeNumber)
    {
        _currentMode = _modes[modeNumber];
        UpdateModes();
    }

    public void ReturnToDefaultMode()
    {
        SelectMode(_defaultModeIndex);
    }
}
