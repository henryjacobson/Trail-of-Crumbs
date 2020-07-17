using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHandController : MonoBehaviour
{
    [HideInInspector]
    public ControlState controlState;

    private ControlState previousControlState;

    [SerializeField]
    private GameObject player;
    private PlayerWithGrappleBehaviour playerGrappleBehaviour;
    [SerializeField]
    private GameObject camera;
    [SerializeField]
    private GameObject returnPointPrefab;
    [SerializeField]
    private KeyCode launchKey = KeyCode.LeftShift;
    [SerializeField]
    private float speed = 10;
    [SerializeField]
    private float maxDistance = 20;
    [SerializeField]
    private float distanceToGrappleToStop = 2;

    private Rigidbody rb;
    private CharacterController playerCC;

    private Transform returnPoint;

    private string grabbableWallTag = "GrabbableWall";

    private string grabbableItemTag = "GrabbableItem";
    private List<Transform> items;

    void Start()
    {
        GameObject returnPoint = Instantiate(this.returnPointPrefab);
        returnPoint.transform.position = this.transform.position;
        returnPoint.transform.SetParent(this.player.transform);
        this.returnPoint = returnPoint.transform;

        this.playerCC = player.GetComponent<CharacterController>();
        this.rb = this.GetComponent<Rigidbody>();

        this.player.layer = LayerMask.NameToLayer("Player");
        this.playerGrappleBehaviour = this.player.AddComponent<PlayerWithGrappleBehaviour>();
        this.playerGrappleBehaviour.SetGrapple(this.gameObject);

        this.resetToResting();
        this.previousControlState = this.controlState;

        this.items = new List<Transform>();
    }

    public void resetToResting()
    {
        this.controlState = ControlState.Resting;
        this.transform.SetParent(this.player.transform);
        this.EnforceRestingPosition();
    }

    void Update()
    {
        if (!LevelManager.isGameOver)
        {
            this.CheckForStateChange();
            switch (this.controlState)
            {
                case ControlState.Resting:
                    this.RestingUpdate();
                    break;
                case ControlState.Launching:
                    this.LaunchingUpdate();
                    break;
                case ControlState.Retracting:
                    this.RetractingUpdate();
                    break;
                case ControlState.PullingPlayer:
                    this.PullingPlayerUpdate();
                    break;
            }
        }
    }

    private void CheckForStateChange()
    {
        if (this.previousControlState != this.controlState)
        {
            this.player.SendMessage("GrappleStateChanged", this.controlState);
        }
        this.previousControlState = this.controlState;
    }

    private void UpdateItemList()
    {
        this.items.Clear();
        foreach(Transform child in this.transform)
        {
            if (child.CompareTag(this.grabbableItemTag))
            {
                this.items.Add(child);
            }
        }
    }

    private void RestingUpdate()
    {
        this.UpdateItemList();
        if (Input.GetKeyDown(this.launchKey))
        {
            this.controlState = ControlState.Launching;
            this.transform.parent = null;
        } else
        {
            this.EnforceRestingPosition();
        }
    }

    private void EnforceRestingPosition()
    {
        this.transform.localPosition = this.returnPoint.localPosition;

        if (this.camera == null)
        {
            this.transform.localRotation = Quaternion.identity;
        } else
        {
            this.transform.localRotation = Quaternion.identity * Quaternion.AngleAxis(this.camera.transform.eulerAngles.x, Vector3.right);
        }
    }

    private void LaunchingUpdate()
    {
        Vector3 pointA = this.transform.position + this.transform.forward * 0.25f;
        Vector3 pointB = this.transform.position - this.transform.forward * 0.25f;
        LayerMask mask = ~LayerMask.GetMask("Light");
        Collider[] colliders = Physics.OverlapCapsule(pointA, pointB, 0.25f, mask);

        bool grabTriggerFound = false;
        bool obstructionFound = false;
        foreach(Collider c in colliders)
        {
            if (this.IsObjectGrabbable(c.gameObject) && this.ObjectNotThisOrChild(c.gameObject))
            {
                grabTriggerFound = true;
            } else if (this.ObjectNotThisOrChild(c.gameObject))
            {
                obstructionFound = true;
            }
        }
        if (!grabTriggerFound && obstructionFound)
        {
            this.controlState = ControlState.Retracting;
        }
    }

    private bool IsObjectGrabbable(GameObject g)
    {
        return g.CompareTag(this.grabbableWallTag) || g.CompareTag(this.grabbableItemTag);
    }

    private bool ObjectNotThisOrChild(GameObject g)
    {
        foreach (Transform child in this.transform)
        {
            if (g == child.gameObject)
            {
                return false;
            }
        }

        return g != this.gameObject && g != this.player;
    }

    private void RetractingUpdate()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, this.returnPoint.position, this.speed * Time.deltaTime);
        if (this.transform.position == this.returnPoint.position)
        {
            this.resetToResting();
        }
    }

    private void PullingPlayerUpdate()
    {
        Vector3 toHook = this.transform.position - this.player.transform.position;
        Vector3 offset = toHook.normalized * this.speed * Time.deltaTime;
        this.playerCC.Move(offset);
        if (toHook.magnitude <= this.distanceToGrappleToStop)
        {
            if (Input.GetKeyDown(this.launchKey))
            {
                this.resetToResting();
            }
        } 
    }

    void FixedUpdate()
    {
        if (!LevelManager.isGameOver)
        {
            switch (this.controlState)
            {
                case ControlState.Resting:
                    this.RestingFixedUpdate();
                    break;
                case ControlState.Launching:
                    this.LaunchingFixedUpdate();
                    break;
                case ControlState.Retracting:
                    this.RetractingFixedUpdate();
                    break;
                case ControlState.PullingPlayer:
                    this.PullingPlayerFixedUpdate();
                    break;
            }
        }
    }

    private void RestingFixedUpdate()
    {

    }

    private void LaunchingFixedUpdate()
    {
        Vector3 offset = this.transform.position + (this.transform.forward * this.speed * Time.fixedDeltaTime);
        this.rb.MovePosition(offset);

        float distanceFromPlayer = (this.transform.position - this.player.transform.position).magnitude;
        if (distanceFromPlayer >= this.maxDistance)
        {
            this.controlState = ControlState.Retracting;
        }
    }

    private void RetractingFixedUpdate()
    {

    }

    private void PullingPlayerFixedUpdate()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.items.Count == 0 && this.controlState != ControlState.PullingPlayer && this.controlState != ControlState.Resting)
        {
            if (other.CompareTag(this.grabbableWallTag))
            {
                this.controlState = ControlState.PullingPlayer;
            }

            if (other.CompareTag(this.grabbableItemTag))
            {
                this.PickUpObject(other.gameObject);
                this.controlState = ControlState.Retracting;
            }
        }
    }

    private void PickUpObject(GameObject g)
    {
        g.transform.SetParent(this.transform);
        g.transform.localRotation = Quaternion.identity;
        g.transform.localPosition = Vector3.zero;
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        this.transform.position = collision.GetContact(0).point;
        if (this.controlState != ControlState.PullingPlayer && this.controlState != ControlState.Resting)
        {
            this.controlState = ControlState.Retracting;
        }
    }
    */
}

public enum ControlState
{
    Resting, Launching, Retracting, PullingPlayer
}
