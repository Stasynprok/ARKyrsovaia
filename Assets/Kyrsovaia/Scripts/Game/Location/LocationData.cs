using UnityEngine;

public class LocationData : MonoBehaviour
{
    public JsonData LocationInformation;
    public Transform AnchorTransform;

    #region Singleton
    /// <summary>
    /// Instance of our Singleton
    /// </summary>
    public static LocationData Instance
    {
        get
        {
            return _instance;
        }
    }
    private static LocationData _instance;

    public void InitializeSingleton()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }

    private void Awake()
    {
        InitializeSingleton();
    }
    #endregion

    public bool TryGetDataFromJSON(string json)
    {
        bool succesGetData = false;
        JsonData data = JsonUtility.FromJson<JsonData>(json);

        if (data == null)
        {
            succesGetData = false;
            return succesGetData;
        }

        LocationInformation = data;
        succesGetData = true;
        return succesGetData;
    }

    public void ClearLocationInformation()
    {
        LocationInformation = null;
        AnchorTransform = null;
    }

}
