using UnityEngine;
using UnityEngine.UI;

public class GameMode : MonoBehaviour, IInteractionManagerMode
{
    [SerializeField] private GameObject _ui;

    [Header("Start Game UI")]
    [SerializeField] private GameObject _startGameUI;
    [SerializeField] private Button _startBNT;

    [Header("Controller UI")]
    [SerializeField] private GameObject _conttrollerUI;
    [SerializeField] private UIButtonEvents _leftBNT;
    [SerializeField] private UIButtonEvents _rightBNT;

    [Header("Game State UI")]
    [SerializeField] private GameObject _stateUI;
    [SerializeField] private GameObject _winText;
    [SerializeField] private GameObject _gameOverText;
    [SerializeField] private Button _findNewLevelBNT;
    [SerializeField] private Button _replayBNT;

    [Header("Game Settings")]
    [SerializeField] private GameObject _prefabGame;
    private GameObject _gameObject;
    private GameController _gameController;

    public void Initialize()
    {
        _gameObject = Instantiate(_prefabGame);
        _gameObject.SetActive(false);

        _gameController = _gameObject.GetComponent<GameController>();
        _gameController.Initialize(_leftBNT, _rightBNT);
    }

    public void Activate()
    {
        DeactivateAllUI();
        TurnOnGame();


        _ui.SetActive(true);
        ActivateStartUI();
        _gameController.ActivateGameMode();
    }

    public void Deactivate()
    {
        _ui.SetActive(false);
        TurnOffGame();
        DeactivateAllUI();
        _gameController.DeactivateGameMode();
    }

    private void TurnOnGame()
    {
        _gameObject.transform.parent = LocationData.Instance.AnchorTransform;
        _gameObject.transform.localPosition = Vector3.zero;

        _gameObject.SetActive(true);
    }
    
    private void TurnOffGame()
    {
        _gameObject.transform.parent = null;
        _gameObject.SetActive(false);
    }

    private void ActivateStartUI()
    {
        _startGameUI.SetActive(true);
        _startBNT.onClick.AddListener(OnStartBTN);
    }

    private void OnStartBTN()
    {
        DeactivateStartGameUI();
        ActivateContollerUI();
        ActivateGame();
    }

    private void ActivateGame()
    {
        PlayerController playerController = _gameController.PlayerController;
        playerController.onPlayerFinish.AddListener(OnWin);
        playerController.onPlayerDead.AddListener(OnGameOver);
        _gameController.StartGame();
    }
    
    private void DeactivateGame()
    {
        PlayerController playerController = _gameController.PlayerController;

        playerController.onPlayerFinish.RemoveListener(OnWin);
        playerController.onPlayerDead.RemoveListener(OnGameOver);

        _gameController.StopGame();
    }

    private void DeactivateAllUI()
    {
        DeactivateStartGameUI();
        DeactivateStateUI();
        DeactivateContollerUI();
    }

    private void ActivateContollerUI()
    {
        _conttrollerUI.SetActive(true);
    }
    private void DeactivateContollerUI()
    {
        _conttrollerUI.SetActive(false);
    }

    private void DeactivateStartGameUI()
    {
        _startGameUI.SetActive(false);
        _startBNT.onClick.RemoveAllListeners();
    }

    private void DeactivateStateUI()
    {
        _stateUI.SetActive(false);
        _findNewLevelBNT.onClick.RemoveAllListeners();
        _replayBNT.onClick.RemoveAllListeners();
    }

    private void OnGameOver()
    {
        DeactivateContollerUI();
        DeactivateGame();

        _stateUI.SetActive(true);
        _gameOverText.SetActive(true);
        _winText.SetActive(false);

        _findNewLevelBNT.gameObject.SetActive(true);
        _replayBNT.gameObject.SetActive(true);

        _findNewLevelBNT.onClick.AddListener(OnFindNewLevelBTN);
        _replayBNT.onClick.AddListener(OnReplayBTN);
    }

    private void RemoveListenersFromEndGameButtons()
    {
        _findNewLevelBNT.onClick.RemoveAllListeners();
        _replayBNT.onClick.RemoveAllListeners();
    }

    private void OnFindNewLevelBTN()
    {
        ResetGame();
        RemoveListenersFromEndGameButtons();
        InteractionManager.Instance.ReturnToDefaultMode();
        DeactivateStateUI();
    }

    private void OnWin()
    {
        DeactivateGame();
        DeactivateContollerUI();
        _stateUI.SetActive(true);
        _gameOverText.SetActive(false);
        _winText.SetActive(true);

        _findNewLevelBNT.gameObject.SetActive(true);
        _replayBNT.gameObject.SetActive(false);

        _findNewLevelBNT.onClick.AddListener(OnFindNewLevelBTN);
    }

    private void OnReplayBTN()
    {
        RemoveListenersFromEndGameButtons();
        DeactivateStateUI();
        ResetGame();
        ActivateStartUI();
    }

    private void ResetGame()
    {
        _gameController.RestartLocation();
    }
}
