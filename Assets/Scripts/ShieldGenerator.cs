using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGenerator : MonoBehaviour
{
    public Transform lens;
    public GameObject laser;
    public AudioClip disableSFX;
    public bool secondRound = true;

    Quaternion startRot;
    Transform shield;
    CrumbsBanksAI ai;
    bool shielding;
    GameObject currentLaser;

    // Start is called before the first frame update
    void Start()
    {
        startRot = transform.rotation;
        ai = GameObject.FindGameObjectWithTag("CrumbsBanks").GetComponent<CrumbsBanksAI>();
        shielding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (shielding)
        {
            transform.LookAt(shield);
            Vector3 target = shield.position - transform.forward * 1.5f;
            currentLaser.transform.LookAt(target);
            currentLaser.transform.Rotate(Vector3.right, 90);
            Vector3 scale = currentLaser.transform.localScale;
            currentLaser.transform.localScale = new Vector3(scale.x, Vector3.Distance(lens.position, target) / 2, scale.z);
            currentLaser.transform.position = Vector3.Lerp(lens.position, target, .5f);
        }
    }

    public bool Enable(bool isSecondRound)
    {
        if (secondRound == isSecondRound)
        {
            currentLaser = Instantiate(laser);
            shield = GameObject.FindGameObjectWithTag("Shield").transform;
            shielding = true;
            Update();
            return true;
        }
        return false;
    }

    public void Disable()
    {
        shielding = false;
        Destroy(currentLaser);
        transform.rotation = startRot;
        ai.DamageShield();
        AudioSource.PlayClipAtPoint(disableSFX, transform.position);
    }
}
