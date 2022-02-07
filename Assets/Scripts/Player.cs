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
    private float clampAngle = 40;

    [SerializeField]
    private CharacterController characterController;

    private Vector3 motionVector, startingRotation;

    private void Awake()
    {
        Instance = this;
    }

   
    private void OnMove(InputValue inputValue)
    {
        Debug.Log("On Move");
        Vector2 moveDelta = inputValue.Get<Vector2>();
        motionVector = transform.right * moveDelta.x + transform.forward * moveDelta.y;
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
        startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);

        transform.Rotate(startingRotation);
    }  
}
