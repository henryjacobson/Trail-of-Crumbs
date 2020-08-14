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

    private static List<PodBreak> podCache = new List<PodBreak>();

    private GameObject brokenPodSpawned;

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
            this.brokenPodSpawned = Instantiate(brokenPod, currTransform.position, currTransform.rotation);
            this.ClearPod();
        }
    }

    private void ClearPod()
    {
        podCache.Add(this);

        this.gameObject.SetActive(false);

        this.OnDestroy();
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

    public static void ClearPodCache()
    {
        podCache = new List<PodBreak>();
    }
    
    public static void ResetPodCache()
    {
        foreach(PodBreak pb in podCache)
        {
            pb.ReplacePod();
            pb.LockDoor();
        }

        ClearPodCache();
    }

    public void ReplacePod()
    {
        if (this.isBroken)
        {
            Destroy(this.brokenPodSpawned);
            this.gameObject.SetActive(true);
            this.isBroken = false;
        }
    }

    public void LockDoor()
    {
        if (!this.isBroken)
        {
            this.doorLockPad.Lock();
        }
    }
}
