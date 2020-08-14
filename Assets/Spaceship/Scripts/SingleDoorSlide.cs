using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleDoorSlide : MonoBehaviour
{
    public enum DoubleSlidingDoorStatus
    {
        Closed,
        Open,
        Animating
    }

    private DoubleSlidingDoorStatus status = DoubleSlidingDoorStatus.Closed;

    private enum DoorDirection
    {
        X,
        Z
    }

    [SerializeField]
    public Transform doorTransform;    //	Left panel of the sliding door

    [SerializeField]
    private DoorDirection openDirection = DoorDirection.X;
    [SerializeField]
    private float slideDistance = 0.88f;        //	Sliding distance to open each panel the door

    private Vector3 doorClosedPosition;
    private Vector3 doorOpenPosition;

    [SerializeField]
    private float speed = 1f;                   //	Spped for opening and closing the door

    private int objectsOnDoorArea = 0;

    //	Sound Fx
    [SerializeField]
    private AudioClip doorOpeningSoundClip;
    [SerializeField]
    public AudioClip doorClosingSoundClip;

    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        Vector3 openPosition = Vector3.zero;

        float x = doorTransform.localPosition.x;
        float y = doorTransform.localPosition.y;
        float z = doorTransform.localPosition.z;

        switch (this.openDirection)
        {
            case DoorDirection.X:
                openPosition = new Vector3(x + slideDistance, y, z);
                break;
            case DoorDirection.Z:
                openPosition = new Vector3(x, y + slideDistance, z);
                break;
        }

        doorClosedPosition = new Vector3(x, y, z);
        doorOpenPosition = openPosition;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (status != DoubleSlidingDoorStatus.Animating)
        {
            if (status == DoubleSlidingDoorStatus.Open)
            {
                if (objectsOnDoorArea == 0)
                {
                    StartCoroutine("CloseDoors");
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (status != DoubleSlidingDoorStatus.Animating)
        {
            if (status == DoubleSlidingDoorStatus.Closed && this.enabled)
            {
                StartCoroutine("OpenDoors");
            }
        }

        if (other.CompareTag("Player"))//GetComponent<Collider>().gameObject.layer == LayerMask.NameToLayer("Characters"))
        {
            objectsOnDoorArea++;
        }
    }

    void OnTriggerStay(Collider other)
    {

    }

    void OnTriggerExit(Collider other)
    {
        //	Keep tracking of objects on the door
        if (other.CompareTag("Player"))//GetComponent<Collider>().gameObject.layer == LayerMask.NameToLayer("Characters"))
        {
            objectsOnDoorArea--;
        }
    }

    IEnumerator OpenDoors()
    {

        if (doorOpeningSoundClip != null)
        {
            audioSource.PlayOneShot(doorOpeningSoundClip, 0.7F);
        }

        status = DoubleSlidingDoorStatus.Animating;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;

            doorTransform.localPosition = Vector3.Slerp(doorClosedPosition, doorOpenPosition, t);

            yield return null;
        }

        status = DoubleSlidingDoorStatus.Open;

    }

    IEnumerator CloseDoors()
    {

        if (doorClosingSoundClip != null)
        {
            audioSource.PlayOneShot(doorClosingSoundClip, 0.7F);
        }

        status = DoubleSlidingDoorStatus.Animating;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;

            doorTransform.localPosition = Vector3.Slerp(doorOpenPosition, doorClosedPosition, t);

            yield return null;
        }

        status = DoubleSlidingDoorStatus.Closed;

    }

    //	Forced door opening
    public bool DoOpenDoor()
    {

        if (status != DoubleSlidingDoorStatus.Animating)
        {
            if (status == DoubleSlidingDoorStatus.Closed)
            {
                StartCoroutine("OpenDoors");
                return true;
            }
        }

        return false;
    }

    //	Forced door closing
    public bool DoCloseDoor()
    {

        if (status != DoubleSlidingDoorStatus.Animating)
        {
            if (status == DoubleSlidingDoorStatus.Open)
            {
                StartCoroutine("CloseDoors");
                return true;
            }
        }

        return false;
    }
}
