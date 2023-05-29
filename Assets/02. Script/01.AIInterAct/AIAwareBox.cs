using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAwareBox : MonoBehaviour
{
    //�ݶ��̴��� ������ �ִ� ������Ʈ�� ����
    //�� ������Ʈ�� �ڽ� ������Ʈ�� ���� ��Ŵ

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
