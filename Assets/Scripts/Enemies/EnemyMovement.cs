using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public List<Transform> path;
    public bool highAlert;
    public float highAlertModifier = 1.5f;
    public float walkSpeed = 5f;
    public float alertDuration = 5f;
    public float alertedSlowDown = 2f;

    float height; // stays the same height
    List<Vector3> vecPath;
    int towards; // whihc target it's currently walking towards
    bool towardsEnd;
    bool alerted;
    Transform alertTarget; // target when alerted
    float alertTime;

    // Start is called before the first frame update
    void Start()
    {
        height = transform.position.y;
        highAlert = false;
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
    }

    // Update is called once per frame
    void Update()
    {
        var speed = highAlert ? walkSpeed * highAlertModifier : walkSpeed;
        speed = alerted ? speed / alertedSlowDown : speed;
        var destination = alerted ? alertTarget.position : vecPath[towards];
        destination.y = height;
        FaceTarget(destination);
        transform.position += transform.forward * speed * Time.deltaTime;
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
                alerted = false;            }
        }
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 lookRotation = target - transform.position;
        lookRotation.y = 0;
        Quaternion lookQuat = Quaternion.LookRotation(lookRotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookQuat, Time.deltaTime * 5);
    }

    public void Alert(Transform target)
    {
        alertTarget = target.transform;
        alerted = true;
        alertTime = alertDuration;
    }
}
