using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrail : MonoBehaviour
{
    #region FloatVariables
    [SerializeField] private float lineDistance = 10.0f;
    [SerializeField] private float rotateSpeed;
    #endregion

    #region VectorVariables
    private Vector2 inputVector;
    private Vector3 shootDirection;
    #endregion

    #region OtherRegion
    private LineRenderer lineRenderer;
    [SerializeField] private GameObject player;
    private RaycastHit hit;
    private GameInputManager gameInputManager;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        gameInputManager = GameInputManager.Instance;
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        PointAndShoot();
    }

    void PointAndShoot()
    {
        inputVector = gameInputManager.GetAttackVectorNormalized();

        shootDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        
        if(inputVector != Vector2.zero)
        {
            lineRenderer.enabled = true;

            transform.position = new Vector3(player.transform.position.x, 0.1f, player.transform.position.z);
            
            lineRenderer.SetPosition(0, transform.position);

            if(Physics.Raycast(transform.position, transform.forward, out hit, lineDistance)) lineRenderer.SetPosition(1, hit.point);
            else lineRenderer.SetPosition(1, transform.position + transform.forward * lineDistance);
            
            transform.forward += Vector3.Slerp(transform.forward, shootDirection, rotateSpeed * Time.deltaTime);
        }
        else if(inputVector == Vector2.zero) lineRenderer.enabled = false;   
    }

    public bool InAttackMode()
    {
        return gameInputManager.GetAttackVectorNormalized() != Vector2.zero;
    }

    public Vector3 GetShootDirection()
    {
        return shootDirection;
    }

    public float GetLineDistance()
    {
        return lineDistance;
    }
}
