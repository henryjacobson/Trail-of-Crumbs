using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public AudioClip brokenSFX;

    private CacheOnCheckpoint coc;

    void Awake()
    {
        this.coc = this.gameObject.AddComponent<CacheOnCheckpoint>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GrappleHand"))
        {
            if (brokenSFX != null)
            {
                AudioSource.PlayClipAtPoint(brokenSFX, transform.position);
            }

            this.coc.OnCache(0.25f);
        }
    }
}
