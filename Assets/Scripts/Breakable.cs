using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public AudioClip brokenSFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GrappleHand"))
        {
            if (brokenSFX != null)
            {
                AudioSource.PlayClipAtPoint(brokenSFX, transform.position);
            }

            Destroy(gameObject, 0.25f);
        }
    }
}
