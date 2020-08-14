using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayPasswordChunks : MonoBehaviour
{
    [SerializeField]
    private GameObject chunkPrefab;

    private int previousListCount;

    void Start()
    {
        this.DrawPasswordBar();
        this.previousListCount = this.GetSeenCharacters().Count;
    }

    void Update()
    {
        if (this.previousListCount != this.GetSeenCharacters().Count)
        {
            this.DrawPasswordBar();
        }
        this.previousListCount = this.GetSeenCharacters().Count;
    }

    private void DrawPasswordBar()
    {
        this.DestroyAllChunks();
        foreach(string s in this.GetSeenCharacters())
        {
            TMP_Text text = Instantiate(this.chunkPrefab, this.transform).GetComponentInChildren<TMP_Text>();
            text.text = s;
        }
    }

    private void DestroyAllChunks()
    {
        foreach(Transform chunk in this.transform)
        {
            Destroy(chunk.gameObject);
        }
    }

    private List<string> GetSeenCharacters()
    {
        return SeePasswordDisplay.seenCharacters;
    }
}
