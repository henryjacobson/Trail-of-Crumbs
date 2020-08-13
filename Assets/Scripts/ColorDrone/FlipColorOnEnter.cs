using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipColorOnEnter : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("GrappleHand"))
        {
            ColorFlipper.FlipColor();
        }
    }
}
