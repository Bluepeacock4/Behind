using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Tooltip("�÷��̾��� �ִ� ��� ��")]
    public int maxLife = 3;

    [Tooltip("�÷��̾��� ���� ��� ��")]
    public int currentLife;

    [Tooltip("�÷��̾��� �̵� �ӵ�")]
    public float speed;

    [Tooltip("�÷��̾��� ������")]
    public float jumpPower;

    [Tooltip("�÷��̾� ���� �ǰ� �� �з����� ��")]
    public float knockbackForce;

    [Tooltip("�÷��̾� ���� �ǰ� �� �����Ǵ� �ð�")]
    public float staggerTime;

    [Tooltip("�÷��̾� �ĸ� �ǰ� �� ����Ǵ� ���� �ð�")]
    public float invincibleTime;

    [Tooltip("�÷��̾� �����̵� �Ÿ�")]
    public float blinkDistance;

    [Tooltip("�÷��̾� �����̵� ��Ÿ��")]
    public float blinkCoolTime;

    public bool isAttacking = false;   // �÷��̾ ���� ������ ���� �Ǻ�

    public bool canControl = true;      // �÷��̾� ��Ʈ�� ���� ���� �Ǻ�

    public bool isInvincible = false;   //�÷��̾� ���� ���� �Ǻ�
}

