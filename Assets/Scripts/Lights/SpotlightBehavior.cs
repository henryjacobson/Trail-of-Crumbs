using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightBehavior : MonoBehaviour
{
    //can do by rotation amount or by position to look at
    //for now, using empty objects as markers
    public Transform markerOne;
    public Transform markerTwo;

    // in case light hits player
    public GameObject player;
    // attached light object
    public Light spotlight;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(20f * Time.deltaTime, 0, 0);
        //transform.LookAt(markerTwo.position);
        //Vector3.Lerp(markerOne.position, markerTwo.position, Time.deltaTime));

        Vector3 dir = markerTwo.position - transform.position;
        dir.y = 0; // keep the direction strictly horizontal
        Quaternion rot = Quaternion.LookRotation(dir);
        // slerp to the desired rotation over time
        transform.rotation = Quaternion.Slerp(markerOne.rotation, markerTwo.rotation, 2.0f * Time.deltaTime);
    }
}

