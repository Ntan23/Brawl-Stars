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
    [SerializeField] private float damage;
    private float timer;
    #endregion

    #region BoolVariables
    private bool firstTimeRun = true;
    private bool firstTimeActive = true;
    #endregion

    #region OtherVariables
    private Vector3 shootDirection;
    private Vector3[] bulletPoints = new Vector3[9];
    private AttackTrail attackTrail;
    private ObjectPoolManager objectPoolManager;
    private Rigidbody rb;
    private GameManager gm;
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        objectPoolManager = ObjectPoolManager.Instance;
        gm = GameManager.Instance;

        shootBullet = new ShootBullet();
        throwBullet = new ThrowBullet();
    }

    void OnEnable()
    {
        if(objectPoolManager == null) objectPoolManager = ObjectPoolManager.Instance;

        if(!firstTimeActive)
        {
            if(bulletType == Type.Shoot)
            {
                attackTrail = GameObject.FindGameObjectWithTag("Player2").GetComponentInChildren<AttackTrail>();
                transform.position = objectPoolManager.GetBulletSpawnPositionForPlayer2().position;
                shootDirection = attackTrail.GetShootDirection();
            }

            if(bulletType == Type.Throw)
            {
                attackTrail = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<AttackTrail>();
                transform.position = objectPoolManager.GetBulletSpawnPositionForPlayer1().position;
                attackTrail.GetBulletPointsForThrow().CopyTo(bulletPoints, 0);
            }
        }
    }

    void OnDisable()
    {
        firstTimeRun = true;

        if(firstTimeActive) firstTimeActive = false;
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

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && bulletType == Type.Shoot) 
        {
            gm.DecreasePlayerHealth(1, damage);
            gameObject.SetActive(false);
        }
        if(other.CompareTag("Player2") && bulletType == Type.Throw) 
        {
            gm.DecreasePlayerHealth(2, damage);
            gameObject.SetActive(false);
        }
    }
}
