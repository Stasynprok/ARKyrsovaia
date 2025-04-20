using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private Transform _anchorTransform;

    private void OnEnable()
    {
        _anchorTransform = LocationData.Instance.AnchorTransform;
    }

    private void Update()
    {
        if (!_anchorTransform || !gameObject.activeSelf)
        {
            return;
        }

        Vector3 paltformPosition = transform.position;
        Vector3 anchorPosition = _anchorTransform.position;

        if (paltformPosition.y < anchorPosition.y)
        {
            DeactivatePlatform();
        }
    }

    public void ActivatePlatform()
    {
        gameObject.SetActive(true);
    }
    
    public void DeactivatePlatform()
    {
        gameObject.SetActive(false);
    }

}
