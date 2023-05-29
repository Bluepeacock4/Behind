using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    private Player player;
    private Rigidbody2D rb;
    private Animator anim;
    private float lastBlinkTime;

    private void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovement();
        Blink();
    }

    private void HandleMovement()
    {
        float moveX = 0;

        if (!player.canControl)
            return;

        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveX = 1;
        }

        rb.velocity = new Vector2(moveX * player.speed, rb.velocity.y);

        if (moveX < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (moveX > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (moveX != 0)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            rb.AddForce(new Vector2(0, player.jumpPower), ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }
        else
        {
            anim.SetBool("isJumping", false);
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !player.isInvincible)
        {
            bool isPlayerFacingRight = transform.rotation.eulerAngles.y == 0;

            Vector2 toEnemy = collision.transform.position - transform.position;

            if (((isPlayerFacingRight && toEnemy.x < 0) || (!isPlayerFacingRight && toEnemy.x > 0)) && player.life <= 1)
            {
                Die();
            }
            else if (((isPlayerFacingRight && toEnemy.x < 0) || (!isPlayerFacingRight && toEnemy.x > 0)) && player.life > 1)
            {
                player.isInvincible = true;
                Collider2D otherCollider = collision.collider;

                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), otherCollider);
                StartCoroutine(ResetCollision(otherCollider));
            }
            else if ((!isPlayerFacingRight && toEnemy.x < 0) || (isPlayerFacingRight && toEnemy.x > 0))
            {
                Vector2 knockbackDir = transform.position - collision.transform.position;
                knockbackDir = knockbackDir.normalized;

                knockbackDir.y = 0.3f;

                rb.AddForce(knockbackDir * player.knockbackForce, ForceMode2D.Impulse);
                StartCoroutine(Knockback());
            }
        }
    }


    private IEnumerator Knockback()
    {
        anim.SetBool("isHit", true);
        player.canControl = false;
        yield return new WaitForSeconds(player.staggerTime);
        anim.SetBool("isHit", false);
        player.canControl = true;
    }

    private void Die()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private IEnumerator ResetCollision(Collider2D otherCollider)
    {
        anim.SetBool("isHit", true);
        yield return new WaitForSeconds(player.invincibleTime);

        anim.SetBool("isHit", true);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), otherCollider, false);
        player.isInvincible = false;
    }

    private void Blink()
    {
        if (!player.canControl)
            return;

        if (Input.GetKeyDown(KeyCode.E) && Time.time - lastBlinkTime > player.blinkCoolTime)
        {
            float blinkDirection = transform.rotation.eulerAngles.y == 0 || transform.rotation.eulerAngles.y == 180 ? 1 : -1;
            Vector2 blinkOffset = transform.right * blinkDirection * player.blinkDistance;
            rb.MovePosition(rb.position + blinkOffset);
            lastBlinkTime = Time.time;
        }
    }

}

