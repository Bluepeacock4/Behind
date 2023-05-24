using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    private Player player;
    private Rigidbody2D rb;

    private void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float moveX = 0;

        var spriteRenderer = GetComponent<SpriteRenderer>();

        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1;
            spriteRenderer.flipX = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveX = 1;
            spriteRenderer.flipX = false;
        }

        rb.velocity = new Vector2(moveX * player.speed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            rb.AddForce(new Vector2(0, player.jumpPower), ForceMode2D.Impulse);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.tag == "Attacking")
            {
                var spriteRenderer = GetComponent<SpriteRenderer>();

                bool isPlayerFacingRight = !spriteRenderer.flipX;

                Vector2 toEnemy = collision.transform.position - transform.position;

                if ((isPlayerFacingRight && toEnemy.x < 0) || (!isPlayerFacingRight && toEnemy.x > 0))
                {
                    Die();
                }
                else
                {
                    player.life--;

                    float direction = transform.localScale.x > 0 ? -1 : 1;

                    rb.AddForce(new Vector2(direction * player.staggerValue, 0), ForceMode2D.Impulse);
                }
            }
        }
    }

    //public void TakeDamage(int damage)
    //{
    //    player.health -= damage;

    //    if (player.health <= 0)
    //    {
    //        Die();
    //    }
    //}

    private void Die()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void Attack()
    {

    }

}

