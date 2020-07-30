using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyObject : MonoBehaviour
{
    [SerializeField]
    private DoorLockPad doorLockPad;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (this.doorLockPad.gameObject == other.gameObject)
        {
            this.doorLockPad.Unlock();
            Destroy(this.gameObject);
        }
    }
}
