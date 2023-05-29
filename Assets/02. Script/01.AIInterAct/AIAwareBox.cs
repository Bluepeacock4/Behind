using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAwareBox : MonoBehaviour
{
    //콜라이더를 가지고 있는 오브젝트에 포함
    //적 오브젝트에 자식 오브젝트로 포함 시킴

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
            GetComponentInParent<AIBase>().OnAware();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
            GetComponentInParent<AIBase>().OnAware();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
            GetComponentInParent<AIBase>().AwareOut();
    }
}
