using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightBehavior : MonoBehaviour
{
    // x rotation values
    public Vector3 orientation1;
    public Vector3 orientation2;

    // in case light hits player
    public Transform player;
    // attached light object
    //public Light spotlight;

    Quaternion posnOneRot;
    Quaternion posnTwoRot;
    Quaternion maxRotation;
   
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        float yAngle = this.transform.localEulerAngles.y;
        float zAngle = this.transform.localEulerAngles.z;

        posnOneRot = Quaternion.LookRotation(orientation1);
        Debug.Log(posnOneRot.eulerAngles);
        posnTwoRot = Quaternion.LookRotation(orientation2);
        maxRotation = posnTwoRot;
    }

    // Update is called once per frame
    void Update()
    {
        float delta = 1f;
        if (!LevelManager.isGameOver)
        {
            if (RotationEquals(transform.rotation, posnOneRot, delta))
            {
                //Quaternion.Set(x, y, z, w);
                maxRotation = posnTwoRot;
            }
            else if (RotationEquals(transform.rotation, posnTwoRot, delta))
            {
                maxRotation = posnOneRot;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation,
                maxRotation, Time.deltaTime);
        } else
        {
            transform.LookAt(this.player);
        }
    }

    private bool RotationEquals(Quaternion q1, Quaternion q2, float delta)
    {
        bool xEquals = Mathf.Abs(q1.x - q2.x) < delta;
        bool yEquals = Mathf.Abs(q1.y - q2.y) < delta;
        bool zEquals = Mathf.Abs(q1.z - q2.z) < delta;
        bool wEquals = Mathf.Abs(q1.w - q2.w) < delta;
        return xEquals && yEquals && zEquals && wEquals;
    }
}

