using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Settings settings;

    void Start()
    {
        playButton.onClick.AddListener(() => {
            SceneLoader.Load(SceneLoader.Scene.GameScene);
        });

        settingsButton.onClick.AddListener(() => {
            settings.ShowSettings();
        });

        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }
}
