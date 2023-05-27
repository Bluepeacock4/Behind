using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  [Tooltip("플레이어의 목숨 수")]
  public int life = 3;

  [Tooltip("플레이어의 이동 속도")]
  public float speed;

  [Tooltip("플레이어의 점프력")]
  public float jumpPower;

  [Tooltip("플레이어 정면 피격 시 밀려나는 힘")]
  public float knockbackForce;

  [Tooltip("플레이어 정면 피격 시 경직되는 시간")]
  public float staggerTime;

  [Tooltip("플레이어 후면 피격 시 적용되는 무적 시간")]
  public float invincibleTime;

  [Tooltip("플레이어 순간이동 거리")]
  public float blinkDistance;

  [Tooltip("플레이어 순간이동 쿨타임")]
  public float blinkCoolTime;

  public bool canControl = true;      // 플레이어 컨트롤 가능 여부 판별

  public bool isInvincible = false;   //플레이어 무적 여부 판별
}

