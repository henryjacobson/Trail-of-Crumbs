using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodBreak : MonoBehaviour
{
    public GameObject brokenPod;
    public AudioClip glassBreaking;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GrappleHand"))
        {
            Transform currTransform = gameObject.transform;
            Debug.Log("here");
            Instantiate(brokenPod, currTransform.position, currTransform.rotation);
            //AudioSource.PlayClipAtPoint(glassBreaking, currTransform.position);
            Destroy(gameObject);
        }
    }
}
