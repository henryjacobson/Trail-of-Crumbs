using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public List<Transform> path;
    public float walkSpeed = 5f;
    public float alertDuration = 5f;
    public float alertedSlowDown = 2f;
    public AudioClip spotted;
    public AudioClip returning;

    NavMeshAgent agent;
    float height; // stays the same height
    List<Vector3> vecPath;
    int towards; // whihc target it's currently walking towards
    bool towardsEnd;
    bool alerted;
    Transform alertTarget; // target when alerted
    float alertTime;
    bool playReturn;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        height = transform.position.y;
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
    }

    // Update is called once per frame
    void Update()
    {
        agent.speed = alerted ? walkSpeed / alertedSlowDown : walkSpeed;
        var destination = alerted ? alertTarget.position : vecPath[towards];
        destination.y = height;
        FaceTarget(destination);
        agent.SetDestination(destination);
        //transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, destination) < 1f)
        {
            if (towards == 0 || towards == path.Count - 1)
            {
                towardsEnd = !towardsEnd;
            }
            towards += towardsEnd ? 1 : -1;
        }
        if (alerted)
        {
            alertTime -= Time.deltaTime;
            if (alertTime <= 0)
            {
                alerted = false;
                if (playReturn)
                {
                    AudioSource.PlayClipAtPoint(returning, transform.position);
                }
            }
        }
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 lookRotation = target - transform.position;
        lookRotation.y = 0;
        Quaternion lookQuat = Quaternion.LookRotation(lookRotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookQuat, Time.deltaTime * 5);
    }

    public void Alert(Transform target, bool initialEnemy)
    {
        if (initialEnemy)
        {
            AudioSource.PlayClipAtPoint(spotted, transform.position);
            playReturn = true;
            var enemies = FindObjectsOfType<EnemyMovement>();
            foreach (EnemyMovement enemy in enemies)
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
        alertTarget = target.transform;
        alerted = true;
        alertTime = alertDuration;
    }
}
