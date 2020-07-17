using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static bool isGameOver;

    void Start()
    {
        isGameOver = false;
    }

    void Update()
    {
        
    }

    public void LevelLost()
    {
        Debug.Log("Level Lost");

        isGameOver = true;

        Invoke("LoadThisLevel", 2);
    }

    private void LoadThisLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
