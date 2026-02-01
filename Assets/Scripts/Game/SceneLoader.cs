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

    public void LoadNextScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        //0: Starting
        //1: Tutorial
        //2: AlphaShow
        //3: SampleScene
        //4: Gameplay
        //5: Ending
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(buildIndex);
    }
}
