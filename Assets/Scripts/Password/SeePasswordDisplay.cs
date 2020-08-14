using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeePasswordDisplay : MonoBehaviour
{
    public static List<string> seenCharacters = new List<string>();

    [SerializeField]
    private DisplayPassword dp;

    private bool seen;

    private bool savedByCheckpoint;

    void Start()
    {
        LevelManager.onLevelReset += this.Reset;
        CheckpointBehavior.onSetCheckpoint += this.OnCheckpoint;
    }

    void OnDestroy()
    {
        LevelManager.onLevelReset -= this.Reset;
        CheckpointBehavior.onSetCheckpoint -= this.OnCheckpoint;
    }

    private void Reset()
    {
        if (seenCharacters.Count > 0)
        {
            seenCharacters = new List<string>();
        }
        this.seen = false;
        this.RevertIfSaved();
    }

    private void RevertIfSaved()
    {
        if (this.savedByCheckpoint)
        {
            this.OnSeen();
        }
    }

    private void OnCheckpoint()
    {
        if (this.seen)
        {
            this.savedByCheckpoint = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!this.seen && other.CompareTag("Player"))
        {
            this.OnSeen();
        }
    }

    private void OnSeen()
    {
        string text = this.dp.GetText();
        seenCharacters.Add(text);
        this.seen = true;
        Debug.Log(text);
    }
}
