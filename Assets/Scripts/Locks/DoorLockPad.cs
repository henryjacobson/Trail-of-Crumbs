using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLockPad : MonoBehaviour
{
    [SerializeField]
    private DoorSlide lockedDoor;
    [SerializeField]
    private Color lockedColor;
    [SerializeField]
    private Color unlockedColor;
    [SerializeField]
    private AudioClip unlockSFX;

    [SerializeField]
    private bool useFX = true;

    private bool isUnlocked;

    private Renderer renderer;

    void Start()
    {
        this.renderer = this.GetComponent<Renderer>();
    }

    void Update()
    {
        this.lockedDoor.enabled = this.isUnlocked;

        if (this.useFX)
        {
            this.renderer.material.SetColor("_EmissionColor", this.isUnlocked ? this.unlockedColor : this.lockedColor);
        }
    }

    public void Unlock()
    {
        if (this.useFX)
        {
            AudioSource.PlayClipAtPoint(this.unlockSFX, this.transform.position);
        }
        this.SetUnlock(true);
    }

    public void Lock()
    {
        this.SetUnlock(false);
    }

    private void SetUnlock(bool isUnlocked)
    {
        this.isUnlocked = isUnlocked;
    }
}
