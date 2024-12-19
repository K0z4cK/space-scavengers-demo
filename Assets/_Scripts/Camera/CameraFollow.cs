using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _poi;
    [SerializeField] private float _movementSmooth;

    private Vector3 _offset;
 
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
        _offset = _transform.position - _poi.position;
    }

    void FixedUpdate()
    {
        Vector3 desiredPosition = _poi.position + _offset;

        _transform.position = Vector3.Lerp(transform.position, desiredPosition, _movementSmooth);
    }
}
