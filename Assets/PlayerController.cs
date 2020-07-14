using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
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
        float moveHorizontal = Input.GetAxis("Horizontal") * 5;
        float moveVertical = Input.GetAxis("Vertical") * 5;
        
        Vector3 forceVector = rb.velocity;
        
        forceVector.x = moveHorizontal;
        forceVector.y = moveVertical;
        rb.velocity = forceVector;
        
        //rb.AddForce(forceVector * speed);
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(0, jumpAmount, 0);
        }
        
        
    }
}
