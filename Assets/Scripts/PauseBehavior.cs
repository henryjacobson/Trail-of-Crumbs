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
    Player_Movement playerMovement;
    Camera_Control[] cameraControls;
    GrappleHandController grappleHand;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<Player_Movement>();
        cameraControls = player.GetComponentsInChildren<Camera_Control>();
        grappleHand = player.GetComponentInChildren<GrappleHandController>();
        UI = GameObject.FindGameObjectWithTag("UI");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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

        playerMovement.enabled = false;
        grappleHand.enabled = false;
        foreach (Camera_Control camera in cameraControls)
        {
            camera.enabled = false;
        }
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

        playerMovement.enabled = true;
        grappleHand.enabled = true; 
        foreach (Camera_Control camera in cameraControls)
        {
            camera.enabled = true;
        }
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
