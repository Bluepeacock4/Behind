using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAwareBox : MonoBehaviour
{
    //�ݶ��̴��� ������ �ִ� ������Ʈ�� ����
    //�� ������Ʈ�� �ڽ� ������Ʈ�� ���� ��Ŵ

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
