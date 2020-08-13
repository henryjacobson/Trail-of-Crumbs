using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyObject : MonoBehaviour
{
    [SerializeField]
    private DoorLockPad doorLockPad;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;
    private Transform initialParent;

    void Start()
    {
        this.initialParent = this.transform.parent;
        this.initialPosition = this.transform.localPosition;
        this.initialRotation = this.transform.localRotation;
        this.initialScale = this.transform.localScale;

        LevelManager.onLevelReset += this.Reset;
    }

    void OnDestroy()
    {
        LevelManager.onLevelReset -= this.Reset;
    }

    void Reset()
    {
        this.transform.parent = this.initialParent;
        this.transform.localPosition = this.initialPosition;
        this.transform.localRotation = this.initialRotation;
        this.transform.localScale = this.initialScale;
    }

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
