using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    private Player player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float lastBlinkTime;

    private void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        HandleMovement();
        HandleBlink();
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
        if (collision.gameObject.tag == "Enemy" && !player.isInvincible)
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();

            bool isPlayerFacingRight = !spriteRenderer.flipX;

            Vector2 toEnemy = collision.transform.position - transform.position;

            if (((isPlayerFacingRight && toEnemy.x < 0) || (!isPlayerFacingRight && toEnemy.x > 0)) && player.life <= 0)
            {
                Die();
            }
            else
            {
                player.life--;
                float direction = transform.localScale.x > 0 ? -1 : 1;

                rb.AddForce(new Vector2(direction * player.staggerValue, 0), ForceMode2D.Impulse);

                player.isInvincible = true;
                StartCoroutine(InvincibilityEffect(player.invincibleTime));
            }
        }
    }

    private void Die()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private IEnumerator InvincibilityEffect(float invincibleTime)
    {
        float timer = 0;
        while (timer < invincibleTime)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        sr.enabled = true;

        player.isInvincible = false;
    }
    private void HandleBlink()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.time - lastBlinkTime > player.blinkCoolTime)
        {
            float blinkDirection = sr.flipX ? -1 : 1;
            transform.position += new Vector3(blinkDirection * player.blinkDistance, 0, 0);
            lastBlinkTime = Time.time;
        }
    }

    private void Attack()
    {

    }

}

