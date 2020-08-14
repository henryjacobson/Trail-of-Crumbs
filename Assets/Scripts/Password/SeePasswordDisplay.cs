using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeePasswordDisplay : MonoBehaviour
{
    public static List<string> seenCharacters { get => GetSeenCharacters(); }
    private static List<SeePasswordDisplay> spd = new List<SeePasswordDisplay>();

    [SerializeField]
    private DisplayPassword dp;

    private bool seen;

    private bool savedByCheckpoint;

    void Awake()
    {
        spd.Add(this);
    }

    public static List<string> GetSeenCharacters()
    {
        List<string> result = new List<string>();
        foreach(SeePasswordDisplay s in spd)
        {
            if (s.HasSeen())
            {
                result.Add(s.GetText());
            }
        }
        return result;
    }

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
    }

    public bool HasSeen()
    {
        return this.seen;
    }

    public string GetText()
    {
        return this.dp.GetText();
    }
}
