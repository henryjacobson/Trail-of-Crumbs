using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private float extendedGrappleMultiplier = 2;
    [SerializeField]
    private float distanceToGrappleToStop = 2;

    [SerializeField]
    private AudioClip fireSFX;
    [SerializeField]
    private AudioClip returnSFX;

    [SerializeField]
    private Text powerupTimerText;

    private Rigidbody rb;
    private CharacterController playerCC;

    private Transform returnPoint;

    private string grabbableWallTag = "GrabbableWall";

    private string grabbableItemTag = "GrabbableItem";
    private List<Transform> items;

    private Dictionary<PowerUp, float> powerUpTimers;
    
    //laser stuff
    
    public float damage = 10f;
    public float range = 100f;
    
    public Camera fpsCam;
    public ParticleSystem laserFlash;
    
    //laser stuff

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

        this.powerUpTimers = this.initPowerUpFlags();

        if (this.powerupTimerText == null)
        {
            this.powerupTimerText = GameObject.Find("PowerupTimerText").GetComponent<Text>();
        }
    }

    private Dictionary<PowerUp, float> initPowerUpFlags()
    {
        Dictionary<PowerUp, float> result = new Dictionary<PowerUp, float>();

        foreach(PowerUp p in GetPowerUpList())
        {
            result.Add(p, 0);
        }

        return result;
    }

    public void resetToResting()
    {
        this.controlState = ControlState.Resting;
        this.transform.SetParent(this.player.transform);
        this.EnforceRestingPosition();
    }

    void Update()
    {
        this.DepletePowerUpTimers();

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
        
        if (Input.GetKeyDown(this.launchKey))
        {
            Shoot();
        }
    }
    
    void Shoot() 
    {   
        if(this.controlState == ControlState.Launching){
            
            Debug.Log("launching");
            
            if(laserFlash != null && fpsCam != null)
            {
                laserFlash.Play();

                RaycastHit hit;

                if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
                {
                    Debug.Log(hit.transform.name);
                }
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
            AudioSource.PlayClipAtPoint(this.fireSFX, this.transform.position);

            this.controlState = ControlState.Launching;
            this.transform.parent = null;
            this.transform.position = this.camera.transform.position + this.camera.transform.forward;
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
            this.transform.localRotation = Quaternion.identity * Quaternion.AngleAxis(this.camera.transform.localEulerAngles.x, Vector3.right);
        }
    }

    private void LaunchingUpdate()
    {
        Vector3 pointA = this.transform.position + this.transform.forward * 0.25f;
        Vector3 pointB = this.transform.position - this.transform.forward * 0.25f;
        LayerMask mask = ~LayerMask.GetMask("Light", "Ignore Raycast");
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
        this.transform.position = Vector3.MoveTowards(this.transform.position, this.returnPoint.position, this.GetGrappleSpeed() * Time.deltaTime);
        if (this.transform.position == this.returnPoint.position)
        {
            AudioSource.PlayClipAtPoint(this.returnSFX, this.transform.position);

            this.resetToResting();
        }
    }

    private void PullingPlayerUpdate()
    {
        Vector3 toHook = this.transform.position - this.player.transform.position;
        Vector3 offset = toHook.normalized * this.GetGrappleSpeed() * Time.deltaTime;
        this.playerCC.Move(offset);
        if (Input.GetKeyDown(this.launchKey))
        {
            AudioSource.PlayClipAtPoint(this.returnSFX, this.transform.position);

            if (toHook.magnitude <= this.distanceToGrappleToStop)
            {
                this.resetToResting();
            } else
            {
                this.controlState = ControlState.Retracting;
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
        Vector3 offset = this.transform.position + (this.transform.forward * this.GetGrappleSpeed() * Time.fixedDeltaTime);
        this.rb.MovePosition(offset);

        float distanceFromPlayer = (this.transform.position - this.player.transform.position).magnitude;
        if (distanceFromPlayer >= this.GetGrappleRange())
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
            if (other.CompareTag("Enemy") && this.IsPowerUpActive(PowerUp.Attack))
            {
                other.gameObject.GetComponent<AttackPowerupDestroy>().Attack();
                this.controlState = ControlState.Retracting;
            }

            if (other.CompareTag("Conductor"))
            {
                other.gameObject.GetComponent<ConductorBehavior>().Attack();
                this.controlState = ControlState.Retracting;
            }

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
        g.transform.localPosition = Vector3.zero + (Vector3.forward * .5f);
    }

    public void SetPowerUp(PowerUp powerUp, float time)
    {
        this.UpdatePowerUp(powerUp, time);
    }

    private void UpdatePowerUp(PowerUp powerUp, float time)
    {
        if (this.powerUpTimers.TryGetValue(powerUp, out float x))
        {
            this.powerUpTimers.Remove(powerUp);
            this.powerUpTimers.Add(powerUp, time);
        }
    }

    private void DepletePowerUpTimers()
    {
        foreach(PowerUp p in GetPowerUpList())
        {
            if (this.powerUpTimers.TryGetValue(p, out float t))
            {
                if (t > 0)
                {
                    t -= Time.deltaTime;
                } else
                {
                    t = 0;
                }
                this.UpdatePowerUp(p, t);
                if (this.IsPowerUpActive(p))
                {
                    this.UpdateTimerText(t);
                }
            }
        }
        
        if (!this.AnyPowerupActive())
        {
            this.UpdateTimerText(0);
        }
    }

    private void UpdateTimerText(float amount)
    {
        if (amount > 0)
        {
            this.powerupTimerText.text = amount.ToString("f2");
        } else
        {
            this.powerupTimerText.text = "";
        }
    }

    private bool IsPowerUpActive(PowerUp powerUp)
    {
        return this.powerUpTimers.TryGetValue(powerUp, out float t) && t > 0;
    }

    public bool AnyPowerupActive()
    {
        foreach(PowerUp p in GetPowerUpList())
        {
            if (this.IsPowerUpActive(p))
            {
                return true;
            }
        }
        return false;
    }

    public static PowerUp[] GetPowerUpList()
    {
        return new PowerUp[2] { PowerUp.ExtendedRange, PowerUp.Attack };
    }

    public void DeactivatePowerups()
    {
        foreach(PowerUp p in GetPowerUpList())
        {
            this.powerUpTimers.Remove(p);
            this.powerUpTimers.Add(p, 0);
        }
    }

    private float GetGrappleRange()
    {
        if (this.IsPowerUpActive(PowerUp.ExtendedRange))
        {
            return this.maxDistance * this.extendedGrappleMultiplier;
        } else
        {
            return this.maxDistance;
        }
    }

    private float GetGrappleSpeed()
    {
        if (this.IsPowerUpActive(PowerUp.ExtendedRange))
        {
            return this.speed * this.extendedGrappleMultiplier;
        } else
        {
            return this.speed;
        }
    }
}

public enum ControlState
{
    Resting, Launching, Retracting, PullingPlayer
}

public enum PowerUp
{
    ExtendedRange, Attack
}
