using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWinUI : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button mainMenuButton;
   
    void Start()
    {
        retryButton.onClick.AddListener(() => {
            SceneLoader.ReloadScene();
        });

        mainMenuButton.onClick.AddListener(() => {
            Debug.Log("Main Menu");
        });

        gameObject.SetActive(false);
    }

    public void ShowWinUI()
    {
        gameObject.SetActive(true);

        void UpdateAlpha(float alpha)
        {
            GetComponent<CanvasGroup>().alpha = alpha;

            if(alpha == 1) 
            {
                retryButton.interactable = true;
                mainMenuButton.interactable = true;
            }
        }

        LeanTween.value(gameObject, UpdateAlpha, 0.0f, 1.0f, 1.0f);
    }
}
