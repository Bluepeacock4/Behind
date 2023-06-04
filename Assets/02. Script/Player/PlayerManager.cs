using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rb;
    private Animator anim;
    private float lastBlinkTime;
    private bool isCooldown = false;
    public AttackEffect attackEffect;
    public Image skillCooldownImage;
    public Image[] lifeImages;
    public GameObject gameOverUI;
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


    //  �÷��̾� �̵� ����
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

    //  �÷��̾� ���� ����
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

    //  ���� ����ִ��� �Ǻ� 
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

    //  �� �浹 ���� �̺�Ʈ ����
    private void EnemyEvent(Collision2D collision)
    {
        bool isPlayerFacingRight = transform.rotation.eulerAngles.y == 0;
        Vector2 toEnemy = collision.transform.position - transform.position;

        if (((isPlayerFacingRight && toEnemy.x < 0) || (!isPlayerFacingRight && toEnemy.x > 0)) && player.currentLife <= 1)
        {
            player.currentLife--;
            UpdateLifeUI();
            GameOver();
        }
        else if (((isPlayerFacingRight && toEnemy.x < 0) || (!isPlayerFacingRight && toEnemy.x > 0)) && player.currentLife > 1)
        {
            player.currentLife--;
            UpdateLifeUI();
            PlayerInvincibility(collision.collider);
        }
        else if ((!isPlayerFacingRight && toEnemy.x < 0) || (isPlayerFacingRight && toEnemy.x > 0))
        {
            Knockback(collision);
        }
    }

    private void UpdateLifeUI()
    {
        for (int i = 0; i < lifeImages.Length; i++)
        {
            if (i < player.currentLife)
            {
                lifeImages[i].enabled = true;  // �ش� ������ �̹��� Ȱ��ȭ
            }
            else
            {
                lifeImages[i].enabled = false;  // �ش� ������ �̹��� ��Ȱ��ȭ
            }
        }
    }

    //  �÷��̾� ����
    private void PlayerInvincibility(Collider2D otherCollider)
    {
        if (otherCollider == null) return;
        StartCoroutine(RedScreenEffect());
        player.maxLife--;

        player.isAttacking = false;
        player.isInvincible = true;

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), otherCollider);
        StartCoroutine(IgnoreContact(otherCollider));
    }

    //  ���� �� ������ �浹 ����
    private IEnumerator IgnoreContact(Collider2D otherCollider)
    {
        if (otherCollider == null) yield break;
        yield return new WaitForSeconds(player.invincibleTime);

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), otherCollider, false);
        player.isInvincible = false;
    }

    //  �÷��̾� �˹�
    private void Knockback(Collision2D collision)
    {
        player.isAttacking = false;
        Vector2 knockbackDir = transform.position - collision.transform.position;
        knockbackDir = knockbackDir.normalized;
        knockbackDir.y = 0.4f;

        rb.AddForce(knockbackDir * player.knockbackForce, ForceMode2D.Impulse);
        StartCoroutine(Stagger());
    }

    //  �˹� �� �÷��̾� �̵� ����
    private IEnumerator Stagger()
    {
        anim.SetTrigger("Hit");
        anim.SetBool("isRunning", false);
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

    private void GameOver()
    {
        gameOverUI.SetActive(true);
        GameManager.Instance.PauseGame();
    }

    private void Blink()
    {
        if (!player.canControl || isCooldown)
            return;

        if (Input.GetKeyDown(KeyCode.E) && Time.time - lastBlinkTime > player.blinkCoolTime)
        {
            float blinkDirection = transform.rotation.eulerAngles.y == 0 || transform.rotation.eulerAngles.y == 180 ? 1 : -1;
            Vector2 blinkOffset = transform.right * blinkDirection * player.blinkDistance;
            rb.MovePosition(rb.position + blinkOffset);
            lastBlinkTime = Time.time;

            // ��ų ��Ÿ�� �ڷ�ƾ ����
            StartCoroutine(SkillCooldown());

            // ������ �浹 ��Ȱ��ȭ �ڷ�ƾ ����
            StartCoroutine(DisableEnemyCollision());
        }
    }

    private IEnumerator DisableEnemyCollision()
    {
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        int playerLayer = LayerMask.NameToLayer("Player");

        // Enemy ���̾�� Player ���̾��� �浹�� �����մϴ�.
        Physics2D.IgnoreLayerCollision(enemyLayer, playerLayer, true);

        yield return new WaitForSeconds(player.blinkInvincibleTime);

        // Enemy ���̾�� Player ���̾��� �浹 ���ø� �����մϴ�.
        Physics2D.IgnoreLayerCollision(enemyLayer, playerLayer, false);
    }

    private IEnumerator SkillCooldown()
    {
        isCooldown = true;

        float time = 0f;
        skillCooldownImage.fillAmount = 1f;

        while (time < player.blinkCoolTime)
        {
            time += Time.deltaTime;
            skillCooldownImage.fillAmount = 1 - (time / player.blinkCoolTime);
            yield return null;
        }

        skillCooldownImage.fillAmount = 0f;

        isCooldown = false;
    }

    public Image redScreen;
    public float redScreenDuration = 0.2f;
    public float maxAlpha = 0.45f;

    IEnumerator RedScreenEffect()
    {
        float time = 0f;

        while (time < redScreenDuration / 2)
        {
            time += Time.deltaTime;
            Color redScreenColor = redScreen.color;
            redScreenColor.a = (time / (redScreenDuration / 2)) * maxAlpha;
            redScreen.color = redScreenColor;
            yield return null;
        }

        time = 0f;

        while (time < redScreenDuration / 2)
        {
            time += Time.deltaTime;
            Color redScreenColor = redScreen.color;
            redScreenColor.a = maxAlpha - ((time / (redScreenDuration / 2)) * maxAlpha);
            redScreen.color = redScreenColor;
            yield return null;
        }

        Color finalColor = redScreen.color;
        finalColor.a = 0f;
        redScreen.color = finalColor;
    }

}
