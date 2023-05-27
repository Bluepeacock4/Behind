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
        Blink();
    }

    private void HandleMovement()
    {
        float moveX = 0;

        var spriteRenderer = GetComponent<SpriteRenderer>();

        if (!player.canControl)
            return;

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

            if (((isPlayerFacingRight && toEnemy.x < 0) || (!isPlayerFacingRight && toEnemy.x > 0)) && player.life <= 1)
            {
                Die();
            }
            else if (((isPlayerFacingRight && toEnemy.x < 0) || (!isPlayerFacingRight && toEnemy.x > 0)) && player.life > 1)
            {
                player.life--;
                player.isInvincible = true;
                StartCoroutine(InvincibilityEffect(player.invincibleTime));
            }
            else if ((!isPlayerFacingRight && toEnemy.x < 0) || (isPlayerFacingRight && toEnemy.x > 0))
            {
                Vector2 knockbackDir = transform.position - collision.transform.position;
                knockbackDir = knockbackDir.normalized;

                knockbackDir.y = 0.3f;

                rb.AddForce(knockbackDir * player.knockbackForce, ForceMode2D.Impulse);
                StartCoroutine(Knockback());
                StartCoroutine(KnockbackEffect());
            }
        }
    }

    private IEnumerator Knockback()
    {
        player.canControl = false;
        yield return new WaitForSeconds(player.staggerTime); 
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

    private IEnumerator InvincibilityEffect(float invincibleTime)
    {
        float timer = 0;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);  // 公利 面倒 公矫

        while (timer < invincibleTime)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        sr.enabled = true;

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);  // 面倒 公矫 辆丰
        player.isInvincible = false;
    }
    private void Blink()
    {

        if (!player.canControl)
            return;

        if (Input.GetKeyDown(KeyCode.E) && Time.time - lastBlinkTime > player.blinkCoolTime)
        {
            float blinkDirection = sr.flipX ? -1 : 1;
            transform.position += new Vector3(blinkDirection * player.blinkDistance, 0, 0);
            lastBlinkTime = Time.time;
        }
    }

    IEnumerator KnockbackEffect()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();

        var originalColor = spriteRenderer.color;

        // Set the sprite's color to red
        spriteRenderer.color = Color.red;

        float knockbackTime = player.staggerTime;
        float elapsed = 0f;

        while (elapsed < knockbackTime)
        {
            spriteRenderer.color = Color.Lerp(Color.red, originalColor, elapsed / knockbackTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = originalColor;
    }

    private void Attack()
    {

    }

}

