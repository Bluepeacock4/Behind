using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBase : MonoBehaviour
{
    [SerializeField] float walkSpeed, chaseSpeed, attackRange;
    [SerializeField] int AIMode, stare;
    bool AIActive;
    public bool awareSpot;
    public float awareTime;
    float idleTurCount;
    public Player player;
    public GameObject enemyAttackRange;
    bool isAttacking;
    ScoreManager scoreManager;
    [SerializeField] GameObject deathScream;
    public Portal portal;
    private bool isDead = false;
    [SerializeField] Animator animator;

    [Space]
    [SerializeField] bool TestShot;


    // Start is called before the first frame update
    void Start()
    {
        stare = 1;
        //AIActive = false;
        player = FindObjectOfType<Player>();
        scoreManager = FindObjectOfType<ScoreManager>();
        AIMode = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (TestShot)
        {
            TestShot = false;
            OnHit(); 
        }
        //print(AIActive);
        if (AIActive)
        {
            switch (AIMode)
            {
                case -1:
                    animator.SetBool("run", true);
                    if (idleTurCount < 3.0f)
                    {
                        transform.Translate(new Vector2(walkSpeed * Time.deltaTime, 0));
                        idleTurCount += walkSpeed * Time.deltaTime;
                    }
                    else
                    {
                        idleTurCount = 0;
                        AIMode = 0;
                    }
                    break;
                case 0:
                    animator.SetBool("run", true);
                    //�� ������ ����ĳ��Ʈ ���� Ȯ��
                    //����ĳ��Ʈ�� ������ ���ɰ� 3���� ��ȯ
                    RaycastHit2D EndCheck = Physics2D.Raycast(transform.position, new Vector2(0, -1), 2.0f, 1 << LayerMask.NameToLayer("PlatformEdge"));
                    if (EndCheck.collider != null)
                    {
                        //print("TurnMode: Update");
                        StartCoroutine(AIIdleTurnWait(1.2f));
                    }
                    //�ƴϸ� �̵�
                    else
                    {
                        //print("idle moving");
                        transform.Translate(new Vector2(walkSpeed * Time.deltaTime, 0));
                    }
                    break;
                case 1:
                    //�÷��̾ �÷��� ���ο� �ִ��� Ȯ��
                    if (!portal.stepOn)
                    {
                        AIResetWait(2.0f);
                    }
                    //���� ���� ����
                    else if (Mathf.Abs(player.gameObject.transform.position.x - transform.position.x) <= attackRange)
                        AIMode = 2;
                    //�÷��̾� ��ġ�� ���� ���ϴ� �����̶� ��ġ�ϴ��� Ȯ��
                    //�̵�
                    else if ((player.gameObject.transform.position.x - transform.position.x) * stare > 0)
                        transform.Translate(new Vector2(chaseSpeed * Time.deltaTime, 0));
                    //�ƴϸ� 3����
                    else
                    {
                        stare *= -1;
                        transform.Rotate(new Vector3(0, 180, 0));
                    }
                    break;
                case 2:
                    //�ִϸ��̼� Ʋ��
                    animator.SetTrigger("attack");
                    enemyAttackRange.SetActive(true);
                    //���ɱ�
                    StartCoroutine(AIAttackWait(0.8f));
                    break;
            }

        }


        if (awareTime > 0 && !awareSpot)
        {
            awareTime -= Time.deltaTime;
        }
        else if (AIMode == 1 && !awareSpot)
        {
            AIMode = 0;
        }
    }

    IEnumerator AIResetWait(float sec)
    {
        AIActive = false;
        awareSpot = false;
        awareTime = 0;
        yield return new WaitForSeconds(sec);
        AIMode = 0;
        AIActive = true;
    }
    IEnumerator AIAttackWait(float sec)
    {
        AIActive = false;
        awareSpot = true;
        awareTime = 5.5f;
        yield return new WaitForSeconds(sec);
        AIMode = 1;
        AIActive = true;
        enemyAttackRange.SetActive(false);
    }
    IEnumerator AIIdleTurnWait(float sec)
    {
        animator.SetBool("run",false);
        AIActive = false;
        //print("turnModeOn");
        yield return new WaitForSeconds(sec);
        stare *= -1;
        transform.Rotate(new Vector3(0, 180, 0));
        AIMode = -1;
        AIActive = true;
    }


    public void OnHit()
    {
        if (!isDead)
        {
            //�÷��̾� ��ġ Ȯ��
            //�տ��� ����
            if ((player.gameObject.transform.position.x - transform.position.x) * stare > 0)
            {
                //���� �ִϸ��̼�
                print("strun");
                animator.SetTrigger("hit");

                //���
                AIResetWait(2f);
            }
            //�ڿ��� ����
            else
            {
                scoreManager.scoreUp();
                scoreManager.ComboUp();
                print("die");
                //��� �ִϸ��̼�
                animator.SetTrigger("hit");
                //�ٸ� ���� ��ġä���ϱ�
                deathScream.SetActive(true);
                //����
                portal.count--;
                isDead = true;
                Destroy(this.gameObject, 0.6f);
            }
        }
    }

    public void OnAware()
    {
        if (!awareSpot && portal.stepOn)
        {
            awareSpot = true;
            AIMode = 1;
            awareTime = 5.0f;
        }

        print("spotted");
    }
    public void AwareOut()
    {
        awareSpot = false;
        print("unspotted");
    }

    public void SetAct()
    {
        AIActive = true;
        //print(AIActive);
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
