using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PiranhaBullet : MonoBehaviour
{
    [SerializeField] private LittleEnemy littleEnemy;
    public float moveSpeed;
    public int damage;
    private Transform player;
    private Vector2 target;
    private Vector2 originalPosition;
    public float shootingDistance;
    public Transform detection;
    private float ground_distance = 0.2f;
    private float wall_distance = 0.2f;

    private float direction;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        target = new Vector2(player.position.x, player.position.y);
        originalPosition = new Vector2(transform.position.x, transform.position.y);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        direction = math.sign(target.x - originalPosition.x);
    }

    // Update is called once per frame
    void Update()
    {
        // if (littleEnemy.GetBeAttackedStatus())
        // {
        //     Destroy(gameObject);
        //     return;
        // }


        if(transform.position.x == target.x && transform.position.y == target.y)
        {
            Destroy(gameObject);
        }

        if(Vector2.Distance(transform.position, originalPosition) > shootingDistance)
        {
            Destroy(gameObject);
        }

        int layer_mask_ground = LayerMask.GetMask("Ground");
        int layer_mask_wall = LayerMask.GetMask("Wall");
        RaycastHit2D groundInfo_right = Physics2D.Raycast(detection.position, Vector2.right, ground_distance, layer_mask_ground);
        RaycastHit2D groundInfo_left = Physics2D.Raycast(detection.position, Vector2.left, ground_distance, layer_mask_ground);
        RaycastHit2D wallInfo_right = Physics2D.Raycast(detection.position, Vector2.right, wall_distance, layer_mask_wall);
        RaycastHit2D wallInfo_left = Physics2D.Raycast(detection.position, Vector2.left, wall_distance, layer_mask_wall);
        if(groundInfo_right.collider == true || groundInfo_left.collider == true || wallInfo_right == true || wallInfo_left == true)
        {
            Destroy(gameObject);
        }
        
        if(direction > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if(direction < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        
        transform.position += direction * transform.right * Time.deltaTime * moveSpeed;


    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") || other.gameObject.name == "Map")
        {
            Destroy(gameObject);
        }
        if(other.gameObject.CompareTag("Player"))
        {
            // Destroy(gameObject);
            if (other.GetComponent<Energy>().CurEnergy < damage) {
                // GlobalAnalysis.player_status = "smallenemy_dead";
                // Debug.Log("lose by: small enemy");
            }
            // GlobalAnalysis.smallenemy_damage += damage;
            // other.GetComponent<Energy>().CurEnergy -= damage;
            other.GetComponent<Player>().TakeDamage(damage);
        }
    }
}
