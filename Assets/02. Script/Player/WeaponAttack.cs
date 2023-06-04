using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && player.isAttacking)
        {
            collision.gameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
        }
    }
}
