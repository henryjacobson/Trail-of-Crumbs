using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipPlayerGravity : MonoBehaviour
{
    [SerializeField]
    private AudioClip flipSFX;
    [SerializeField]
    private AudioClip unflipSFX;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (this.flipSFX != null)
            {
                this.Play(this.flipSFX);
            }

            other.SendMessage("FlipGravity");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (this.unflipSFX != null)
            {
                this.Play(this.unflipSFX);
            }

            other.SendMessage("FlipGravity");
        }
    }

    private void Play(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }
}
