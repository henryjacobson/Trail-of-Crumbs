using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodBreak : MonoBehaviour
{
    public bool isBroken;
    [SerializeField]
    private DoorLockPad doorLockPad;
    public GameObject brokenPod;
    public AudioClip glassBreaking;
    public int carNumber;

    private void Start()
    {
        isBroken = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GrappleHand"))
        {
            Transform currTransform = gameObject.transform;
            AudioSource.PlayClipAtPoint(glassBreaking, currTransform.position);
            //Debug.Log("here");
            isBroken = true;
            Instantiate(brokenPod, currTransform.position, currTransform.rotation);
            Destroy(gameObject, 0.15f);
        }
    }

    private void OnDestroy()
    {
        bool allBroken = true;
        GameObject[] pods = GameObject.FindGameObjectsWithTag("PodCar" + carNumber);
        foreach (GameObject pod in pods)
        {
            allBroken = pod.GetComponent<PodBreak>().isBroken;
        }

        if (allBroken)
        {
            this.doorLockPad.Unlock();
        }
    }
}
