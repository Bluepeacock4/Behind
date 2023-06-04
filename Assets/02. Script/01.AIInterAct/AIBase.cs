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
                    //발 밑으로 레이캐스트 쏴서 확인
                    //레이캐스트에 닿으면 대기걸고 3으로 변환
                    RaycastHit2D EndCheck = Physics2D.Raycast(transform.position, new Vector2(0, -1), 2.0f, 1 << LayerMask.NameToLayer("PlatformEdge"));
                    if (EndCheck.collider != null)
                    {
                        //print("TurnMode: Update");
                        StartCoroutine(AIIdleTurnWait(1.2f));
                    }
                    //아니면 이동
                    else
                    {
                        //print("idle moving");
                        transform.Translate(new Vector2(walkSpeed * Time.deltaTime, 0));
                    }
                    break;
                case 1:
                    //플레이어가 플랫폼 내부에 있는지 확인
                    if (!portal.stepOn)
                    {
                        AIResetWait(2.0f);
                    }
                    //범위 내면 공격
                    else if (Mathf.Abs(player.gameObject.transform.position.x - transform.position.x) <= attackRange)
                        AIMode = 2;
                    //플레이어 위치가 적이 향하는 방향이랑 일치하는지 확인
                    //이동
                    else if ((player.gameObject.transform.position.x - transform.position.x) * stare > 0)
                        transform.Translate(new Vector2(chaseSpeed * Time.deltaTime, 0));
                    //아니면 3으로
                    else
                    {
                        stare *= -1;
                        transform.Rotate(new Vector3(0, 180, 0));
                    }
                    break;
                case 2:
                    //애니메이션 틀기
                    animator.SetTrigger("attack");
                    enemyAttackRange.SetActive(true);
                    //대기걸기
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
            //플레이어 위치 확인
            //앞에서 맞음
            if ((player.gameObject.transform.position.x - transform.position.x) * stare > 0)
            {
                //스턴 애니메이션
                print("strun");
                animator.SetTrigger("hit");

                //대기
                AIResetWait(2f);
            }
            //뒤에서 맞음
            else
            {
                scoreManager.scoreUp();
                scoreManager.ComboUp();
                print("die");
                //사망 애니메이션
                animator.SetTrigger("hit");
                //다른 적이 눈치채게하기
                deathScream.SetActive(true);
                //삭제
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
         AI베이스
        프레임당 호출
        대기상황용 bool 변수의 if문으로 감싸져있음 

        0은 배회상태
        문제없을 경우 이동
        난간 끝일때 대기 상태 걸고 상태 3으로 

        1은 플레이어 인지상태
        대기상태는 코루틴

        2는 공격상태
        공격 애니메이션 재생하고 대기걸고 상태 1로

        3은 고개 돌리기
        방향 뒤집고 바로 0으로


        기타 상호작용
        플레이어가 인지 범위 밖으로 벗어나면 float변수로 카운트


        피격 시
        플레어이의 앞뒤 위치 확인
            앞이면
            스턴상태의 애니메이션으로 변경하고 대기걸기
            뒤면
            HP--하고 죽을지 확인
        죽을 시
        자신이 속한 플랫폼에서 자신을 제거
        제거된것을 매니저에 전달



         */

//플랫폼의 스크립트에서 스폰된 적 리스트 할당
