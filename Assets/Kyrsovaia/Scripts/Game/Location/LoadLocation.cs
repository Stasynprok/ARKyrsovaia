using System.Collections.Generic;
using UnityEngine;

public class LoadLocation : MonoBehaviour
{
    public Transform Player;

    [Header("Parents For Platforms")]
    public Transform CommonPlatformParent;
    public Transform BoostPlatformParent;
    public Transform CollapsingPlatformParent;
    public Transform FinishPlatformParent;

    [Header("Prefabs Platforms")]
    public GameObject CommonPlatformPrefab;
    public GameObject BoostPlatformPrefab;
    public GameObject CollapsingPlatformPrefab;
    public GameObject FinishPlatformPrefab;

    private List<PlatformController> _platformObjects = new List<PlatformController>();

    public void GenerateLocationFromData(JsonData data)
    {
        float rounding = data.RoundingData;
        SetPlayerPosition(data.PlayerPosition, rounding);

        PlatformData[] platformDatas = data.PlatformInformation;

        for (int i = 0; i < platformDatas.Length; i++)
        {
            switch (platformDatas[i].Type)
            {
                case PlatformType.Collapsing:
                    SpawnPlatform(CollapsingPlatformParent, CollapsingPlatformPrefab, platformDatas[i], rounding);
                    break;
                case PlatformType.Common:
                    SpawnPlatform(CommonPlatformParent, CommonPlatformPrefab, platformDatas[i], rounding);
                    break;
                case PlatformType.Finish:
                    SpawnPlatform(FinishPlatformParent, FinishPlatformPrefab, platformDatas[i], rounding);
                    break;
                case PlatformType.Boost:
                    SpawnPlatform(BoostPlatformParent, BoostPlatformPrefab, platformDatas[i], rounding);
                    break;
            }
        }
    }

    public void SetPlayerPosition(CustomVector dataPosition, float rounding)
    {
        Vector3 playerLocalPosition = dataPosition.GetVectorFromData(rounding);
        Player.localPosition = playerLocalPosition;
    }

    private void SpawnPlatform(Transform parent, GameObject prefab, PlatformData platformData, float rounding)
    {
        CustomVector[] positionsPlatforms = platformData.Position;

        for (int i = 0; i < positionsPlatforms.Length; i++)
        {
            Vector3 platformLocalPosition = positionsPlatforms[i].GetVectorFromData(rounding);

            GameObject platform = Instantiate(prefab, parent);

            platform.transform.localPosition = platformLocalPosition;
            PlatformController platformController = platform.GetComponent<PlatformController>();

            _platformObjects.Add(platformController);
        }
    }

    public void ResetPlatformActive()
    {
        for (int i = 0; i < _platformObjects.Count; i++)
        {
            _platformObjects[i].ActivatePlatform();
        }
    }

    public void DestroyAllPlatforms()
    {
        for (int i = 0; i < _platformObjects.Count; i++)
        {
            GameObject platformObject = _platformObjects[i].gameObject;
            Destroy(platformObject);
        }

        _platformObjects.Clear();
    }
}
