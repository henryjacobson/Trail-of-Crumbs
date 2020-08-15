using DigitalRuby.PyroParticles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumbsBanksAI : MonoBehaviour
{
    public float height;
    public GameObject shield;

    public GameObject fireball;
    public Transform hand;
    public Vector3 handOffset;

    public float attackTimeMin = 2.5f;
    public float attackTimeMax = 5f;

    public Transform center;
    public float xMin;
    public float xMax;
    public float zMin;
    public float zMax;
    public float speed;

    public GameObject particle0;
    public GameObject particle1;

    public AudioClip song;
    public AudioClip riser;
    public float riserDelay;

    public AudioClip attackSFX;
    public AudioClip ouchSFX;
    public AudioClip shieldSFX;
    public AudioClip deathSFX;

    public GameObject camera;

    public AudioClip initMusic;

    Transform player;
    Animator anim;
    bool doneStanding;
    bool attacked;
    bool shielded;
    float attackTimer;
    float attackTime;
    GameObject shieldObject;
    Vector3 prevSpot;
    Vector3 hoverSpot;

    bool spotted;
    bool started;

    Rigidbody[] ragdoll;

    AudioSource src;

    int nShieldGens;

    int damageTaken;

    FSMStates state;

    Vector3 startPos;
    Quaternion startRot;

    enum FSMStates
    {
        Sitting,
        Rising,
        Neutral,
        Attacking,
        Damaged,
        Shielding
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();

        state = FSMStates.Sitting;
        doneStanding = false;
        spotted = false;

        damageTaken = 0;

        particle0.SetActive(false);
        particle1.SetActive(false);

        src = Camera.main.GetComponent<AudioSource>();

        ragdoll = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in ragdoll)
        {
            rb.isKinematic = true;
        }

        startPos = transform.position;
        startRot = transform.rotation;

        LevelManager.onLevelReset += this.CheckPointReset;
    }

    void OnDestroy()
    {
        LevelManager.onLevelReset -= this.CheckPointReset;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case FSMStates.Rising:
                RisingUpdate();
                break;
            case FSMStates.Neutral:
                NeutralUpdate();
                break;
            case FSMStates.Attacking:
                AttackingUpdate();
                break;
            case FSMStates.Damaged:
                DamagedUpdate();
                break;
            case FSMStates.Shielding:
                ShieldingUpdate();
                break;
        }
    }

    void RisingUpdate()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Flying"))
        {
            particle0.SetActive(true);
            particle1.SetActive(true);
            if (!doneStanding)
            {
                doneStanding = true;
                transform.position += transform.forward * .45f;
            }
            transform.position += Vector3.up * Time.deltaTime;
            if (transform.position.y >= height)
            {
                transform.position = new Vector3(transform.position.x, height, transform.position.z);
                state = FSMStates.Neutral;
                anim.SetTrigger("start");

                prevSpot = transform.position;
                hoverSpot = transform.position;

                attackTime = 0;
                attackTimer = 0;

                src.clip = song;
                src.Play();
            }
        }
    }

    void NeutralUpdate()
    {
        if (Vector3.Distance(hoverSpot, transform.position) > 1f) 
        {
            transform.position = Vector3.Lerp(prevSpot, hoverSpot, (attackTime - attackTimer) / attackTime);
        }

        FacePlayer();
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            state = FSMStates.Attacking;
            anim.SetTrigger("attack");
            attacked = false;
        }
    }

    void AttackingUpdate()
    {
        FacePlayer();
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        if (!attacked && info.IsName("Attack") && info.normalizedTime >= 0.4f)
        {
            attacked = true;
            Vector3 pos = hand.position + handOffset;
            GameObject fireObj = Instantiate(fireball);
            fireObj.transform.position = pos;
            fireObj.transform.LookAt(player);
            fireObj.transform.position += fireObj.transform.forward * 2;
            attackTimer = attackTime = Random.Range(attackTimeMin, attackTimeMax);
            AudioSource.PlayClipAtPoint(attackSFX, transform.position);
        }
        if (attacked && info.IsName("Idle"))
        {
            state = FSMStates.Neutral;
            NewSpot();
        }
    }

    void DamagedUpdate()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Shield"))
        {
            state = FSMStates.Shielding;
            shielded = false;
        }
    }

    void ShieldingUpdate()
    {
        FacePlayer();
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        if (!shielded && info.normalizedTime >= 0.55f)
        {
            shielded = true;
            shieldObject = Instantiate(shield);
            shieldObject.transform.SetParent(transform);
            shieldObject.transform.localPosition = new Vector3(.14f, .87f, 0);

            ShieldGenerator[] shieldGens = FindObjectsOfType<ShieldGenerator>();
            nShieldGens = 0;
            foreach (ShieldGenerator shieldGen in shieldGens)
            {
                if (shieldGen.Enable(damageTaken == 2))
                    nShieldGens++;
            }

            attackTimer = Random.Range(attackTimeMin, attackTimeMax);

            AudioSource.PlayClipAtPoint(shieldSFX, transform.position);
        }
        if (shielded && info.IsName("Idle"))
        {
            state = FSMStates.Neutral;
            NewSpot();
        }
    }

    void FacePlayer()
    {
        Vector3 spot = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(spot);
    }

    public void DamageShield()
    {
        nShieldGens--;
        if (nShieldGens == 0)
        {
            Destroy(shieldObject);
            shielded = false;
        }
    }

    public void TakeDamage()
    {
        if (!shielded && (state == FSMStates.Neutral || state == FSMStates.Attacking))
        {
            if (damageTaken < 2)
            {
                anim.SetTrigger("damage");
                state = FSMStates.Damaged;
                damageTaken++; 
                AudioSource.PlayClipAtPoint(ouchSFX, transform.position);
            }
            else
            {
                /*
                var pos = transform.position + Vector3.up * 2f;
                var ratio = 1 / Vector3.Distance(pos, center.position);
                camera.transform.position = Vector3.Lerp(pos, center.position, ratio);
                camera.transform.LookAt(pos);
                camera.SetActive(true);
                */
                AudioSource.PlayClipAtPoint(deathSFX, transform.position);
                FindObjectOfType<LevelManager>().LevelWon();
                GetComponent<Animator>().enabled = false;
                particle0.SetActive(false);
                particle1.SetActive(false);
                enabled = false;
                foreach (Rigidbody rb in ragdoll)
                {
                    rb.isKinematic = false;
                }
            }
        }
    }

    public void DelaySpotPlayer(float delay)
    {
        Invoke("SpotPlayerExtra", delay);
        started = true;
    }

    public void SpotPlayerExtra()
    {
        if (!spotted && started)
        {
            spotted = true;
            state = FSMStates.Rising;
            anim.SetTrigger("spotted");
            print("now");

            Invoke("Riser", riserDelay);
        }

    }

    public void SpotPlayer()
    {
        if (!spotted)
        {
            spotted = true;
            state = FSMStates.Rising;
            anim.SetTrigger("spotted");
            print("now");

            Invoke("Riser", riserDelay);
        }

    }

    void Riser()
    {
        src.clip = riser;
        src.Play();
    }

    void NewSpot()
    {
        prevSpot = transform.position;
        do
        {
            float x = center.position.x + Random.Range(xMin, xMax);
            float z = center.position.z + Random.Range(zMin, zMax);
            hoverSpot = new Vector3(x, height, z);
        } while (Vector3.Distance(hoverSpot, player.position) < 5f && Vector3.Distance(hoverSpot, prevSpot) < 10f);
    }

    public void CheckPointReset()
    {
        particle0.SetActive(false);
        particle1.SetActive(false);
        state = FSMStates.Sitting;
        doneStanding = false;
        spotted = false;
        started = false;
        damageTaken = 0;
        transform.position = startPos;
        transform.rotation = startRot;
        anim.SetTrigger("reset");
        src.clip = initMusic;
        src.Play();
        Destroy(shieldObject);
        shielded = false;
        ShieldGenerator[] shieldGens = FindObjectsOfType<ShieldGenerator>();
        foreach (ShieldGenerator shieldGen in shieldGens)
        {
            shieldGen.Disable();
        }

        GameObject.FindGameObjectWithTag("Monologue").GetComponent<BoxCollider>().enabled = true;
    }
}
