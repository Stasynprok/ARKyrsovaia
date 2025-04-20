using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 5f;
    private bool _isPlay = false;
    private Transform _cameraTransform;

    public void ActivateGameMode()
    {
        _isPlay = true;
        _cameraTransform = InteractionManager.Instance.ARCamera.transform;
    }
    
    public void DeactivateGameMode()
    {
        _isPlay = false;
    }


    private void Update()
    {
        if (_isPlay)
        {
            Vector3 direction = _cameraTransform.position - transform.position;
            direction.y = 0;

            if (direction == Vector3.zero) return;

            Quaternion targetRotation = Quaternion.LookRotation(
                Vector3.Cross(direction.normalized, Vector3.up),
                Vector3.up
            );

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                _rotationSpeed * Time.deltaTime
            );
        
        }
    }
}
