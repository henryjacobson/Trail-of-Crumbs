using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBehavior : MonoBehaviour
{
    public static event Action onSetCheckpoint; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<LevelManager>().SetCheckPoint(other.transform.position);
            PodBreak.ClearPodCache();
            CacheOnCheckpoint.ClearCache();
            Destroy(gameObject);
            onSetCheckpoint();
        }
    }
}
