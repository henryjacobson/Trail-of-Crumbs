using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MonologueBehavior : MonoBehaviour
{
    static public bool intro;

    public GameObject[] cameras;
    public float[] durations;
    public AudioClip clip;

    public float spotDelay;

    AudioSource src;

    GameObject canvas;
    int idx;
    float timer;
    bool began;

    Camera main;
    DisableControls disableControls;

    // Update is called once per frame
    void Update()
    {
        if (began)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                intro = false;
                FindObjectOfType<CrumbsBanksAI>().SpotPlayer();
                FindObjectOfType<CrumbsBanksAI>().GetComponent<AudioSource>().clip = null;
            }
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
                    began = false;
                    main.depth = 0;
                    intro = false;
                    disableControls.Enable();
                    canvas.SetActive(true);
                    Cursor.lockState = CursorLockMode.Locked;
                    GetComponent<BoxCollider>().enabled = false;
                }
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            began = true;
            intro = true;
            main = Camera.main;
            main.depth = -100;
            idx = -1;
            disableControls = GameObject.FindGameObjectWithTag("Player").GetComponent<DisableControls>();
            disableControls.Disable();
            Cursor.lockState = CursorLockMode.Locked;
            canvas = GameObject.FindGameObjectWithTag("UI");
            FindObjectOfType<CrumbsBanksAI>().DelaySpotPlayer(spotDelay);
            src = GameObject.FindGameObjectWithTag("CrumbsBanks").GetComponent<AudioSource>();
            src.clip = clip;
            src.Play();
        }
    }
}
