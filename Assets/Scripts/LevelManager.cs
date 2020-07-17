using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static bool isGameOver;

    [SerializeField]
    private string nextLevel;
    [SerializeField]
    private Text gameOverText;

    void Start()
    {
        isGameOver = false;
    }

    void Update()
    {
        
    }

    public void LevelLost()
    {
        this.SetGameOverText("YOU GOT CAUGHT");

        isGameOver = true;

        Invoke("LoadThisLevel", 2);
    }

    public void LevelWon()
    {
        this.SetGameOverText("OBJECTIVE COMPLETE");

        isGameOver = true;

        Invoke("LoadNextLevel", 2);
    }

    private void SetGameOverText(string text)
    {
        this.gameOverText.text = text;
    }

    private void LoadThisLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void LoadNextLevel()
    {
        if (this.nextLevel != "")
        {
            SceneManager.LoadScene(this.nextLevel);
        }
    }
}
