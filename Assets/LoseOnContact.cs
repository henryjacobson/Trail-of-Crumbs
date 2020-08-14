using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseOnContact : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<LevelManager>().LevelLost();
        }
    }
}
