using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image player1HealthBar;
    [SerializeField] private Image player2HealthBar;
    [SerializeField] private GameObject player2HealthUI;
    [SerializeField] private GameObject versusText;
    private GameManager gm;

    void Start()
    {
        gm = GameManager.Instance;
    }

    public void UpdateHealthBar(int playerIndex)
    {
        if(playerIndex == 1) player1HealthBar.fillAmount = gm.GetPlayerHealth(playerIndex)/100;
        if(playerIndex == 2) player2HealthBar.fillAmount = gm.GetPlayerHealth(playerIndex)/100;
    }

    public void ShowPlayer2HealthBar()
    {
        versusText.gameObject.SetActive(true);
        player2HealthUI.SetActive(true);
    }

    public void HidePlayer2HealthBar()
    {
        versusText.gameObject.SetActive(false);
        player2HealthUI.SetActive(false);
    }

    public void HideAll()
    {
        this.gameObject.SetActive(false);
    }

    public void ShowAll()
    {
        this.gameObject.SetActive(true);
    }   
}
