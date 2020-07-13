using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Vector3 startPostion;
    public Vector3 endPosition;
    public static bool highAlert;
    public float highAlertModifier = 1.5f;
    public float walkSpeed = 5f;

    bool towardsEnd;
    // Start is called before the first frame update
    void Start()
    {
        highAlert = false;
        towardsEnd = true;
        if (startPostion == null)
        {
            startPostion = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var speed = highAlert ? walkSpeed * highAlertModifier : walkSpeed;
        var destination = towardsEnd ? endPosition : startPostion;
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        transform.LookAt(destination);
        if (Vector3.Distance(transform.position, destination) < 0.1f)
        {
            towardsEnd = !towardsEnd;
        }
    }
}
