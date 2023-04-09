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

    #region EnumVariables
    private enum State {
        IsPlaying, GameOver
    }

    private State gameState;
    #endregion

    private float player1Heath;
    private float player2Health;
    private int playerJoinCount;
    [SerializeField] private HealthBarUI healthBarUI;
    [SerializeField] private PlayerWinUI[] playerWinUI;
    private AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.Instance;

        player1Heath = 100;
        player2Health = 100;

        playerJoinCount = 0;

        gameState = State.IsPlaying;
        healthBarUI.HideAll();
    }

    public void DecreasePlayerHealth(int playerIndex, float damage)
    {
        if(gameState == State.IsPlaying)
        {
            if(playerIndex == 1 && player1Heath > 0) player1Heath -= damage;
            if(playerIndex == 2 && player2Health > 0) player2Health -= damage;

            healthBarUI.UpdateHealthBar(playerIndex);
            CheckWhoWin();
        }
    }

    void CheckWhoWin()
    {
        if(player2Health <= 0)
        {
            playerWinUI[0].ShowWinUI();
            audioManager.PlayWinSFX();
            gameState = State.GameOver;
        } 

        if(player1Heath <= 0) 
        {
            playerWinUI[1].ShowWinUI();
            audioManager.PlayWinSFX();
            gameState = State.GameOver;
        }
        
        if(player1Heath <= 0 && player2Health <= 0) 
        {
            playerWinUI[2].ShowWinUI();
            audioManager.PlayWinSFX();
            gameState = State.GameOver;
        }
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

    public bool IsPlaying()
    {
        return gameState == State.IsPlaying;
    }
}
