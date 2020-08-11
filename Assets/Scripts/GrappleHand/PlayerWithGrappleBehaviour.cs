using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWithGrappleBehaviour : MonoBehaviour
{
    private Rigidbody rb;

    bool active;
    bool pulling;

    private GameObject grapple;
    private GrappleHandController grappleController;

    // Start is called before the first frame update
    void Start()
    {
        this.rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetGrapple(GameObject grapple)
    {
        this.grapple = grapple;
        this.grappleController = grapple.GetComponent<GrappleHandController>();
    }

    private void GrappleStateChanged(ControlState state)
    {
        if (state == ControlState.PullingPlayer)
        {
            this.pulling = true;
            this.active = false;
        }
        else
        {
            this.active = true;
        }

        if (state == ControlState.Resting)
        {
            this.pulling = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (this.pulling)
        {
            this.rb.velocity = Vector3.zero;
            this.rb.angularVelocity = Vector3.zero;
            this.grappleController.ResetToResting();
        }
    }
}
