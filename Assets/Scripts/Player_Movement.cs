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

    private Transform groundChecker;

    private CharacterController _charCont;

    private bool controlsActive;

    private float footstepCounter;

    private bool flippingGravity;

    // Start is called before the first frame update
    void Start(){
        groundChecker = transform.Find("GroundChecker");
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

            if (this.IsGrounded())
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
        
        //crouching
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (this.IsGrounded())
            {
                    _charCont.height = 0.0f;
                    speed = 3.0f;
            }
        }
        else 
        {
            _charCont.height = 2.0f;
            speed = 6.0f;
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

    private void FlipGravity()
    {
        StartCoroutine(this.StartGravityFlip());
    }

    private IEnumerator StartGravityFlip()
    {
        while(this.flippingGravity)
        {
            yield return null;
        }
        this.flippingGravity = true;
        StartCoroutine("SmoothRotate", this.transform.rotation * Quaternion.AngleAxis(180, Vector3.forward));
    }

    private IEnumerator SmoothRotate(Quaternion rotation)
    {
        while(Quaternion.Angle(this.transform.rotation, rotation) >= 1)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, Time.deltaTime * 12);
            yield return null;
        }

        this.transform.rotation = rotation;
        this.flippingGravity = false;
    }

    private bool IsGrounded()
    {
        Collider[] colliders = Physics.OverlapSphere(this.groundChecker.position, this._charCont.radius, ~LayerMask.GetMask("Ignore Raycast", "Player"));
        foreach(Collider collider in colliders)
        {
            if (!collider.isTrigger)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawSphere(this.groundChecker.position, this._charCont.radius);
    }
}
