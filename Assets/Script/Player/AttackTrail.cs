using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrail : MonoBehaviour
{
    #region FloatVariables
    private float lineDistance = 1.0f;
    [SerializeField] private float rotateSpeed;
    #endregion

    #region VectorVariables
    private Vector2 inputVector;
    private Vector3 attackDirection;
    private Vector3 attackDistance;
    #endregion

    #region OtherRegion
    [SerializeField] private LineRenderer lineRenderer;
    private RaycastHit hit;
    private GameInputManager gameInputManager;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        gameInputManager = GameInputManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        PointAndShoot();
    }

    void PointAndShoot()
    {
        inputVector = gameInputManager.GetAttackVectorNormalized();

        attackDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        
        if(attackDirection.x < 0 || attackDirection.x > 0 || attackDirection.z < 0 || attackDirection.z > 0)
        {
            if(!lineRenderer.gameObject.activeInHierarchy) lineRenderer.gameObject.SetActive(true);

            lineRenderer.SetPosition(0, transform.position);

            if(Physics.Raycast(transform.position, transform.forward, out hit, lineDistance)) lineRenderer.SetPosition(1, hit.point);
            else 
            {
                lineRenderer.SetPosition(1, transform.forward * lineDistance);
            }
      
            lineRenderer.transform.forward += Vector3.Slerp(lineRenderer.transform.forward, attackDirection, rotateSpeed * Time.deltaTime);
        }
        else lineRenderer.gameObject.SetActive(false);
        
    }
}
