using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private PlayerController playerController;
    private bool canMove;

    void Start() => playerController = GetComponent<PlayerController>();

    public bool DetectCollision(Vector3 moveDirection)
    {
        if(playerController == null) playerController = GetComponent<PlayerController>();

        if(playerController !=  null) canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerController.GetPlayerHeight(), playerController.GetPlayerRadius(), moveDirection, playerController.GetMoveDistance());

        return canMove;
    }
}
