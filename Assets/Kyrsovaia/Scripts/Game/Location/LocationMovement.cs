using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationMovement : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private Transform _centerLocationTransform;
    [SerializeField] private Transform _locationTransform;

    private bool _isPlay = false;

    private void Update()
    {
        if (!_isPlay)
        {
            return;
        }

        Vector3 playerVelocity = _player.VelocityPlayer;
        Vector3 playerLocalPosition = _player.transform.localPosition;
        Vector3 centerLocalPosition = _centerLocationTransform.localPosition;

        float yPlayerVelocity = playerVelocity.y;

        if (playerLocalPosition.y >= centerLocalPosition.y && yPlayerVelocity > 0.0f)
        {
            if (!_player.MoveLocation)
            {
                _player.MoveLocation = true;
            }
            Vector3 velocityLocation = yPlayerVelocity * -Vector3.up;
            _locationTransform.Translate(velocityLocation * Time.deltaTime, Space.Self);
            return;
        }

        if (_player.MoveLocation)
        {
            _player.MoveLocation = false;
        }
    }

    public void StartGame()
    {
        _isPlay = true;
    }

    public void StopGame()
    {
        _isPlay = false;
    }

    public void ResetLocationPosition()
    {
        _locationTransform.localPosition = Vector3.zero;
    }
}
