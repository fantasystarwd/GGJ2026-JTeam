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
        AudioManager.Instance.PlaySFX(SoundEffectType.Button);
        SceneManager.LoadScene(sceneIndex);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(buildIndex);
    }
}
