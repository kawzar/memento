using System.Collections;
using System.Collections.Generic;

using Cinemachine;

using UnityEngine;

public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField]
    private float horizontalSpeed = 10f;

    [SerializeField]
    private float verticalSpeed = 10f;

    [SerializeField]
    private float clampAngle = 80f;

    private PlayerActionsExample playerControls;

    private Vector3 startingRotation;


    private void OnEnable()
    {
        playerControls = new PlayerActionsExample();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow && stage == CinemachineCore.Stage.Aim)
        {
            if(startingRotation == null)
            {
                startingRotation = transform.localRotation.eulerAngles;
            }

            Vector2 deltaInput = playerControls.Player.Look.ReadValue<Vector2>();
            startingRotation.x += deltaInput.x * Time.deltaTime * verticalSpeed;
            startingRotation.y += deltaInput.y * Time.deltaTime * horizontalSpeed;
            startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);

            Quaternion quaternion = Quaternion.Euler(startingRotation.y, startingRotation.x, 0f);
            state.RawOrientation = quaternion;

            Player.Instance.OnLook(deltaInput);
        }
    }
}
