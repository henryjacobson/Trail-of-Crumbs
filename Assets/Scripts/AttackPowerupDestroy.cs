using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPowerupDestroy : MonoBehaviour
{
    public GameObject sparks;
    public GameObject smoke;
    public AudioClip sound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        gameObject.SetActive(false);
        AudioSource.PlayClipAtPoint(sound, transform.position);
        Instantiate(sparks, transform.position + Vector3.up, Quaternion.Euler(90 * Vector3.left));
        Instantiate(smoke, transform.position + Vector3.up, Quaternion.Euler(90 * Vector3.left));
        this.GetComponent<CacheOnCheckpoint>().OnCache(0.5f);
    }
}
