using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConductorTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<ConductorBehavior>().PlayerSeen();
            Destroy(gameObject);
        }
    }
}
