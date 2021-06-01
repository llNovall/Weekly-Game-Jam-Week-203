using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Vector3 _startPosition, _endPosition;

    [SerializeField]
    private float _distance;

    [SerializeField]
    private float _padding;

    [SerializeField]
    private Transform _player;

    private void Start()
    {
        _player = PlayerTracker.Current.transform;
        _distance = Mathf.Abs(_endPosition.x - _startPosition.x);
    }

    private void Update()
    {
        float remappedPlayerX = HelperUtility.MapValue(_player.transform.position.x, _startPosition.x, _endPosition.x, 0, _distance);
        gameObject.transform.position = Vector3.Lerp(_startPosition, _endPosition, Mathf.Clamp(remappedPlayerX/_distance, _padding, 1 - _padding));

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_startPosition + Vector3.forward * 3, Vector3.Lerp(_startPosition + Vector3.forward * 3, _endPosition + Vector3.forward * 3, _padding));
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(_startPosition, _endPosition);
    }
}
