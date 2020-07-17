using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObjectBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("GrappleHand"))
        {
            this.transform.parent = null;
        }
    }
}
