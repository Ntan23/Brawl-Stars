using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{
    private PlayerInputManager playerInputManager;
    private List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField] private GameObject player2Prefabs;
    [SerializeField] private List<Transform> startingPoints;

    void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    public void AddPlayer(PlayerInput player)
    {
        players.Add(player);
 
        //need to use the parent due to the structure of the prefab
        Transform playerParent = player.transform.parent;
        playerParent.position = startingPoints[players.Count - 1].position;
    }

    public void SwitchNextPlayerPrefab(PlayerInput input)
    {
        playerInputManager.playerPrefab = player2Prefabs; 
    } 
}
