using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    private static CheckpointManager instance;

    private static int sceneId;

    private Transform player;

    private Vector3 checkpointPosition;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            sceneId = SceneManager.GetActiveScene().buildIndex;
            DontDestroyOnLoad(this.gameObject);
        } else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        this.player = this.InitPlayerTransform();
        this.checkpointPosition = this.player.position;

        SceneManager.sceneLoaded += this.Checkpoint;
    }

    private void Checkpoint(Scene scene, LoadSceneMode mode)
    {
        this.player = this.InitPlayerTransform();

        if (scene.buildIndex == sceneId)
        {
            this.WarpPlayer(this.checkpointPosition);
            InvokeRepeating("TestPlayerActive", 2, 1);
        } else
        {
            this.checkpointPosition = this.player.position;
            sceneId = scene.buildIndex;
        }
    }

    void TestPlayerActive()
    {
        Debug.Log(this.player.position + ", " + this.checkpointPosition);
        this.WarpPlayer(this.checkpointPosition);
    }

    private Transform InitPlayerTransform()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            return this.transform;
        } else
        {
            return player.transform;
        }
    }

    public void SetCheckPoint(Vector3 position)
    {
        this.checkpointPosition = position;
        DisplayMessage("CHECKPOINT REACHED");
        Invoke("ClearMessage", 2);
    }

    private void DisplayMessage(string text)
    {
        if (!LevelManager.isGameOver)
        {
            GameObject.FindObjectOfType<LevelManager>().SetGameOverText(text);
        }
    }

    private void ClearMessage()
    {
        this.DisplayMessage("");
    }

    private void WarpPlayer(Vector3 newPos)
    {
        CharacterController cc = this.player.gameObject.GetComponent<CharacterController>();
        cc.enabled = false;
        this.player.position = newPos;
        cc.enabled = true;
    }
}
