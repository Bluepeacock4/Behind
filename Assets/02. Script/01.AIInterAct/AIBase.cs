using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBase : MonoBehaviour
{
    [SerializeField] float walkSpeed, chaseSpeed, attackRange;
    int AIMode, stare;
    bool AIActive;
    public float awareTime;

    Player player;
    // Start is called before the first frame update
    void Start()
    {
        AIActive = false;
        AIMode = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (AIActive)
        {
            switch (AIMode)
            {
                case 0:
                    //�� ������ ����ĳ��Ʈ ���� Ȯ��
                    //����ĳ��Ʈ�� ������ ���ɰ� 3���� ��ȯ
                    RaycastHit2D EndCheck = Physics2D.Raycast(transform.position, new Vector2(0, -1), 2.0f/*, ���̾� */);
                    if (EndCheck.collider != null)
                    {
                        AIMode = 3;
                    }
                    //�ƴϸ� �̵�
                    else
                        transform.Translate(new Vector2(stare * walkSpeed * Time.deltaTime, 0));
                    break;
                case 1:
                    //�÷��̾� ��ġ�� ���� ���ϴ� �����̶� ��ġ�ϴ��� Ȯ��
                    //�̵�
                    if ((player.gameObject.transform.position.x - transform.position.x) * stare > 0)
                        transform.Translate(new Vector2(stare * chaseSpeed * Time.deltaTime, 0));
                    //���� ���� ����
                    else if (Mathf.Abs(player.gameObject.transform.position.x - transform.position.x) <= attackRange)
                        AIMode = 2;
                    //�ƴϸ� 3����
                    else
                        AIMode = 3;
                    break;
                case 2:
                    //�ִϸ��̼� Ʋ��

                    //���ɱ�
                    //StartCoroutine(AIResetWait());
                    break;
                case 3:
                    stare *= -1;
                    //stare���̶� ��������Ʈ ���� ��ġ�ϴ��� Ȯ��

                    break;
            }

        }

        if (awareTime > 0)
        {
            OnAware();
            awareTime -= Time.deltaTime;
        }
        else if (AIMode == 1)
        {
            AIMode = 0;
        }
    }

    IEnumerator AIResetWait(float sec)
    {
        AIActive = false;
        yield return new WaitForSeconds(sec);
        AIMode = 0;
        AIActive = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�÷��̾� �������� Ȯ��
        //if ()
        //OnHit();
    }

    public void OnHit()
    {
        //�÷��̾� ��ġ Ȯ��
        //�տ��� ����
        if ((player.gameObject.transform.position.x - transform.position.x) * stare > 0)
        {
            //���� �ִϸ��̼�

            //���
            //AIResetWait();
        }
        //�ڿ��� ����
        else
        {
            //��� �ִϸ��̼�
        }
    }

    public void OnAware()
    {
        AIMode = 1;
    }

    public void OnAwareBackSide()
    {
        AIMode = 1;
    }
}
/*
         AI���̽�
        �����Ӵ� ȣ��
        ����Ȳ�� bool ������ if������ ���������� 

        0�� ��ȸ����
        �������� ��� �̵�
        ���� ���϶� ��� ���� �ɰ� ���� 3���� 

        1�� �÷��̾� ��������
        �����´� �ڷ�ƾ

        2�� ���ݻ���
        ���� �ִϸ��̼� ����ϰ� ���ɰ� ���� 1��

        3�� �� ������
        ���� ������ �ٷ� 0����


        ��Ÿ ��ȣ�ۿ�
        �÷��̾ ���� ���� ������ ����� float������ ī��Ʈ


        �ǰ� ��
        �÷������� �յ� ��ġ Ȯ��
            ���̸�
            ���ϻ����� �ִϸ��̼����� �����ϰ� ���ɱ�
            �ڸ�
            HP--�ϰ� ������ Ȯ��
        ���� ��
        �ڽ��� ���� �÷������� �ڽ��� ����
        ���ŵȰ��� �Ŵ����� ����



         */

//�÷����� ��ũ��Ʈ���� ������ �� ����Ʈ �Ҵ�
