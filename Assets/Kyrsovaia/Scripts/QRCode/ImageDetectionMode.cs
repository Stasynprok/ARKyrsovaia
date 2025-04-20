using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageDetectionMode : MonoBehaviour
{
    public UnityEvent FindAnchor = new UnityEvent();
    [SerializeField] private string _anchorVisualObjectName;
    [SerializeField] private GameObject _objectToAnchorPrefab;
    [SerializeField] private XRReferenceImageLibrary _refLibrary;

    private ARTrackedImageManager _arTrackedImageManager;
    private GameObject _objectToAnchor;

    private void ActivateTrackedObject(ARTrackedImage trackedImage)
    {
        _objectToAnchor = Instantiate(_objectToAnchorPrefab);
        _objectToAnchor.transform.position = trackedImage.transform.position;
        _objectToAnchor.transform.rotation = Quaternion.identity;
        _objectToAnchor.AddComponent<ARAnchor>();

        LocationData.Instance.AnchorTransform = _objectToAnchor.transform;
        FindAnchor?.Invoke();

        Deactivate();
    }

    public void DisableAnchorVisual()
    {
        int childCount = _objectToAnchor.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform transformChild = _objectToAnchor.transform.GetChild(i);
            string nameChild = transformChild.name;
            if (_anchorVisualObjectName == nameChild)
            {
                transformChild.gameObject.SetActive(false);
                return;
            }
        }
    }


    private void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var addedImage in args.added)
        {
            ActivateTrackedObject(addedImage);
        }
    }

    public void Activate()
    {
        Transform xrTransform = InteractionManager.Instance.transform;
        _arTrackedImageManager = xrTransform.AddComponent<ARTrackedImageManager>();
        _arTrackedImageManager.referenceLibrary = _refLibrary;
        _arTrackedImageManager.enabled = true;

        _arTrackedImageManager.trackedImagesChanged += OnImageChanged;
        if (_objectToAnchor)
        {
            Destroy(_objectToAnchor);
        }
    }

    public void Deactivate()
    {
        if (_arTrackedImageManager)
        {
            _arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
            Destroy(_arTrackedImageManager);
        }
    }
}
