using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLockPad : MonoBehaviour
{
    [SerializeField]
    private DoorSlide lockedDoor;

    private bool isUnlocked;

    void Update()
    {
        this.lockedDoor.enabled = this.isUnlocked;
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
