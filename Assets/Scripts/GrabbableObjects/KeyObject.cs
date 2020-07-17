using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyObject : MonoBehaviour
{
    public Collider lockedDoor;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (lockedDoor == other)
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
