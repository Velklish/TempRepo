using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void LoadScene(string sceneName)
    {
        var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("sceneName");
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene.buildIndex);
    }
    
    public void LoadScene(int sceneIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }

}
