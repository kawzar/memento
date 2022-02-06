using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private float speed = 5;

    [SerializeField]
    private InputActionReference moveAction;

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

    private void Update()
    {
        if (motionVector != Vector3.zero)
        {
            transform.forward = motionVector;
            agent.Move(motionVector * speed * Time.deltaTime);
        }
    }

    private void OnMove(InputAction.CallbackContext callbackContext)
    {
        Vector2 moveDelta = callbackContext.ReadValue<Vector2>();
        motionVector = new Vector3(motionVector.x, transform.position.y, motionVector.y);
    }

    private void OnDisable()
    {
        moveAction.action.performed -= OnMove;
    }
}
