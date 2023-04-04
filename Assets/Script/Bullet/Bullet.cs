using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 shootDirection;
    [SerializeField] private float speed;
    private float timer;
    private AttackTrail attackTrail;
    private ObjectPoolManager objectPoolManager;

    void OnEnable()
    {
        attackTrail = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<AttackTrail>();
        objectPoolManager = ObjectPoolManager.Instance;

        shootDirection = attackTrail.GetShootDirection();
        transform.position = objectPoolManager.GetBulletSpawnPosition().position;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(timer > 2.0f)
        {
            timer = 0.0f;
            gameObject.SetActive(false);
        }

        transform.Translate(shootDirection * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag("CanBeHit")) gameObject.SetActive(false);
    }
}
