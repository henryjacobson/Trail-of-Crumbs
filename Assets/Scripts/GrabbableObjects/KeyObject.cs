using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyObject : MonoBehaviour
{
    [SerializeField]
    private DoorLockPad doorLockPad;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GrappleHand"))
        {
            gameObject.transform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
        }

        if (this.doorLockPad.gameObject == other.gameObject)
        {
            this.doorLockPad.Unlock();
            Destroy(this.gameObject);
        }
    }
}
