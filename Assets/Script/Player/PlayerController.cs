using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region EnumVariables
    private enum State {
        Idle, Walking
    }

    private State state;
    #endregion

    #region VectorVariables
    private Vector2 inputVector;
    private Vector3 moveDirection;
    private Vector3 moveDirectionX;
    private Vector3 moveDirectionZ;
    #endregion

    #region FloatVariables
    [SerializeField] private float playerHeight;
    [SerializeField] private float playerRadius;
    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed;
    private float moveDistance;
    #endregion

    #region BoolVariables
    private bool canMove;
    #endregion

    #region OtherVariables
    private GameInputManager gameInputManager;
    [SerializeField] private Transform playerPositionIndicatorTransform;
    private CollisionDetector collisionDetector;
    private PlayerAnimationControl playerAnimationControl;
    #endregion

    void Start()
    {
        gameInputManager = GameInputManager.Instance;    
        collisionDetector = GetComponent<CollisionDetector>();
        playerAnimationControl = GetComponentInChildren<PlayerAnimationControl>();

        state = State.Idle;
    }

    void Update()
    {
        MoveAndRotate();
    }

    void MoveAndRotate()
    {
        inputVector = gameInputManager.GetMovementVectorNormalized();

        moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        moveDistance = speed * Time.deltaTime;

        canMove = collisionDetector.DetectCollision(moveDirection);
        
        if(!canMove)
        {
            moveDirectionX = new Vector3(moveDirection.x, 0f, 0f).normalized;
            canMove = moveDirection.x != 0 && collisionDetector.DetectCollision(moveDirectionX);

            if(canMove) moveDirection = moveDirectionX;
            else if(!canMove)
            {
                moveDirectionZ = new Vector3(0f, 0f, moveDirection.z).normalized;
                canMove = moveDirection.z != 0 && collisionDetector.DetectCollision(moveDirectionZ);

                if(canMove) moveDirection = moveDirectionZ;
            }
        }

        if(canMove) 
        {
            transform.position += moveDirection * moveDistance;

            if(moveDirection != Vector3.zero) state = State.Walking;
            else if(moveDirection == Vector3.zero) state = State.Idle;
        }
      
        transform.forward += Vector3.Slerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
    }

    public float GetPlayerHeight()
    {
        return playerHeight;
    }

    public float GetPlayerRadius()
    {
        return playerRadius;
    }

    public float GetMoveDistance()
    {
        return moveDistance;
    }

    public Vector3 GetMoveDirection()
    {
        return moveDirection;
    }

    public int GetState()
    {
        return (int)state;
    }
}
