using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public enum Scene {
        MainMenu, LoadingScene, GameScene
    }
    private static Scene targetScene;

    public static void Load(Scene targetScene)
    {
        SceneLoader.targetScene = targetScene;

        SceneManager.LoadSceneAsync(Scene.LoadingScene.ToString());
    }

    public static void SceneLoaderCallback()
    {
        SceneManager.LoadSceneAsync(targetScene.ToString());
    }

    public static void ReloadScene()
    {   
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
