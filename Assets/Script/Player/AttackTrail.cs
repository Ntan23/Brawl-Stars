using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootAttackTrail
{
    void Aim(LineRenderer lineRenderer, Vector2 inputVector,  Vector3 shootDirection, Transform transform, GameObject player, float lineDistance, float rotateSpeed);
}

public interface IThrowAttackTrail 
{
    void Aim(LineRenderer lineRenderer, Vector2 inputVector, Vector3 shootDirection, Vector3[] bulletPoints, Transform transform, GameObject player, float YLinePower, float rotateSpeed);
}

public class ShootAttackTrail : IShootAttackTrail
{
    public void Aim(LineRenderer lineRenderer, Vector2 inputVector,  Vector3 shootDirection, Transform transform, GameObject player, float lineDistance, float rotateSpeed)
    {
        lineRenderer.positionCount = 2;

        if(inputVector != Vector2.zero)
        {
            lineRenderer.enabled = true;

            transform.position = new Vector3(player.transform.position.x, 0.1f, player.transform.position.z);
            
            lineRenderer.SetPosition(0, transform.position);

            if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, lineDistance)) lineRenderer.SetPosition(1, hit.point);
            else lineRenderer.SetPosition(1, transform.position + transform.forward * lineDistance);
            
            transform.forward += Vector3.Slerp(transform.forward, shootDirection, rotateSpeed * Time.deltaTime);
        }
        else if(inputVector == Vector2.zero) lineRenderer.enabled = false;   
    }
}

public class ThrowAttackTrail : IThrowAttackTrail
{
    public void Aim(LineRenderer lineRenderer, Vector2 inputVector, Vector3 shootDirection, Vector3[] bulletPoints, Transform transform, GameObject player, float YLinePower, float rotateSpeed)
    {
        lineRenderer.positionCount = 10;

        if(inputVector != Vector2.zero)
        {
            lineRenderer.enabled = true;

            transform.position = new Vector3(player.transform.position.x, 0.1f, player.transform.position.z);
            
            lineRenderer.SetPosition(0, transform.position);

            for(int i = 1; i < 10; i++)
            {
                lineRenderer.SetPosition(i, new Vector3(lineRenderer.GetPosition(i-1).x + inputVector.x, i == 1 ? 0.5f : Mathf.Cos(YLinePower * (i * 0.1f)) * (i * 0.5f), lineRenderer.GetPosition(i-1).z + inputVector.y));

                bulletPoints[i-1] = lineRenderer.GetPosition(i); 
            }
            
            transform.forward += Vector3.Slerp(transform.forward, shootDirection, rotateSpeed * Time.deltaTime);
        }
        else if(inputVector == Vector2.zero) lineRenderer.enabled = false;   
    }
}

public class AttackTrail : MonoBehaviour
{
    #region EnumVariables
    private enum Mode {
        Shoot, Throw
    }

    [SerializeField] private Mode mode;
    #endregion

    #region InterfaceVariables
    private IShootAttackTrail shootAttackTrail;
    private IThrowAttackTrail throwAttackTrail;
    #endregion

    #region FloatVariables
    [SerializeField] private float rotateSpeed;

    [Header("For Shoot")]
    [SerializeField] private float lineDistance = 10.0f;
    
    [Header("For Throw")]
    [SerializeField] private float YLinePower;
    #endregion

    #region VectorVariables
    private Vector2 inputVector;
    private Vector3 shootDirection;
    private Vector3[] bulletPoints;
    #endregion

    #region OtherRegion
    private LineRenderer lineRenderer;
    [Header("Player References")]
    [SerializeField] private GameObject player;
    private GameInputManager gameInputManager;
    #endregion

    void Start()
    {   
        gameInputManager = GameInputManager.Instance;
        lineRenderer = GetComponent<LineRenderer>();

        shootAttackTrail = new ShootAttackTrail();
        throwAttackTrail = new ThrowAttackTrail();

        bulletPoints = new Vector3[9];
    }

    void Update()
    {
        if(mode == Mode.Shoot) 
        {
            inputVector = gameInputManager.GetShootAimVectorNormalized();
            shootDirection = new Vector3(inputVector.x, 0f, inputVector.y);

            shootAttackTrail.Aim(lineRenderer, inputVector, shootDirection, transform, player, lineDistance, rotateSpeed);
        }
    
        if(mode == Mode.Throw) 
        {
            inputVector = gameInputManager.GetThrowAimVector();
            shootDirection = new Vector3(inputVector.x, 0f, inputVector.y);

            throwAttackTrail.Aim(lineRenderer, inputVector, shootDirection, bulletPoints, transform, player, YLinePower, rotateSpeed);
        }
    }

    // public void CheckMode()
    // {
    //     if(mode == Mode.Shoot) 
    //     {
    //         inputVector = gameInputManager.GetShootAimVectorNormalizedForPlayer1();
    //         shootDirection = new Vector3(inputVector.x, 0f, inputVector.y);

    //         shootAttackTrail.Aim(lineRenderer, inputVector, shootDirection, transform, player, lineDistance, rotateSpeed);
    //     }
    
    //     if(mode == Mode.Throw) 
    //     {
    //         inputVector = gameInputManager.GetThrowAimVectorForPlayer1();
    //         shootDirection = new Vector3(inputVector.x, 0f, inputVector.y);

    //         throwAttackTrail.Aim(lineRenderer, inputVector, shootDirection, bulletPoints, transform, player, YLinePower, rotateSpeed);
    //     }
    // }

    public bool InAttackMode()
    {
        return gameInputManager.GetShootAimVectorNormalized() != Vector2.zero || gameInputManager.GetThrowAimVector() != Vector2.zero;
    }

    public Vector3 GetShootDirection()
    {
        return shootDirection;
    }

    public float GetLineDistance()
    {
        return lineDistance;
    }

    public Vector3[] GetBulletPointsForThrow()
    {
        return bulletPoints;
    }

    public bool ShootMode()
    {
        return mode == Mode.Shoot;
    }

    public bool ThrowMode()
    {
        return mode == Mode.Throw;
    }
}
