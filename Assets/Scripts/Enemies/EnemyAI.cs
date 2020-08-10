using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public List<Transform> path;
    public float walkSpeed = 5f;
    public float alertDuration = 5f;
    public float alertedSlowDown = 2f;
    public AudioClip spotted;
    public AudioClip returning;
    public AudioClip gameOver;
    AudioSource audioSource;

    NavMeshAgent agent;
    List<Vector3> vecPath;
    int towards; // whihc target it's currently walking towards
    bool towardsEnd;

    Vector3 alertTarget; // target when alerted
    float alertTime;
    bool playReturn;

    FSMStates state;

    Transform player;

    enum FSMStates
    {
        Patrol,
        Alerted,
        GameOver
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        towardsEnd = true;
        if (path[0] == null)
        {
            path[0] = transform;
        }
        vecPath = new List<Vector3>();
        foreach (Transform t in path) 
        {
            vecPath.Add(t.position);
        }
        towards = 1;
        GetComponentInChildren<SpotlightDetectPlayer>().SetParent(this);
        audioSource = GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case FSMStates.Patrol:
                PatrolUpdate();
                break;
            case FSMStates.Alerted:
                AlertedUpdate();
                break;
            case FSMStates.GameOver:
                GameOverUpdate();
                break;
        }
    }

    void PatrolUpdate()
    {
        agent.speed = walkSpeed;
        var destination = vecPath[towards];
        destination.y = transform.position.y;
        agent.SetDestination(destination);

        if (Vector3.Distance(transform.position, destination) < 1f)
        {
            if (towards == 0 || towards == path.Count - 1)
            {
                towardsEnd = !towardsEnd;
            }
            towards += towardsEnd ? 1 : -1;
        }
    }

    void AlertedUpdate()
    {
        agent.speed = walkSpeed / alertedSlowDown;
        var destination = alertTarget;
        destination.y = transform.position.y;
        agent.SetDestination(destination);

        alertTime -= Time.deltaTime;
        if (alertTime <= 0)
        {
            state = FSMStates.Patrol;
            if (playReturn)
            {
                audioSource.clip = returning;
                audioSource.Play();
            }
        }
    }

    void GameOverUpdate()
    {
        agent.isStopped = true;
        var destination = alertTarget;
        destination.y = transform.position.y;
        transform.LookAt(destination);
    }

    public void Alert(Transform target, bool initialEnemy)
    {
        if (state != FSMStates.Alerted && state != FSMStates.GameOver)
        {
            if (initialEnemy)
            {
                audioSource.clip = spotted;
                audioSource.Play();
                playReturn = true;
                var enemies = FindObjectsOfType<EnemyAI>();
                foreach (EnemyAI enemy in enemies)
                {
                    if (enemy != this)
                    {
                        enemy.Alert(target, false);
                    }
                }
            }
            else
            {
                playReturn = false;
            }
            alertTarget = target.transform.position;
            state = FSMStates.Alerted;
            alertTime = alertDuration;
        }

    }

    public void GameOver()
    {
        state = FSMStates.GameOver;
        audioSource.clip = gameOver;
        audioSource.Play();
    }
}
