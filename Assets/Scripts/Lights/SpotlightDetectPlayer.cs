using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class SpotlightDetectPlayer : MonoBehaviour
{
    private Light light;

    private Transform player;
    private Transform grappleHand;
    private EnemyMovement parentEnemy;

    void Start()
    {
        this.light = this.GetComponent<Light>();
        this.light.type = LightType.Spot;

        this.player = GameObject.FindGameObjectWithTag("Player").transform;
        this.grappleHand = GameObject.FindGameObjectWithTag("GrappleHand").transform;
    }

    void Update()
    {
        if (!LevelManager.isGameOver && this.DetectPlayer())
        {
            FindObjectOfType<LevelManager>().LevelLost();
        }
        if (this.DetectGrappleHand())
        {
            if (parentEnemy != null)
            {
                parentEnemy.Alert(player, true);
            }
        }
    }

    private bool DetectPlayer()
    {
        Vector3 toPlayer = (this.player.position - this.transform.position).normalized;

        float angleToPlayer = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(toPlayer, this.transform.forward));

        if (angleToPlayer > this.light.spotAngle / 2)
        {
            return false;
        } else
        {
            RaycastHit hit = this.castToPlayer(toPlayer);
            if (hit.collider == null)
            {
                return false;
            } else if (hit.collider.CompareTag("Player"))
            {
                return true;
            } else
            {
                return false;
            }
        }
    }

    private bool DetectGrappleHand()
    {
        Vector3 toObject = (this.grappleHand.position - this.transform.position).normalized;

        float angleToObject = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(toObject, this.transform.forward));

        if (angleToObject > this.light.spotAngle / 2)
        {
            return false;
        }
        else
        {
            RaycastHit hit = this.castToPlayer(toObject);
            if (hit.collider == null)
            {
                return false;
            }
            else if (hit.collider.CompareTag("GrappleHand"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private RaycastHit castToPlayer(Vector3 toPlayer)
    {
        //TRIGGERS SHOULD BE IN THE IgnoreRaycast LAYER
        RaycastHit hit;
        Physics.Raycast(this.transform.position, toPlayer, out hit, this.light.range, ~LayerMask.GetMask("IgnoreRaycast"));
        return hit;
    }

    public void SetParent(EnemyMovement parent)
    {
        parentEnemy = parent;
    }
}
