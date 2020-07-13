using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHandController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private KeyCode launchKey;
    [SerializeField]
    private float speed;

    private Rigidbody rb;
    private Rigidbody playerRb;
    
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
    }

    void Update()
    {
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
        if (Input.GetKeyDown(this.launchKey))
        {
            this.controlState = ControlState.Launching;
        }
    }

    private void LaunchingUpdate()
    {
        
    }

    private void RetractingUpdate()
    {

    }

    private void PullingPlayerUpdate()
    {

    }
}
