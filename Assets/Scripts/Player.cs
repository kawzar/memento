using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField]
    private float speed = 5;

    [SerializeField]
    private float rotationSpeed = 3;

    [SerializeField]
    private float topClamp = 40;

    [SerializeField]
    private float bottomClamp = 40;

    [SerializeField]
    private CharacterController characterController;

    [SerializeField]
    private Transform head;

    private Vector3 motionVector, startingRotation;
    private float cinemachineTargetPitch, rotationVelocity;

    private void Awake()
    {
        Instance = this;
    }

   
    private void OnMove(InputValue inputValue)
    {
        Debug.Log("On Move");
        Vector2 moveDelta = inputValue.Get<Vector2>();

        if (moveDelta != Vector2.zero)
        {
            motionVector = transform.right * moveDelta.x + transform.forward * moveDelta.y;
        }

        characterController.Move(motionVector.normalized * speed * Time.deltaTime);
    }

    private void OnLook(InputValue inputValue)
    {
        Debug.Log("On Move");
        Vector2 deltaInput = inputValue.Get<Vector2>();

        if (startingRotation == null)
        {
            startingRotation = transform.localRotation.eulerAngles;
        }

        startingRotation.x += deltaInput.x * Time.deltaTime * rotationSpeed;
        startingRotation.y += deltaInput.y * Time.deltaTime * rotationSpeed;
        startingRotation.z = Mathf.Clamp(startingRotation.y, -bottomClamp, bottomClamp);

        cinemachineTargetPitch += deltaInput.y * rotationSpeed * Time.deltaTime;
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

        head.localRotation = Quaternion.Euler(cinemachineTargetPitch, 0.0f, 0.0f);

        rotationVelocity = deltaInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * rotationVelocity);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
