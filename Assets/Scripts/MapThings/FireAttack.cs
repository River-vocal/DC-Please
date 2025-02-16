using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAttack : MonoBehaviour
{
    [SerializeField] private float movementDistance;
    [SerializeField] private float speed;
    [SerializeField] public int damage;
    private bool movingLeft;
    private float leftEdge;
    private float rightEdge;

    private void Awake()
    {
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
    }

    private void Update()
    {
        if (movingLeft)
        {
            if (transform.position.x > leftEdge)
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y,
                    transform.position.z);
            }
            else
            {
                movingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightEdge)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y,
                    transform.position.z);
            }
            else
            {
                movingLeft = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            Energy energy = col.gameObject.GetComponent<Energy>();
            // if  (energy.CurEnergy < damage) {
            //     GlobalAnalysis.player_status = "trap_dead";
            //     Debug.Log("lose by: trap");
            // }
            // GlobalAnalysis.trap_damage += damage;
            col.GetComponent<Player>().TakeDamage(damage);
        }
    }
}
