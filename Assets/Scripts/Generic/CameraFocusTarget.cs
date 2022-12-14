using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

public class CameraFocusTarget : MonoBehaviour
{
    [Title("Camera Settings")]
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private CinemachineTargetGroup _targetGroup;
    [SerializeField] private VariableTransform _cameraTarget;
    [SerializeField] private VariableFloat _cameraDistance;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _focusDistance;
    private bool _canFocusPlayer = true;

    [Title("Focus Settings")]
    [SerializeField] private float _weight;
    [SerializeField] private float _radius;

    [Title("Listening on")]
    [SerializeField] private VoidEventChannelSO _eventTriggerPlayerMovementOn;
    [SerializeField] private Vector3EventChannelSO _eventTriggerPlayerMovementOff;

    private void Start()
    {
        _eventTriggerPlayerMovementOn.OnEventRaised += ChangeFocus;
        _eventTriggerPlayerMovementOff.OnEventRaised += ChangeFocus;
    }

    private void ChangeFocus(Vector3 _)
    {
        ChangeFocus();
    }

    private void ChangeFocus()
    {
        _canFocusPlayer = !_canFocusPlayer;

        if (_canFocusPlayer)
        {
            _virtualCamera.Follow = _player;
            _targetGroup.RemoveMember(_cameraTarget.Value);
        }
        else
        {
            _virtualCamera.Follow = _targetGroup.transform;
            AdjustFocusDistance();
            _targetGroup.AddMember(_cameraTarget.Value, _weight, _radius);
        }
    }

    private void AdjustFocusDistance()
    {
        _focusDistance.localPosition = new Vector3(0f, -_cameraDistance.Value, 0f);
    }

}
