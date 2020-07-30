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

    private bool isUnlocked;

    private Renderer renderer;

    void Start()
    {
        this.renderer = this.GetComponent<Renderer>();
    }

    void Update()
    {
        this.lockedDoor.enabled = this.isUnlocked;

        this.renderer.material.SetColor("_EmissionColor", this.isUnlocked ? this.unlockedColor : this.lockedColor);
    }

    public void Unlock()
    {
        this.SetUnlock(true);
    }

    private void SetUnlock(bool isUnlocked)
    {
        this.isUnlocked = isUnlocked;
    }
}
