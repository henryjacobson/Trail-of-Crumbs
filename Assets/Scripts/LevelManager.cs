using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static bool isGameOver;

    public static event Action onLevelReset;

    [SerializeField]
    private string nextLevel;
    [SerializeField]
    private Text gameOverText;
    public bool canBreakPods;
    public Transform player;
    private Vector3 checkpointPosition;
    [SerializeField]
    private AudioClip loseSFX;
    [SerializeField]
    private AudioClip winSFX;

    void Start()
    {
        isGameOver = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        checkpointPosition = player.position;
    }

    public void LevelLost()
    {
        this.SetGameOverText("YOU GOT CAUGHT");

        AudioSource.PlayClipAtPoint(this.loseSFX, Camera.main.transform.position);

        isGameOver = true;

        Invoke("LoadThisLevel", 2);
    }

    public void LevelWon()
    {
        this.SetGameOverText("OBJECTIVE COMPLETE");

        AudioSource.PlayClipAtPoint(this.winSFX, Camera.main.transform.position);

        isGameOver = true;

        Invoke("LoadNextLevel", 2);
    }

    public void SetGameOverText(string text)
    {
        this.gameOverText.text = text;
        gameOverText.enabled = true;
    }

    private void LoadThisLevel()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        this.WarpPlayer(this.checkpointPosition);
        isGameOver = false;
        CellPrimeBehavior cellPrime = FindObjectOfType<CellPrimeBehavior>();
        if (cellPrime != null)
        {
            cellPrime.Checkpoint();
        }

        onLevelReset.Invoke();

        if (canBreakPods)
        {
            PodBreak.ResetPodCache();
        }

        FindObjectOfType<GrappleHandController>().DeactivatePowerups();

        GivePowerup.ResetPowerupTimers();

        CacheOnCheckpoint.ResetCache();

        FindObjectOfType<PowerUpSlider>().SetValue(0);

        FindObjectOfType<GrappleHandController>().ResetToResting();

        FindObjectOfType<GrappleHandController>().controlState = ControlState.Retracting;

        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();
        foreach(EnemyAI enemy in enemies)
        {
            enemy.state = EnemyAI.FSMStates.Patrol;
            enemy.anim.enabled = true;
        }

        DisableGameOverText();
    }

    private void WarpPlayer(Vector3 newPos)
    {
        CharacterController cc = this.player.gameObject.GetComponent<CharacterController>();
        cc.enabled = false;
        this.player.position = newPos;
        cc.enabled = true;
    }

    public void SetCheckPoint(Vector3 position)
    {
        checkpointPosition = position;
        SetGameOverText("CHECKPOINT REACHED");
        Invoke("DisableGameOverText", 2);
    }

    private void DisableGameOverText()
    {
        SetGameOverText("");
    }

    private void LoadNextLevel()
    {
        if (this.nextLevel != "")
        {
            SceneManager.LoadScene(this.nextLevel);
        }
    }
}
