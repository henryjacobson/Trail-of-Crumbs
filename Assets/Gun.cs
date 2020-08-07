using UnityEngine;

//used tutorial https://www.youtube.com/watch?v=THnivyG0Mvo
public class Gun : MonoBehaviour {
    
    public float damage = 10f;
    public float range = 100f;
    
    public Camera fpsCam;
    public ParticleSystem laserFlash;

    // Update is called once per frame
    void Update() {
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Shoot();
        }
        
    }
    
    void Shoot ()
    {
        GrappleHandController gr = GetComponent<GrappleHandController>();
        
        //this prints Resting
        Debug.Log(gr.controlState);
        
        if(gr.controlState == ControlState.Launching){
            
            Debug.Log("launching");
            
            laserFlash.Play();

            RaycastHit hit;
            
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                Debug.Log(hit.transform.name);
            }
        }
    }
}
