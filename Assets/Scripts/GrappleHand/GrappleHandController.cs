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
    private AudioClip hitSFX;

    [SerializeField]
    private PowerUpSlider powerUpTimerSlider;
    [SerializeField]
    private Color extendRangeUIColor = Color.green;
    [SerializeField]
    private Color attackUIColor = Color.red;

    private Rigidbody rb;
    private CharacterController playerCC;
    private Collider collider;

    private Transform returnPoint;
    private Vector3 initialLocalScale;

    private string grabbableWallTag = "GrabbableWall";

    private string grabbableItemTag = "GrabbableItem";
    private List<Transform> items;

    private Dictionary<PowerUp, float> powerUpTimers;
    private Dictionary<PowerUp, Color> powerUpUIColors;
    
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
        returnPoint.transform.SetParent(this.camera.transform);
        this.returnPoint = returnPoint.transform;

        this.initialLocalScale = this.transform.localScale;

        this.playerCC = player.GetComponent<CharacterController>();
        this.rb = this.GetComponent<Rigidbody>();
        this.collider = this.GetComponent<Collider>();

        this.player.layer = LayerMask.NameToLayer("Player");
        this.playerGrappleBehaviour = this.player.AddComponent<PlayerWithGrappleBehaviour>();
        this.playerGrappleBehaviour.SetGrapple(this.gameObject);

        this.ResetToResting();
        this.previousControlState = this.controlState;

        this.items = new List<Transform>();

        this.powerUpTimers = this.initPowerUpFlags();
        this.powerUpUIColors = this.initPowerUpColors();
        

        if (this.powerUpTimerSlider == null)
        {
            this.powerUpTimerSlider = GameObject.Find("PowerUpTimerSlider").GetComponent<PowerUpSlider>();
        }

        if (this.laserFlash == null)
        {
            this.laserFlash = this.returnPoint.Find("LaserFlash").GetComponent<ParticleSystem>();
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

    private Dictionary<PowerUp, Color> initPowerUpColors()
    {
        Dictionary<PowerUp, Color> result = new Dictionary<PowerUp, Color>();

        result.Add(PowerUp.ExtendedRange, this.extendRangeUIColor);
        result.Add(PowerUp.Attack, this.attackUIColor);

        return result;
    }

    public void ResetToResting()
    {
        this.controlState = ControlState.Resting;
        this.transform.SetParent(this.camera.transform);
        if (this.laserFlash != null)
        {
            this.laserFlash.Stop();
            this.laserFlash.Clear();
        }
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

            if (this.controlState != ControlState.Resting)
            {
                this.returnPoint.LookAt(this.transform);
            }

            this.collider.enabled = this.controlState != ControlState.Resting;
        }
        
        if (Input.GetMouseButtonDown(0))
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
        if (Input.GetMouseButtonDown(0))
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

        this.transform.localRotation = Quaternion.identity;

        this.transform.localScale = this.initialLocalScale;
    }

    private void LaunchingUpdate()
    {

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

            this.ResetToResting();
        }
    }

    private void PullingPlayerUpdate()
    {
        Vector3 toHook = this.transform.position - this.player.transform.position;
        Vector3 offset = toHook.normalized * this.GetGrappleSpeed() * Time.deltaTime;
        this.playerCC.Move(offset);
        if (Input.GetMouseButtonDown(0))
        {
            AudioSource.PlayClipAtPoint(this.returnSFX, this.transform.position);

            if (toHook.magnitude <= this.distanceToGrappleToStop)
            {
                this.ResetToResting();
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
        Vector3 pointA = this.transform.position + this.transform.forward * 0.25f;
        Vector3 pointB = this.transform.position - this.transform.forward * 0.25f;
        LayerMask mask = ~LayerMask.GetMask("Light", "Ignore Raycast");
        Collider[] colliders = Physics.OverlapCapsule(pointA, pointB, 0.25f, mask);

        bool grabTriggerFound = false;
        bool obstructionFound = false;
        foreach (Collider c in colliders)
        {
            if (this.IsObjectGrabbable(c.gameObject) && this.ObjectNotThisOrChild(c.gameObject))
            {
                grabTriggerFound = true;
            }
            else if (this.ObjectNotThisOrChild(c.gameObject))
            {
                obstructionFound = true;
            }
        }

        if (!grabTriggerFound && obstructionFound)
        {
            AudioSource.PlayClipAtPoint(this.hitSFX, this.transform.position);
            this.controlState = ControlState.Retracting;
        }

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

            if (other.CompareTag("CrumbsBanks"))
            {
                other.gameObject.GetComponent<CrumbsBanksAI>().TakeDamage();
                this.controlState = ControlState.Retracting;
            }

            if (other.CompareTag("ShieldGenerator"))
            {
                other.gameObject.GetComponentInChildren<ShieldGenerator>().Disable();
                this.controlState = ControlState.Retracting;
            }

            if (other.CompareTag(this.grabbableWallTag))
            {
                this.controlState = ControlState.PullingPlayer;
                AudioSource.PlayClipAtPoint(this.hitSFX, this.transform.position);
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
        this.powerUpTimerSlider.SetMax(time);
        if (this.powerUpUIColors.TryGetValue(powerUp, out Color c))
        {
            this.powerUpTimerSlider.SetColor(c);
        }
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
                    this.UpdateTimerSlider(t);
                } else
                {
                    t = 0;
                }
                this.UpdatePowerUp(p, t);
            }
        }
        
        /*
        if (!this.AnyPowerupActive())
        {
            this.UpdateTimerText(0);
        }
        */
    }

    private void UpdateTimerSlider(float amount)
    {
        this.powerUpTimerSlider.SetValue(amount);
    }

    /*
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
    */

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
