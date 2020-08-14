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
        idx = -1;
        disableControls = GameObject.FindWithTag("Player").GetComponent<DisableControls>();
        canvas = GameObject.FindWithTag("UI");
        print(canvas);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            intro = false;
        }
        disableControls.Disable();
        canvas.SetActive(false);
        if (timer <= 0 || !intro)
        {
            if (idx >= 0)
            {
                cameras[idx].SetActive(false);
            }

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
