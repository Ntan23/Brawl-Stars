using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootBullet
{
    void Move(Transform transform, Vector3 shootDirection, float speed);
}

public interface IThrowBullet
{
    void Throw(Transform transform, Vector3[] points, Rigidbody rb, float speed, bool firstTimeRun);
}

public class ShootBullet : IShootBullet
{
    public void Move(Transform transform, Vector3 shootDirection, float speed)
    {
        transform.Translate(shootDirection * speed * Time.deltaTime);
    }
}

public class ThrowBullet : IThrowBullet
{  
    private bool isThrow;
    private bool firstTimeRun = true;
    private int currentIndex;

    public void Throw(Transform transform, Vector3[] points, Rigidbody rb, float speed, bool firstTime)
    {
        firstTimeRun = firstTime;

        if(firstTimeRun)
        {
            isThrow = true;
            currentIndex = 0;
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        
        if(isThrow) 
        {
            transform.LookAt(points[currentIndex]);
            isThrow = false;
        }
        else if((points[currentIndex] - transform.position).sqrMagnitude < 0.2f)
        {
            currentIndex++;
            transform.LookAt(points[currentIndex]);
        }
    }
}

public class Bullet : MonoBehaviour
{
    #region EnumVariables
    private enum Type {
        Shoot, Throw
    }
    
    [SerializeField] private Type bulletType;
    #endregion

    #region InterfaceVariables
    private IShootBullet shootBullet;
    private IThrowBullet throwBullet;
    #endregion

    #region FloatVariables
    [SerializeField] private float speed;
    private float timer;
    #endregion

    #region BoolVariables
    private bool firstTimeRun = true;
    #endregion

    #region OtherVariables
    private Vector3 shootDirection;
    private Vector3[] bulletPoints;
    private AttackTrail attackTrail;
    private ObjectPoolManager objectPoolManager;
    private Rigidbody rb;
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        shootBullet = new ShootBullet();
        throwBullet = new ThrowBullet();
    }

    void OnEnable()
    {
        if(attackTrail == null) attackTrail = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<AttackTrail>();
        if(objectPoolManager == null) objectPoolManager = ObjectPoolManager.Instance;

        if(bulletPoints == null) bulletPoints = new Vector3[9];

        shootDirection = attackTrail.GetShootDirection();
        transform.position = objectPoolManager.GetBulletSpawnPosition().position;

        if(attackTrail.GetBulletPointsForThrow() != null) attackTrail.GetBulletPointsForThrow().CopyTo(bulletPoints, 0);
    }

    void OnDisable()
    {
        firstTimeRun = true;
    }

    void Update()
    {
        if(bulletType == Type.Shoot)
        {
            timer += Time.deltaTime;

            if(timer > 1.0f)
            {
                timer = 0.0f;
                gameObject.SetActive(false);
            }
        }

        if(bulletType == Type.Shoot) shootBullet.Move(transform, shootDirection, speed);
        if(bulletType == Type.Throw)
        {
            throwBullet.Throw(transform, bulletPoints, rb, speed, firstTimeRun);
            firstTimeRun = false;
        } 
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag("CanBeHit")) gameObject.SetActive(false);
    }
}
