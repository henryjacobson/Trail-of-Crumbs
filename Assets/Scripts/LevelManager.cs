using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static bool isGameOver;

    [SerializeField]
    private Text caughtText;

    void Start()
    {
        isGameOver = false;
    }

    void Update()
    {
        
    }

    public void LevelLost()
    {
        this.SetCaughtText("YOU GOT CAUGHT");

        isGameOver = true;

        Invoke("LoadThisLevel", 2);
    }

    private void SetCaughtText(string text)
    {
        this.caughtText.text = text;
    }

    private void LoadThisLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
