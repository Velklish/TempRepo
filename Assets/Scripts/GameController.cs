using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private List<BasicPlayerController> _players;
    public GameObject deathScreen;
    public GameObject winScreen;
    
    private void Start()
    {
        _players = FindObjectsOfType<BasicPlayerController>().ToList();
        Debug.Log(_players.Count);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
    }

    public void ActivateDeathScreen(BasicPlayerController sender)
    {
        deathScreen.SetActive(true);
        _players = FindObjectsOfType<BasicPlayerController>().ToList();
        
        if (_players.Count == 1)
        {
            winScreen.SetActive(true);
        }
    }
}
