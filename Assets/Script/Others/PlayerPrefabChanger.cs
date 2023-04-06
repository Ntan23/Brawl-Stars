using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPrefabChanger : MonoBehaviour
{
    private PlayerInputManager playerInputManager;
    [SerializeField] private GameObject player2Prefabs;

    void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    public void SwitchNextPlayerPrefab(PlayerInput input)
    {
        playerInputManager.playerPrefab = player2Prefabs; 
    }   
}
