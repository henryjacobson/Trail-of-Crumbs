using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//	This script handles automatic opening and closing sliding doors
//	It is fired by triggers and the door closes if found no character in the trigger area


//	Door status
public enum DoubleSlidingDoorStatus
{
    Closed,
    Open,
    Animating
}

[RequireComponent(typeof(AudioSource))]
public class DoorSlide : MonoBehaviour
{

    private DoubleSlidingDoorStatus status = DoubleSlidingDoorStatus.Closed;

    private enum DoorDirection
    {
        X,
        Z
    }

    [SerializeField]
    private Transform halfDoorLeftTransform;    //	Left panel of the sliding door
    [SerializeField]
    public Transform halfDoorRightTransform;    //	Right panel of the sliding door

    [SerializeField]
    private DoorDirection openDirection = DoorDirection.X;
    [SerializeField]
    private float slideDistance = 0.88f;        //	Sliding distance to open each panel the door

    private Vector3 leftDoorClosedPosition;
    private Vector3 leftDoorOpenPosition;

    private Vector3 rightDoorClosedPosition;
    private Vector3 rightDoorOpenPosition;

    bool doubleDoors = true;

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
        if (halfDoorRightTransform == null)
        {
            doubleDoors = false;
        }
        
        Vector3 openPosition = Vector3.zero;
        switch(this.openDirection)
        {
            case DoorDirection.X:
                openPosition = new Vector3(slideDistance, 0, 0);
                break;
            case DoorDirection.Z:
                openPosition = new Vector3(0, 0, slideDistance);
                break;
        }

        leftDoorClosedPosition = new Vector3(0f, 0f, 0f);
        leftDoorOpenPosition = openPosition;

        if (doubleDoors)
        {
            rightDoorClosedPosition = halfDoorRightTransform.localPosition;
            rightDoorOpenPosition = rightDoorClosedPosition - openPosition;
        }

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
        if (other.CompareTag("Player"))
        {
            if (status != DoubleSlidingDoorStatus.Animating)
            {
                if (status == DoubleSlidingDoorStatus.Closed && this.enabled)
                {
                    StartCoroutine("OpenDoors");
                }
            }
            objectsOnDoorArea++;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (status != DoubleSlidingDoorStatus.Animating)
            {
                if (status == DoubleSlidingDoorStatus.Closed && this.enabled)
                {
                    StartCoroutine("OpenDoors");
                }
            }
            objectsOnDoorArea++;
        }
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

            halfDoorLeftTransform.localPosition = Vector3.Slerp(leftDoorClosedPosition, leftDoorOpenPosition, t);
            if (doubleDoors)
            {
                halfDoorRightTransform.localPosition = Vector3.Slerp(rightDoorClosedPosition, rightDoorOpenPosition, t);
            }

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

            halfDoorLeftTransform.localPosition = Vector3.Slerp(leftDoorOpenPosition, leftDoorClosedPosition, t);
            if (doubleDoors)
            {
                halfDoorRightTransform.localPosition = Vector3.Slerp(rightDoorOpenPosition, rightDoorClosedPosition, t);
            }

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
