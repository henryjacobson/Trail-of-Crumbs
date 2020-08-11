using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumbsBanksAI : MonoBehaviour
{
    public float height;

    Transform player;
    CharacterController controller;
    Animator anim;

    FSMStates state;

    enum FSMStates
    {
        Sitting,
        Rising,
        Neutral,
        Attacking,
        Damaged,
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        state = FSMStates.Sitting;

        Invoke("SptoPlayer", 2);

        print("pls");
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
        }
    }

    void RisingUpdate()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Flying"))
        {
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

    }

    void AttackingUpdate()
    {

    }

    void DamagedUpdate()
    {

    }

    public void PlayerSpotted()
    {
        state = FSMStates.Rising;
        anim.SetTrigger("spotted");
        print("now");
    }
}
