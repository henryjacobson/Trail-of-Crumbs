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

    private Rigidbody rb;
    private CharacterController playerCC;

    private Transform returnPoint;

    private string grabbableWallTag = "GrabbableWall";

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
    }

    public void resetToResting()
    {
        this.controlState = ControlState.Resting;
        this.transform.SetParent(this.player.transform);
        this.EnforceRestingPosition();
    }

    void Update()
    {
        Debug.Log(this.controlState);
        this.CheckForStateChange();
        switch(this.controlState)
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

    private void CheckForStateChange()
    {
        if (this.previousControlState != this.controlState)
        {
            this.player.SendMessage("GrappleStateChanged", this.controlState);
        }
        this.previousControlState = this.controlState;
    }

    private void RestingUpdate()
    {
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
        if (toHook.magnitude > 1)
        {
            Vector3 offset = toHook.normalized * this.speed * Time.deltaTime;
            this.playerCC.Move(offset);
        } else
        {
            this.controlState = ControlState.Resting;
        }
    }

    void FixedUpdate()
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
        if (other.CompareTag("GrabbableWall") && this.controlState != ControlState.PullingPlayer && this.controlState !=  ControlState.Resting)
        {
            this.controlState = ControlState.PullingPlayer;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        this.transform.position = collision.GetContact(0).point;
        if (this.controlState != ControlState.PullingPlayer && this.controlState != ControlState.Resting)
        {
            this.controlState = ControlState.Retracting;
        }
    }
}

public enum ControlState
{
    Resting, Launching, Retracting, PullingPlayer
}
