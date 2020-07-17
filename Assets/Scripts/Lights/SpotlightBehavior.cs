using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightBehavior : MonoBehaviour
{
    public Vector3 rotation1;
    public Vector3 rotation2;

    public float rotateSpeed = .2f;

    // in case light hits player
    public Transform player;
    // attached light object
    //public Light spotlight;

    private Vector3 orientation1;
    private Vector3 orientation2;
    private Vector3 currentOrientation;
    private Vector3 targetOrientation;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        transform.localEulerAngles = rotation1;
        orientation1 = transform.forward;
        transform.localEulerAngles = rotation2;
        orientation2 = transform.forward;

        currentOrientation = orientation1;
        targetOrientation = orientation2;
    }

    // Update is called once per frame
    void Update()
    {
        float delta = 0.01f;
        if (!LevelManager.isGameOver)
        {
            float t = Time.time;
            currentOrientation = Vector3.Lerp(orientation1, orientation2, (Mathf.Sin(t * rotateSpeed) + 1) / 2);

            transform.rotation = Quaternion.LookRotation(currentOrientation);
        } else
        {
            transform.LookAt(this.player);
        }
    }

    private bool VectorEquals(Vector3 v1, Vector3 v2, float delta)
    {
        bool xEquals = Mathf.Abs(v1.x - v2.x) < delta;
        bool yEquals = Mathf.Abs(v1.y - v2.y) < delta;
        bool zEquals = Mathf.Abs(v1.z - v2.z) < delta;
        return xEquals && yEquals && zEquals;
    }

    /*
     * public class SpotlightBehavior : MonoBehaviour
{
    // x rotation values
    public float xPosnOne = 90;
    public float xPosnTwo = 25;

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

        posnOneRot = Quaternion.Euler(new Vector3(xPosnOne, yAngle, zAngle));
        posnTwoRot = Quaternion.Euler(new Vector3(xPosnTwo, yAngle, zAngle));
        maxRotation = posnTwoRot;
    }

    // Update is called once per frame
    void Update()
    {
        if (!LevelManager.isGameOver)
        {
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
        } else
        {
            transform.LookAt(this.player);
        }
    }
}
    */
}

