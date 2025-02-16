using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGround : MonoBehaviour
{
    [SerializeField] private LittleEnemy littleEnemy;
    [SerializeField] private float movementDistance;

    [SerializeField] public Boolean hasTwoLives;
    [SerializeField] public bool isRoundWalk;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    public VisualEffectSystemManager visualEffectSystemManager;

    private float speed;
    private int damage;
    private bool movingLeft;
    private float leftEdge;
    private float rightEdge;
    private Animator anim;
    private Boolean beAttacked = false;
    private string direction = "right";
    public Transform groundCheck;
    private float width;
    private float height;
    private SpriteRenderer sp;
    private float originalSpeed;
    private Collider2D collider;


    private void Awake()
    {
        speed = littleEnemy.speed;
        damage = littleEnemy.damage;
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
        anim = GetComponent<Animator>();
        width = GetComponent<Collider2D>().bounds.size.x;
        height = GetComponent<Collider2D>().bounds.size.y;
        sp = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        originalSpeed = speed;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckDistance);
        Gizmos.DrawLine(transform.position,
            new Vector3(transform.position.x + movementDistance, transform.position.y, transform.position.z));
        Gizmos.DrawLine(transform.position,
            new Vector3(transform.position.x - movementDistance, transform.position.y, transform.position.z));
    }

    private void FixedUpdate()
    {
        if (littleEnemy.GetBeAttackedStatus())
        {
            LittleEnemyDeath();
            return;
        }
        if (isRoundWalk)
        {
            bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckDistance, groundLayer);
            if (!isGrounded)
            {
                switch (direction)
                {
                    // current moving right 
                    case "right":
                        transform.Rotate(0, 0, -90);
                        transform.position += new Vector3(height / 2, -width, 0);
                        direction = "down";
                        break;
                    case "down":

                        transform.Rotate(0, 0, -90);
                        transform.position += new Vector3(-width, -height / 2, 0);
                        direction = "left";
                        break;
                    case "left":
                        transform.Rotate(0, 0, -90);
                        transform.position += new Vector3(-height / 2, width, 0);
                        direction = "up";
                        break;
                    case "up":

                        transform.Rotate(0, 0, -90);
                        transform.position += new Vector3(width, height / 2, 0);
                        direction = "right";
                        break;
                }
            }
            else
            {
                switch (direction)
                {
                    // current moving right 
                    case "right":
                        transform.position += new Vector3(speed * 0.02f , 0, 0);
                        break;
                    case "down":
                        transform.position += new Vector3(0, -speed * 0.02f , 0);
                        break;
                    case "left":
                        transform.position += new Vector3(-speed * 0.02f , 0, 0);
                        break;
                    case "up":
                        transform.position += new Vector3(0, speed * 0.02f , 0);
                        break;
                }
            }
        }
        else
        {
            if (movingLeft)
            {
                if (transform.position.x > leftEdge)
                {
                    transform.position += new Vector3(-speed * 0.02f , 0, 0);
                }
                else
                {
                    movingLeft = false;
                    sp.flipX = false;
                }
            }
            else
            {
                if (transform.position.x < rightEdge)
                {
                    transform.position += new Vector3(speed * 0.02f , 0, 0);
                }
                else
                {
                    movingLeft = true;
                    sp.flipX = true;
                }
            }
        }
    }


    public void LittleEnemyDeath()
    {
        littleEnemy.SetBeAttackedStatus(false);
        if (!hasTwoLives || beAttacked)
        {
            littleEnemy.SetDeathStatus(true);
            speed = 0;
            originalSpeed = 0;
            collider.enabled = false;
            anim.SetTrigger("Death");
            visualEffectSystemManager.GenerateEvilPurpleExplode(transform);
        }
        else
        {
            littleEnemy.SetDeathStatus(false);
            beAttacked = true;
            transform.localScale = new Vector3(transform.localScale.x * 0.5f, transform.localScale.y * 0.5f,
                transform.localScale.z * 0.5f);
            transform.position += new Vector3(0, -height, 0);
            originalSpeed *= 2f;
            visualEffectSystemManager.GenerateBleedParticleEffect(transform);
            anim.SetTrigger("Hurt");
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (col.GetComponent<Energy>().CurEnergy < damage)
            {
                // GlobalAnalysis.player_status = "smallenemy_dead";
                // Debug.Log("lose by: small enemy");
            }

            // GlobalAnalysis.smallenemy_damage += damage;
            col.GetComponent<Player>().TakeDamage(damage);
        }
    }

    private void Freeze()
    {
        // Debug.Log("Freeze position");
        speed = 0f;
    }

    private void Unfreeze()
    {
        speed = originalSpeed;
    }

    private void Deactivate()
    {
        
        gameObject.SetActive(false);
    }
}