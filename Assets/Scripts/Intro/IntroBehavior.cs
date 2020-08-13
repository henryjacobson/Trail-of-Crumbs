using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroBehavior : MonoBehaviour
{
    static public bool intro;

    public GameObject[] cameras;
    public float[] durations;
    public AudioClip finalClip;

    GameObject canvas;
    int idx;
    float timer;

    Camera main;
    DisableControls disableControls;

    // Start is called before the first frame update
    void Start()
    {
        intro = true;
        main = Camera.main;
        main.depth = -100;
        cameras[0].SetActive(true);
        timer = durations[0];
        idx = 0;
        disableControls = GameObject.FindGameObjectWithTag("Player").GetComponent<DisableControls>();
        disableControls.Disable();
        canvas = GameObject.FindGameObjectWithTag("UI");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            intro = false;
        }
        //canvas.SetActive(false);
        if (timer <= 0 || !intro)
        {
            cameras[idx].SetActive(false);
            if (idx < cameras.Length - 1 && intro)
            {
                idx++;
                cameras[idx].SetActive(true);
                timer = durations[idx];
            }
            else
            {
                main.depth = 0;
                intro = false;
                disableControls.Enable();
                AudioSource.PlayClipAtPoint(finalClip, GameObject.FindGameObjectWithTag("Player").transform.position);
                canvas.SetActive(true);
                gameObject.SetActive(false);
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
