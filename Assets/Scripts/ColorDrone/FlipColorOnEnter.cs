using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipColorOnEnter : MonoBehaviour
{
    [SerializeField]
    private AudioClip flipSFX;

    private Renderer renderer;

    void Start()
    {
        this.renderer = this.GetComponent<Renderer>();

        this.SetColor();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("GrappleHand"))
        {
            this.OnFlip();
        }
    }

    private void OnFlip()
    {
        AudioSource.PlayClipAtPoint(this.flipSFX, this.transform.position);

        ColorFlipper.FlipColor();

        this.SetColor();
    }

    private void SetColor()
    {
        switch (ColorFlipper.activeColor)
        {
            case ColorDroneSpotlightColor.Blue:
                this.renderer.material.color = Color.cyan;
                break;
            case ColorDroneSpotlightColor.Red:
                this.renderer.material.color = Color.red;
                break;
            default:
                return;
        }
    }
}
