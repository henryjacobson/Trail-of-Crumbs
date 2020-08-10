using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertExample : MonoBehaviour
{
    public EnemyAI enemy;
    public float delay = 5f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Alert", delay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Alert()
    {
        enemy.Alert(gameObject.transform, true);
    }
}
