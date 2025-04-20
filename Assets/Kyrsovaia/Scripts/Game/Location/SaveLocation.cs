using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLocation : MonoBehaviour
{
    [TextArea(3, 10)]
    public string _json;

    public Transform Player;
    public Transform CommonPlatformParent;
    public Transform BoostPlatformParent;
    public Transform CollapsingPlatformParent;
    public Transform FinishPlatformParent;

    public float RoundingData = 100.0f;


    [Header("Start In Editor")]
    public bool GetJSON = false;

    private void OnValidate()
    {
        if (GetJSON)
        {
            SerializeJSON();
            GetJSON = false;
        }
    }

    private void SerializeJSON()
    {
        JsonData jsonData = GetJsonData();
        _json = JsonUtility.ToJson(jsonData);
    }

    private JsonData GetJsonData()
    {
        PlatformBuff[] commonPlatforms = CommonPlatformParent.GetComponentsInChildren<PlatformBuff>();
        PlatformBuff[] boostPlatforms = BoostPlatformParent.GetComponentsInChildren<PlatformBuff>();
        PlatformBuff[] collapsingPlatforms = CollapsingPlatformParent.GetComponentsInChildren<PlatformBuff>();
        PlatformBuff[] finishPlatforms = FinishPlatformParent.GetComponentsInChildren<PlatformBuff>();

        List<PlatformData> positionList = new List<PlatformData>();

        positionList.Add(GetPlatformData(commonPlatforms));
        positionList.Add(GetPlatformData(boostPlatforms));
        positionList.Add(GetPlatformData(collapsingPlatforms));
        positionList.Add(GetPlatformData(finishPlatforms));

        Vector3 currentPlayerPosition = Player.transform.localPosition;

        currentPlayerPosition = currentPlayerPosition * RoundingData;

        float xPosition = Mathf.Round(currentPlayerPosition.x);
        float yPosition = Mathf.Round(currentPlayerPosition.y);
        float zPosition = Mathf.Round(currentPlayerPosition.z);

        CustomVector playerPosition = new CustomVector(xPosition, yPosition, zPosition);
        JsonData data = new JsonData(playerPosition, positionList.ToArray(), RoundingData);

        return data;
    }

    private PlatformData GetPlatformData(PlatformBuff[] platforms)
    {
        CustomVector[] arrayPositions = new CustomVector[platforms.Length];

        for (int i = 0; i < platforms.Length; i++)
        {
            Vector3 currentPosition = platforms[i].transform.localPosition;

            currentPosition = currentPosition * RoundingData;

            float xPosition = Mathf.Round(currentPosition.x);
            float yPosition = Mathf.Round(currentPosition.y);
            float zPosition = Mathf.Round(currentPosition.z);

            CustomVector vectorPosition = new CustomVector(xPosition, yPosition, zPosition);
            arrayPositions[i] = vectorPosition;
        }

        PlatformData platformData = new PlatformData(platforms[0].Type, arrayPositions);
        return platformData;
    }
}

[Serializable]
public class JsonData
{
    public CustomVector PlayerPosition;
    public PlatformData[] PlatformInformation;
    public float RoundingData;

    public JsonData(CustomVector playerPosition, PlatformData[] platformInformation, float roundingData)
    {
        PlayerPosition = playerPosition;
        PlatformInformation = platformInformation;
        RoundingData = roundingData;
    }
}

[Serializable]
public class PlatformData
{
    public PlatformType Type;
    public CustomVector[] Position;

    public PlatformData(PlatformType type, CustomVector[] position)
    {
        Type = type;
        Position = position;
    }
}

[Serializable]
public class CustomVector
{
    public float X;
    public float Y;
    public float Z;

    public CustomVector(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vector3 GetVectorFromData(float rounding)
    {
        Vector3 vector = Vector3.zero;

        float xPosition = X / rounding;
        float yPosition = Y / rounding;
        float zPosition = Z / rounding;

        vector = new Vector3(xPosition, yPosition, zPosition);
        return vector;
    }
}
