using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rb;
    private Animator anim;
    private float lastBlinkTime;
    public AttackEffect attackEffect;
    private LayerMask groundLayer;

    private void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        groundLayer = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        PlayerMovement();
        Blink();

        if (Input.GetKey(KeyCode.F))
        {
            Attack();
        }
    }


    //  플레이어 이동 관리
    private void PlayerMovement()
    {
        float moveX = 0;

        if (!player.canControl || player.isAttacking)
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
        UpdateRotation(moveX);
        UpdateRunningAnimation(moveX);
        Jump();
    }

    //  플레이어 방향 설정
    private void UpdateRotation(float moveX)
    {
        if (moveX < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (moveX > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void UpdateRunningAnimation(float moveX)
    {
        anim.SetBool("isRunning", moveX != 0);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(new Vector2(0, player.jumpPower), ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }
        else
        {
            anim.SetBool("isJumping", false);
        }
    }

    //  땅과 닿고있는지 판별 
    private bool IsGrounded()
    {
        Vector2 raycastOrigin = transform.position;
        Vector2 raycastDirection = Vector2.down;
        float raycastDistance = 0.1f;

        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, raycastDirection, raycastDistance, groundLayer);

        return hit.collider != null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !player.isInvincible)
        {
            EnemyEvent(collision);
        }
    }

    //  적 충돌 시의 이벤트 관리
    private void EnemyEvent(Collision2D collision)
    {
        bool isPlayerFacingRight = transform.rotation.eulerAngles.y == 0;
        Vector2 toEnemy = collision.transform.position - transform.position;

        if (((isPlayerFacingRight && toEnemy.x < 0) || (!isPlayerFacingRight && toEnemy.x > 0)) && player.life <= 1)
        {
            Die();
        }
        else if (((isPlayerFacingRight && toEnemy.x < 0) || (!isPlayerFacingRight && toEnemy.x > 0)) && player.life > 1)
        {
            PlayerInvincibility(collision.collider);
        }
        else if ((!isPlayerFacingRight && toEnemy.x < 0) || (isPlayerFacingRight && toEnemy.x > 0))
        {
            Knockback(collision);
        }
    }

    //  플레이어 무적
    private void PlayerInvincibility(Collider2D otherCollider)
    {
        player.life--;

        player.isAttacking = false;
        player.isInvincible = true;

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), otherCollider);
        StartCoroutine(IgnoreContact(otherCollider));
    }

    //  무적 시 적과의 충돌 무시
    private IEnumerator IgnoreContact(Collider2D otherCollider)
    {
        yield return new WaitForSeconds(player.invincibleTime);

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), otherCollider, false);
        player.isInvincible = false;
    }

    //  플레이어 넉백
    private void Knockback(Collision2D collision)
    {
        player.isAttacking = false;
        Vector2 knockbackDir = transform.position - collision.transform.position;
        knockbackDir = knockbackDir.normalized;
        knockbackDir.y = 0.4f;

        rb.AddForce(knockbackDir * player.knockbackForce, ForceMode2D.Impulse);
        StartCoroutine(Stagger());
    }

    //  넉백 시 플레이어 이동 제한
    private IEnumerator Stagger()
    {
        anim.SetTrigger("Hit");
        player.canControl = false;
        yield return new WaitForSeconds(player.staggerTime);
        player.canControl = true;
    }

    private void Attack()
    {
        if (player.isAttacking || !player.canControl)
            return;

        player.isAttacking = true;
        anim.SetTrigger("Attack");
        attackEffect.gameObject.SetActive(true);
        attackEffect.PlayEffect();
    }

    public void EndAttack()
    {
        player.isAttacking = false;
    }

    private void Die()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
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
