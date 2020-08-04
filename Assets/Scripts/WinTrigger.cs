using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [SerializeField]
    private Collider objectToDetect;

    void OnTriggerEnter(Collider other)
    {
        if (other.Equals(this.objectToDetect))
        {
            GameObject.FindObjectOfType<LevelManager>().LevelWon();
        }
    }
}
