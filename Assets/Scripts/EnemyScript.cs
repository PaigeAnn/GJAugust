using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class EnemyScript : MonoBehaviour
{
    Rigidbody2D enemyRB;

    public Transform player;

    public float enemySpeed = 5f;
    public int moveSpace = 5;
    public float startPos;
    public float endPos;

    public float detectionRadius = 5f;
    public float attackRadius = 1.5f;

    public int attackDamage = 10;
    public float attackCooldown = 1f;
    private float attackTimer;

    public int enemyHealth = 100;

    public bool moveRight = true;
    public bool isFacingRight;


    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();

        startPos = transform.position.x;
        endPos = startPos + moveSpace;

        isFacingRight = transform.localScale.x > 0;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the Player has the Player tag.");
        }
    }

    void Update()
    {
        if (player == null)
            return;

        float distanceToPlayer = Vector2.Distance(
            transform.position,
            player.position
        );

        if (distanceToPlayer <= detectionRadius)
        {
            ChasePlayer(distanceToPlayer);
        }
        else
        {
            Patrol();
        }

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    void Patrol()
    {
        if (moveRight)
        {
            enemyRB.linearVelocity = new Vector2(
                enemySpeed,
                enemyRB.linearVelocity.y
            );

            if (!isFacingRight)
            {
                Flip();
            }

            if (enemyRB.position.x >= endPos)
            {
                moveRight = false;
            }
        }
        else
        {
            enemyRB.linearVelocity = new Vector2(
                -enemySpeed,
                enemyRB.linearVelocity.y
            );

            if (isFacingRight)
            {
                Flip();
            }

            if (enemyRB.position.x <= startPos)
            {
                moveRight = true;
            }
        }
    }

    void ChasePlayer(float distanceToPlayer)
    {
        if (distanceToPlayer <= attackRadius)
        {
            enemyRB.linearVelocity = new Vector2(
                0,
                enemyRB.linearVelocity.y
            );

            Attack();
            return;
        }

        if (player.position.x > transform.position.x)
        {
            enemyRB.linearVelocity = new Vector2(
                enemySpeed,
                enemyRB.linearVelocity.y
            );

            if (!isFacingRight)
            {
                Flip();
            }
        }
        else
        {
            enemyRB.linearVelocity = new Vector2(
                -enemySpeed,
                enemyRB.linearVelocity.y
            );

            if (isFacingRight)
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        transform.localScale = new Vector3(
            -transform.localScale.x,
            transform.localScale.y,
            transform.localScale.z
        );

        isFacingRight = transform.localScale.x > 0;
    }

    void Attack()
    {
        if (attackTimer <= 0)
        {
            Debug.Log("Enemy attacking player!");

            PlayerScript playerScript = player.GetComponent<PlayerScript>();

            if (playerScript != null)
            {
                playerScript.health = playerScript.health - 10;
                Debug.Log("Player was attacked for " + attackDamage + " damage!");
            }

            attackTimer = attackCooldown;
        }
    }
   
    
    

    public void Death()
    {
        if (enemyHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}