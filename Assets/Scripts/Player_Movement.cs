using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//CITATION: used tutorial https://www.youtube.com/watch?v=Ov9ekwAGhMA
public class Player_Movement : MonoBehaviour
{
    public float speed = 6.0f;
    public float gravity = -9.0f;

    private float yVelocity;

    public FootstepSFX footstepSFX;
    public float footstepDelay = 0.5f;

    private CharacterController _charCont;

    private bool controlsActive;

    private float footstepCounter;

    // Start is called before the first frame update
    void Start(){
        _charCont = GetComponent<CharacterController>();
        controlsActive = true;
    }

    // Update is called once per frame
    void Update(){
        if (controlsActive && !LevelManager.isGameOver)
        {
            float deltaX = Input.GetAxis("Horizontal") * speed;
            float deltaZ = Input.GetAxis("Vertical") * speed;
            Vector3 movement = new Vector3(deltaX, 0, deltaZ);
            movement = Vector3.ClampMagnitude(movement, speed);

            if (_charCont.isGrounded)
            {
                this.yVelocity = gravity;
                if (deltaX != 0 || deltaZ != 0)
                {
                    this.HandleFootstepSFX();
                }
            } else
            {
                this.yVelocity += gravity * Time.deltaTime;
                this.ResetFootstepSFX();
            }

            movement.y = this.yVelocity;

            movement *= Time.deltaTime;
            movement = transform.TransformDirection(movement);
            _charCont.Move(movement);
        }
    }

    private void GrappleStateChanged(ControlState controlState)
    {
        controlsActive = controlState != ControlState.PullingPlayer;
    }

    private void HandleFootstepSFX()
    {
        if (this.footstepCounter > 0)
        {
            this.footstepCounter -= Time.deltaTime;
        } else
        {
            this.footstepCounter = this.footstepDelay;
            this.footstepSFX.PlaySFX();
        }
    }

    private void ResetFootstepSFX()
    {
        this.footstepCounter = 0;
    }
}
