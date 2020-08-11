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


    Transform player;
    CharacterController controller;
    Animator anim;
    bool doneStanding;
    bool attacked;
    bool shielded;
    float attackTimer;
    GameObject shieldObject;

    FSMStates state;

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
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        state = FSMStates.Sitting;
        doneStanding = false;

        Invoke("SpotPlayer", 2);
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
            if (!doneStanding)
            {
                doneStanding = true;
                controller.Move(transform.forward * .45f);
            }
            controller.Move(Vector3.up * Time.deltaTime);
            if (transform.position.y >= height)
            {
                transform.position = new Vector3(transform.position.x, height, transform.position.z);
                state = FSMStates.Neutral;
                anim.SetTrigger("start");
            }
        }
    }

    void NeutralUpdate()
    {
        transform.LookAt(player);
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
        transform.LookAt(player);
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        if (!attacked && info.IsName("Attack") && info.normalizedTime >= 0.5f)
        {
            attacked = true;
            Vector3 pos = hand.position + handOffset;
            GameObject fireObj = Instantiate(fireball);
            fireObj.transform.position = pos;
            fireObj.transform.LookAt(player);
            attackTimer = Random.Range(attackTimeMin, attackTimeMax);
        }
        if (attacked && info.IsName("Idle"))
        {
            state = FSMStates.Neutral;
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
        transform.LookAt(player);
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        if (!shielded && info.normalizedTime >= 0.55f)
        {
            shielded = true;
            shieldObject = Instantiate(shield);
            shieldObject.transform.SetParent(transform);
            attackTimer = Random.Range(attackTimeMin, attackTimeMax);
        }
        if (shielded && info.IsName("Idle"))
        {
            state = FSMStates.Neutral;
        }
    }

    public void DestroyShield()
    {
        Destroy(shieldObject);
        shielded = false;
    }

    public void TakeDamage()
    {
        anim.SetTrigger("damage");
        state = FSMStates.Damaged;
    }

    public void SpotPlayer()
    {
        state = FSMStates.Rising;
        anim.SetTrigger("spotted");
        print("now");
    }
}
