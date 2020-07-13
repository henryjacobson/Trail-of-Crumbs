using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GrappleHandPlayerBehaviour : MonoBehaviour
{
    private GrappleHandController grappleHandController;

    private Rigidbody rb;

    void Start()
    {
        this.rb = this.GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (this.grappleHandController.controlState == GrappleHandController.ControlState.PullingPlayer)
        {
            this.grappleHandController.resetToResting();
        }
    }
}
