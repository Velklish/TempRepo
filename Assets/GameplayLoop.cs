using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayLoop : MonoBehaviour
{
    [SerializeField] public GameObject deathScreen;

    public void SpawnDeathWindow()
    {
        deathScreen.SetActive(true);
    }
    
}
