using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAwareBox : MonoBehaviour
{
    //콜라이더를 가지고 있는 오브젝트에 포함
    //적 오브젝트에 자식 오브젝트로 포함 시킴

    Player player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player.transform.gameObject)
            GetComponentInParent<AIBase>().awareTime = 5.0f;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == player.transform.gameObject)
            GetComponentInParent<AIBase>().awareTime = 5.0f;
    }
}
