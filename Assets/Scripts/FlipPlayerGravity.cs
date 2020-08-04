using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipPlayerGravity : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.SendMessage("FlipGravity");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.SendMessage("FlipGravity");
        }
    }
}
