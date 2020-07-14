using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float speed = 2f;
    public float jumpAmount = 200;
    
    Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        //rb.AddForce(transform.forward * 5);
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        
        //debug
        //debug
        
        Vector3 forceVector = new Vector3(moveHorizontal, 0.0f, moveVertical);
        
        rb.AddForce(forceVector * speed);
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(0, jumpAmount, 0);
        }
        
        
    }
}
