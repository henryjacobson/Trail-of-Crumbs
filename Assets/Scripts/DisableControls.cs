using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableControls : MonoBehaviour
{
    Player_Movement playerMovement;
    Camera_Control[] cameraControls;
    GrappleHandController grappleHand;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponentInChildren<Player_Movement>();
        cameraControls = player.GetComponentsInChildren<Camera_Control>();
        grappleHand = player.GetComponentInChildren<GrappleHandController>();
    }

    public void Disable()
    {
        playerMovement.enabled = false;
        grappleHand.enabled = false;
        foreach (Camera_Control camera in cameraControls)
        {
            camera.enabled = false;
        }
    }

    public void Enable()
    {
        playerMovement.enabled = true;
        grappleHand.enabled = true;
        foreach (Camera_Control camera in cameraControls)
        {
            camera.enabled = true;
        }
    }
}
