using UnityEngine;
using UnityEngine.UI;

public class QRDetectionMode : MonoBehaviour, IInteractionManagerMode
{
    [SerializeField] private GameObject _ui;

    [Header("UI Stages")]
    [SerializeField] private GameObject _uiImageScan;
    [SerializeField] private GameObject _uiImageScanApply;
    [SerializeField] private GameObject _uiQRCodeScan;

    [Header("Image Detection Buttons")]
    [SerializeField] private Button _applyAnchor;
    [SerializeField] private Button _tryAgain;

    [Header("Components")]
    [SerializeField] private ImageDetectionMode _imageDetection;
    [SerializeField] private QRScanner _qrScanner;

    public void Initialize()
    {
        return;
    }

    public void Activate()
    {
        LocationData.Instance.ClearLocationInformation();
        _ui.SetActive(true);
        _uiImageScan.SetActive(true);
        _uiQRCodeScan.SetActive(false);
        _uiImageScanApply.SetActive(false);

        _imageDetection.Activate();
        _imageDetection.FindAnchor.AddListener(ApplyAnchorUI);
        _qrScanner.Deactivate();
    }

    private void ApplyAnchorUI()
    {
        _uiImageScan.SetActive(false);
        _uiImageScanApply.SetActive(true);
        _imageDetection.FindAnchor.RemoveListener(ApplyAnchorUI);
        _applyAnchor.onClick.AddListener(OnFindAnchor);
        _tryAgain.onClick.AddListener(TryAgainImageDetect);
    }

    private void TryAgainImageDetect()
    {
        _uiImageScan.SetActive(true);
        _uiImageScanApply.SetActive(false);

        _applyAnchor.onClick.RemoveListener(OnFindAnchor);
        _tryAgain.onClick.RemoveListener(TryAgainImageDetect);

        _imageDetection.Activate();
        _imageDetection.FindAnchor.AddListener(ApplyAnchorUI);
    }


    private void OnFindAnchor()
    {
        _imageDetection.DisableAnchorVisual();
        _uiImageScanApply.SetActive(false);
        _applyAnchor.onClick.RemoveListener(OnFindAnchor);
        _tryAgain.onClick.RemoveListener(TryAgainImageDetect);

        _uiImageScan.SetActive(false);
        _uiQRCodeScan.SetActive(true);

        _qrScanner.Activate();
        _qrScanner.SuccesScanJSON.AddListener(OnSuccesScanQR);
    }

    private void OnSuccesScanQR()
    {
        _qrScanner.SuccesScanJSON.RemoveListener(OnSuccesScanQR);
        SelectMode(1);
    }

    public void Deactivate()
    {
        _ui.SetActive(false);
        
    }

    public void SelectMode(int mode)
    {
        InteractionManager.Instance.SelectMode(mode);
    }
}
