using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private int buildIndex;
    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        buildIndex = currentScene.buildIndex;
    }

    public void LoadNextScene(int nextSceneIndex)
    {
        SceneManager.LoadScene(nextSceneIndex);
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(buildIndex);
    }
}
