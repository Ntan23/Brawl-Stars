using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region EnumVariables
    private enum State {
        Idle, Walking, Shooting
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
    private bool canShoot = true;
    private bool isShooting = false;
    #endregion

    #region IntegerVariables
    [SerializeField] private int bulletAmount;
    #endregion

    #region OtherVariables
    private GameInputManager gameInputManager;
    [SerializeField] private Transform playerPositionIndicatorTransform;
    [SerializeField] private Transform bullet;
    [SerializeField] private Transform shootPosition;
    private CollisionDetector collisionDetector;
    private PlayerAnimationControl playerAnimationControl;
    private AttackTrail attackTrail;
    private ObjectPoolManager objectPoolManager;
    #endregion

    void Start()
    {
        gameInputManager = GameInputManager.Instance;    
        objectPoolManager = ObjectPoolManager.Instance;

        collisionDetector = GetComponent<CollisionDetector>();
        playerAnimationControl = GetComponentInChildren<PlayerAnimationControl>();
        attackTrail = GetComponentInChildren<AttackTrail>();

        state = State.Idle;

        gameInputManager.OnShootAction += GameInput_OnShootAction;
    }

    private void GameInput_OnShootAction(object sender, EventArgs e)
    {
        Shoot();
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
            if(moveDirection != Vector3.zero && isShooting) state = State.Shooting;
            if(moveDirection == Vector3.zero && isShooting) state = State.Shooting;
            else if(moveDirection == Vector3.zero) state = State.Idle;
        }

        if(!attackTrail.InAttackMode()) transform.forward += Vector3.Slerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
        else if(attackTrail.InAttackMode()) transform.forward += Vector3.Slerp(transform.forward, attackTrail.GetShootDirection(), rotateSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        if(attackTrail.InAttackMode() && canShoot) 
        {
            isShooting = true;
            StartCoroutine(ShootBullet());
            canShoot = false;
        }
    }

    IEnumerator ShootBullet()
    {
        GameObject bullet = null;

        for(int i = 0; i < bulletAmount; i++)
        {
            bullet = objectPoolManager.GetPooledObject();
            bullet.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }

        isShooting = false;
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
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
