using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConductorTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GrappleHand"))
        {
            FindObjectOfType<ConductorBehavior>().PlayerSeen();
            other.gameObject.GetComponent<GrappleHandController>().controlState = ControlState.Retracting;
        }
    }
}
