using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHandController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private KeyCode launchKey = KeyCode.LeftShift;
    [SerializeField]
    private float speed = 10;
    [SerializeField]
    private float maxDistance = 20;

    private Rigidbody rb;
    private Rigidbody playerRb;

    private Vector3 restingOffset;
    private Vector3 globalRestingPosition;

    private string grabbableWallTag = "GrabbableWall";

    private enum ControlState
    {
        Resting, Launching, Retracting, PullingPlayer
    }

    private ControlState controlState;

    void Start()
    {
        this.playerRb = player.GetComponent<Rigidbody>();
        this.rb = this.GetComponent<Rigidbody>();
        controlState = ControlState.Resting;
        this.restingOffset = this.transform.position - this.player.transform.position;
    }

    void Update()
    {
        this.globalRestingPosition = this.player.transform.position + this.restingOffset;

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

    private void RestingUpdate()
    {
        this.transform.SetParent(this.player.transform);

        if (Input.GetKeyDown(this.launchKey))
        {
            this.controlState = ControlState.Launching;
            this.player.transform.DetachChildren();
        }
    }

    private void LaunchingUpdate()
    {

    }

    private void RetractingUpdate()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, this.globalRestingPosition, this.speed * Time.deltaTime);
        if (this.transform.position == this.player.transform.position)
        {
            this.controlState = ControlState.Resting;
        }
    }

    private void PullingPlayerUpdate()
    {

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
        if (other.CompareTag("GrabbableWall"))
        {
            this.controlState = ControlState.PullingPlayer;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        this.controlState = ControlState.Retracting;
    }
}
