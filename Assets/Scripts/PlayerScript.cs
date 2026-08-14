using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    public float moveSpeed = 10f;

    private Vector2 dir;
    private Rigidbody2D rb2d;

    private bool facingLeft = false;
    private bool isAttacking = false;

    public int damage = 10;
    public int health = 100;


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        if (rb2d == null)
        {
            Debug.LogError("Player does not have a Rigidbody2D!");
        }
    }

    public void MovePlayer(InputAction.CallbackContext ctx)
    {
        dir = ctx.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + dir * moveSpeed * Time.fixedDeltaTime);
    }

    void Update()
    {
        FlipPlayer();
    }

    void FlipPlayer()
    {
        if (dir.x > 0 && facingLeft)
        {
            facingLeft = false;

            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else if (dir.x < 0 && !facingLeft)
        {
            facingLeft = true;

            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    public void Attack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !isAttacking)
        {
            Debug.Log("Attacking");
            isAttacking = true;
        }
        if (ctx.canceled && isAttacking)
        {
            isAttacking = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isAttacking && other.CompareTag("Enemy"))
        {
            EnemyScript enemy = other.GetComponent<EnemyScript>();

            if (enemy != null)
            {
                enemy.enemyHealth -= damage;
                Debug.Log("Enemy Health: " + enemy.enemyHealth);
                if (enemy.enemyHealth <= 0)
                {
                    enemy.Death();
                }
            }
        }
    }
}