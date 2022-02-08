
using UnityEngine;
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

    [SerializeField]
    private float smoothInputSpeed = 0.2f;

    [SerializeField]
    private InputActionReference moveAction;

    [SerializeField]
    private InputActionReference lookAction;

    private Vector3 motionVector, startingRotation;
    private float cinemachineTargetPitch, rotationVelocity;
    private bool isGrounded;
    private float gravity = 9.8f;
    private Vector2 currentInputVector, smoothInputVelocity;
    
    private void Awake()
    {
        Instance = this;
    }
    
    private void Update()
    {
        Vector2 moveInputValue = moveAction.action.ReadValue<Vector2>();
        currentInputVector = Vector2.SmoothDamp(currentInputVector, moveInputValue, ref smoothInputVelocity, smoothInputSpeed * Time.deltaTime);
        isGrounded = characterController.isGrounded;

        if(isGrounded && characterController.velocity.y < 0)
        {
            motionVector.y = 0;
        }

        if (moveInputValue != Vector2.zero)
        {
            motionVector = new Vector3(currentInputVector.x, motionVector.y, currentInputVector.y) * speed;            
        }

        motionVector.y -= gravity * Time.deltaTime;
        characterController.Move(motionVector);

        Vector2 lookInput = lookAction.action.ReadValue<Vector2>();

        if (startingRotation == null)
        {
            startingRotation = transform.localRotation.eulerAngles;
        }

        startingRotation.x += lookInput.x * Time.deltaTime * rotationSpeed;
        startingRotation.y += lookInput.y * Time.deltaTime * rotationSpeed;
        startingRotation.z = Mathf.Clamp(startingRotation.y, -bottomClamp, bottomClamp);

        cinemachineTargetPitch += lookInput.y * rotationSpeed * Time.deltaTime;
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

        head.localRotation = Quaternion.Euler(cinemachineTargetPitch, 0.0f, 0.0f);

        rotationVelocity = lookInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * rotationVelocity);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
