using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGenerator : MonoBehaviour
{
    public Transform lens;
    public GameObject laser;

    Quaternion startRot;
    Transform target;
    CrumbsBanksAI ai;
    bool shielding;
    Transform currentLaser;

    // Start is called before the first frame update
    void Start()
    {
        startRot = transform.rotation;
        GameObject cbObj = GameObject.FindGameObjectWithTag("CrumbsBanks");
        target = cbObj.transform;
        ai = cbObj.GetComponent<CrumbsBanksAI>();
        shielding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (shielding)
        {
            transform.LookAt(target);
            currentLaser.transform.LookAt(target);
            currentLaser.Rotate(Vector3.right, 90);
            Vector3 scale = currentLaser.localScale;
            currentLaser.localScale = new Vector3(scale.x, Vector3.Distance(lens.position, target.position) / 2, scale.z);
            currentLaser.position = Vector3.Lerp(lens.position, target.position, .5f);
        }
    }

    public void Enable()
    {
        shielding = true;
        currentLaser = Instantiate(laser).transform;
        Update();
    }

    public void Disable()
    {
        shielding = false;
        laser.SetActive(false);
        transform.rotation = startRot;
        ai.DamageShield();
    }
}
