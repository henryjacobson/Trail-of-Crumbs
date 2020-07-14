using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightBehavior : MonoBehaviour
{
    //can do by rotation amount or by position to look at
    //for now, using empty objects as markers
    public Transform markerOne;
    public Transform markerTwo;

    public float xPosnOne = 90;
    public float xPosnTwo = 25;

    // in case light hits player
    public Transform player;
    // attached light object
    public Light spotlight;

    Quaternion posnOneRot;
    Quaternion posnTwoRot;
    Quaternion maxRotation;
   
    // Start is called before the first frame update
    void Start()
    {
        posnOneRot = Quaternion.Euler(new Vector3(xPosnOne, 0, 0));
        posnTwoRot = Quaternion.Euler(new Vector3(xPosnTwo, 0, 0));
        maxRotation = posnTwoRot;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(20f * Time.deltaTime, 0, 0);
        //transform.LookAt(markerTwo.position);
        //Vector3.Lerp(markerOne.position, markerTwo.position, Time.deltaTime));

        /*
         * dir.y = 0; // keep the direction strictly horizontal
        Quaternion rotate = Quaternion.LookRotation(dir);
        // slerp to the desired rotation over time
        dir = markerTwo.position - transform.position;
        */

        if (transform.rotation == posnOneRot)
        {
            //Quaternion.Set(x, y, z, w);
            maxRotation = posnTwoRot;
        }
        else if (transform.rotation == posnTwoRot)
        {
            maxRotation = posnOneRot;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation,
            maxRotation, Time.deltaTime);

        Debug.Log(Vector3.Distance(spotlight.transform.position,
            player.position));
    }
}

