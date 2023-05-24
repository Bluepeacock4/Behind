using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBase : MonoBehaviour
{
    [SerializeField] float walkSpeed, chaseSpeed;
    int AIMode, stare;
    bool AIActive;
    [SerializeField] float awareTime;
    // Start is called before the first frame update
    void Start()
    {

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
                        AIMode = 3;
                    //�ƴϸ� �̵�
                    else
                        transform.Translate(new Vector2(stare * walkSpeed * Time.deltaTime, 0));
                    break;
                case 1:
                    //�÷��̾� ��ġ�� ���� ���ϴ� �����̶� ��ġ�ϴ��� Ȯ��
                    //�ƴϸ� 3����

                    //�׳� �̵�
                    transform.Translate(new Vector2(stare * chaseSpeed * Time.deltaTime, 0));
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
            awareTime -= Time.deltaTime;
        }

        //���鿡 �ڽ� ����ĳ��Ʈ �߻�

        //
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
        //���� �ִϸ��̼�

        //���
        //AIResetWait();
        //�ڿ��� ����
        //��� �ִϸ��̼�


    }

    public void OnAware()
    {

    }

    public void OnAwareBackSide()
    {

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
