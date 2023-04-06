using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private Transform playerPositionIndicatorTransform;
    [SerializeField] private Transform bullet;
    [SerializeField] private GameObject shootBullet;
    [SerializeField] private GameObject throwBullet;
    private CollisionDetector collisionDetector;
    private PlayerAnimationControl playerAnimationControl;
    private AttackTrail attackTrail;
    private ObjectPoolManager objectPoolManager;
    #endregion

    void Start()
    {  
        objectPoolManager = ObjectPoolManager.Instance;

        collisionDetector = GetComponent<CollisionDetector>();
        playerAnimationControl = GetComponentInChildren<PlayerAnimationControl>();
        attackTrail = GetComponentInChildren<AttackTrail>();

        state = State.Idle;
    }

    void Update()
    {
        MoveAndRotate();
    }

    void MoveAndRotate()
    {
        moveDirection = new Vector3(inputVector.normalized.x, 0f, inputVector.normalized.y);
        moveDistance = speed * Time.deltaTime;

        if(collisionDetector != null) canMove = collisionDetector.DetectCollision(moveDirection);
        
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
        if(attackTrail != null)
        {
            if(attackTrail.InAttackMode() && canShoot) 
            {
                isShooting = true;

                if(attackTrail.ShootMode()) StartCoroutine(ShootBullet());
                if(attackTrail.ThrowMode()) StartCoroutine(ThrowBullet());

                canShoot = false;
            }
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        inputVector = ctx.ReadValue<Vector2>();
    } 

    public void OnShoot(InputAction.CallbackContext ctx)
    {
        ctx.action.performed += OnShoot_Performed;
    }

    private void OnShoot_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
       Shoot();
    }   

    IEnumerator ShootBullet()
    {
        GameObject bullet = null;

        for(int i = 0; i < bulletAmount; i++)
        {
            bullet = objectPoolManager.GetPooledObject("BulletShoot");
            bullet.SetActive(true);

            //Instantiate(shootBullet);
            yield return new WaitForSeconds(0.1f);
        }

        isShooting = false;
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
    }

    IEnumerator ThrowBullet()
    {
        GameObject bullet = objectPoolManager.GetPooledObject("BulletThrow");
        bullet.SetActive(true);
        //Instantiate(throwBullet);
        yield return new WaitForSeconds(0.5f);
        isShooting = false;
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
