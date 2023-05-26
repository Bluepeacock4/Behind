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
                    //발 밑으로 레이캐스트 쏴서 확인
                    //레이캐스트에 닿으면 대기걸고 3으로 변환
                    RaycastHit2D EndCheck = Physics2D.Raycast(transform.position, new Vector2(0, -1), 2.0f/*, 레이어 */);
                    if (EndCheck.collider != null)
                    {
                        AIMode = 3;
                    }
                    //아니면 이동
                    else
                        transform.Translate(new Vector2(stare * walkSpeed * Time.deltaTime, 0));
                    break;
                case 1:
                    //플레이어 위치가 적이 향하는 방향이랑 일치하는지 확인
                    //이동
                    if ((player.gameObject.transform.position.x - transform.position.x) * stare > 0)
                        transform.Translate(new Vector2(stare * chaseSpeed * Time.deltaTime, 0));
                    //범위 내면 공격
                    else if (Mathf.Abs(player.gameObject.transform.position.x - transform.position.x) <= attackRange)
                        AIMode = 2;
                    //아니면 3으로
                    else
                        AIMode = 3;
                    break;
                case 2:
                    //애니메이션 틀기

                    //대기걸기
                    //StartCoroutine(AIResetWait());
                    break;
                case 3:
                    stare *= -1;
                    //stare값이랑 스프라이트 방향 일치하는지 확인

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
        //플레이어 공격인지 확인
        //if ()
        //OnHit();
    }

    public void OnHit()
    {
        //플레이어 위치 확인
        //앞에서 맞음
        if ((player.gameObject.transform.position.x - transform.position.x) * stare > 0)
        {
            //스턴 애니메이션

            //대기
            //AIResetWait();
        }
        //뒤에서 맞음
        else
        {
            //사망 애니메이션
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
