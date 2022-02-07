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
    private InputActionReference moveAction;

    [SerializeField]
    private CharacterController characterController;

    private Vector3 motionVector;

    private void Awake()
    {
        Instance = this;
        moveAction.action.performed += OnMove;
    }

    private void OnEnable()
    {
        Vector3 move = Vector3.zero;
        moveAction.action.performed += OnMove;
    }


    private void OnMove(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("On Move");
        Vector2 moveDelta = callbackContext.ReadValue<Vector2>();
        motionVector = transform.right * moveDelta.x + transform.forward * moveDelta.y;
        characterController.Move(motionVector.normalized * speed * Time.deltaTime);
    }

    public void OnLook(Vector2 deltaInput)
    {
        Debug.Log("On look");

        transform.Rotate(Vector3.up, deltaInput.x * rotationSpeed * Time.deltaTime);
    }

    private void OnDisable()
    {
        moveAction.action.performed -= OnMove;
    }
}
