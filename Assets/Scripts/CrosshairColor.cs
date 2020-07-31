using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//move this to GrappleHand prefab
public class CrosshairColor : MonoBehaviour
{

    private string grabbableWallTag = "GrabbableWall";
    private string grabbableItemTag = "GrabbableItem";
    
    public Image reticleImage;
    public Color reticleDementorColor;
    public Color reductoColor;
    Color originalReticleColor;
    
    // Start is called before the first frame update
    void Start()
    {
        originalReticleColor = reticleImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private bool IsObjectGrabbable(GameObject g)
    {
        return g.CompareTag(this.grabbableWallTag) || g.CompareTag(this.grabbableItemTag);
    }
    
    private void FixedUpdate()
    {
        ReticleEffect();
    }
    
    void ReticleEffect()
    {
        RaycastHit hit;
        
        if(Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            //color for aiming at walls
            if(hit.collider.CompareTag(this.grabbableWallTag))
            {
                
                reticleImage.color = Color.Lerp
                    (reticleImage.color, reticleDementorColor, Time.deltaTime * 2);
                    
                reticleImage.transform.localScale = Vector3.Lerp(
                    reticleImage.transform.localScale, new Vector3(0.7f, 0.7f, 1),
                    Time.deltaTime * 2);
            }
            //color for aiming at grabbable items
            else if (hit.collider.CompareTag(this.grabbableItemTag))
            {
                
                reticleImage.color = Color.Lerp
                    (reticleImage.color, reductoColor, Time.deltaTime * 2);
                    
                reticleImage.transform.localScale = Vector3.Lerp(
                    reticleImage.transform.localScale, new Vector3(0.7f, 0.7f, 1),
                    Time.deltaTime * 2);
            }
            else
            {
                reticleImage.color = Color.Lerp
                    (reticleImage.color, originalReticleColor, Time.deltaTime * 2);
                    
                reticleImage.transform.localScale = Vector3.Lerp(
                    reticleImage.transform.localScale, Vector3.one,
                    Time.deltaTime * 2);
            } 
        }
        else
        {
            reticleImage.color = Color.Lerp
                (reticleImage.color, originalReticleColor, Time.deltaTime * 2);
                    
            reticleImage.transform.localScale = Vector3.Lerp(
                reticleImage.transform.localScale, Vector3.one,
                Time.deltaTime * 2);
        }
    }
}
