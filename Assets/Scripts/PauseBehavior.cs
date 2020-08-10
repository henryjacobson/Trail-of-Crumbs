using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseBehavior : MonoBehaviour
{
    public static bool isPaused;

    public GameObject background;
    public GameObject mainMenu;
    public GameObject settings;

    GameObject UI;
    DisableControls disableControls;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        disableControls = player.GetComponent<DisableControls>();
        UI = GameObject.FindGameObjectWithTag("UI");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !IntroBehavior.intro)
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        mainMenu.SetActive(true);
        background.SetActive(true);
        UI.SetActive(false);

        Cursor.lockState = CursorLockMode.None;

        disableControls.Disable();
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;

        mainMenu.SetActive(false);
        settings.SetActive(false);
        background.SetActive(false);
        UI.SetActive(true);


        Cursor.lockState = CursorLockMode.Locked;

        disableControls.Enable();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
