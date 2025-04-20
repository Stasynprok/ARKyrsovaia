using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerController PlayerController;
    public LocationMovement LocationMovement;
    public LookAtPlayer LookAtPlayer;
    public LoadLocation LoadLocation;


    public void Initialize(UIButtonEvents leftMoveButton, UIButtonEvents rightMoveButton)
    {
        PlayerController.Initialize(leftMoveButton, rightMoveButton);
    }

    public void ActivateGameMode()
    {
        LookAtPlayer.ActivateGameMode();
        LoadLocationData();
    }

    public void DeactivateGameMode()
    {
        LookAtPlayer.DeactivateGameMode();
        UnloadLocationData();
    }

    public void StartGame()
    {
        LocationMovement.StartGame();
        PlayerController.StartGame();
    }

    public void StopGame()
    {
        LocationMovement.StopGame();
        PlayerController.StopGame();
    }

    public void LoadLocationData()
    {
        LoadLocation.GenerateLocationFromData(LocationData.Instance.LocationInformation);
    }
    public void UnloadLocationData()
    {
        LoadLocation.DestroyAllPlatforms();
    }

    public void RestartLocation()
    {
        LoadLocation.ResetPlatformActive();
        LocationMovement.ResetLocationPosition();
        CustomVector playerPosition = LocationData.Instance.LocationInformation.PlayerPosition;
        float rounding = LocationData.Instance.LocationInformation.RoundingData;
        LoadLocation.SetPlayerPosition(playerPosition, rounding);
    }
}
