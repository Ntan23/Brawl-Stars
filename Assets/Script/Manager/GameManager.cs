using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance {get; private set;}

    void Awake()
    {
        if(Instance == null) Instance = this;
    }
    #endregion

    private float player1Heath;
    private float player2Health;
    private int playerJoinCount;
    [SerializeField] private HealthBarUI healthBarUI;

    void Start()
    {
        player1Heath = 100;
        player2Health = 100;

        playerJoinCount = 0;

        healthBarUI.HideAll();
    }

    public void DecreasePlayerHealth(int playerIndex, float damage)
    {
        if(playerIndex == 1 && player1Heath > 0) player1Heath -= damage;
        if(playerIndex == 2 && player2Health > 0) player2Health -= damage;

        healthBarUI.UpdateHealthBar(playerIndex);
        CheckWhoWin();
    }

    void CheckWhoWin()
    {
        if(player1Heath <= 0) Debug.Log("Player 2 Win");
        if(player2Health <= 0) Debug.Log("Player 1 Win");
    }

    public float GetPlayerHealth(int playerIndex)
    {
        if(playerIndex == 1) return player1Heath;
        if(playerIndex == 2) return player2Health;
        else return 0;
    }

    public void OnPlayerJoin()
    {
        playerJoinCount++;

        if(!healthBarUI.gameObject.activeInHierarchy) healthBarUI.ShowAll();

        if(playerJoinCount == 1) healthBarUI.HidePlayer2HealthBar();
        if(playerJoinCount == 2) healthBarUI.ShowPlayer2HealthBar();
    }
}
