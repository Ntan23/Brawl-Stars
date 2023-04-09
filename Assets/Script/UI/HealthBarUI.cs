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

        UpdateHealthBarColor(1);
        UpdateHealthBarColor(2);

        gameObject.SetActive(false);
    }

    public void UpdateHealthBar(int playerIndex)
    {
        if(playerIndex == 1) player1HealthBar.fillAmount = gm.GetPlayerHealth(playerIndex)/100;
        if(playerIndex == 2) player2HealthBar.fillAmount = gm.GetPlayerHealth(playerIndex)/100;

        UpdateHealthBarColor(playerIndex);
    }

    void UpdateHealthBarColor(int playerIndex)
    {
        if(playerIndex == 1) 
        {
            if(player1HealthBar.fillAmount > 0.5f) player1HealthBar.color = Color.green;
            else if(player1HealthBar.fillAmount <= 0.5f && player1HealthBar.fillAmount > 0.2f) player1HealthBar.color = Color.yellow;
            else if(player1HealthBar.fillAmount <= 0.2f) player1HealthBar.color = Color.red;
        }

        if(playerIndex == 2)
        {
            if(player2HealthBar.fillAmount > 0.5f) player2HealthBar.color = Color.green;
            else if(player2HealthBar.fillAmount <= 0.7f && player2HealthBar.fillAmount > 0.2f) player2HealthBar.color = Color.yellow;
            else if(player2HealthBar.fillAmount <= 0.2f) player2HealthBar.color = Color.red;
        }
    }

    public void ShowPlayer2HealthBar()
    {
        player2HealthUI.SetActive(true);
        versusText.gameObject.SetActive(true);

        void UpdateAlpha(float alpha)
        {
            player2HealthUI.GetComponent<CanvasGroup>().alpha = alpha;
        }

        LeanTween.value(player2HealthUI, UpdateAlpha, 0.0f, 1.0f, 1.0f);
        LeanTween.scale(versusText.gameObject, new Vector3(1.2f, 1.2f, 1.2f), 0.5f).setEaseInElastic();
    }

    public void HidePlayer2HealthBar()
    {
        versusText.gameObject.SetActive(false);
        player2HealthUI.SetActive(false);
    }

    public void HideAll() => gameObject.SetActive(false);

    public void ShowAll()
    {
        gameObject.SetActive(true);

        void UpdateAlpha(float alpha)
        {
            GetComponent<CanvasGroup>().alpha = alpha;
        }

        LeanTween.value(gameObject, UpdateAlpha, 0.0f, 1.0f, 1.0f);
    }   
}
