using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScreamBox : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
            GetComponentInParent<AIBase>().awareTime = 5.0f;
    }
}
