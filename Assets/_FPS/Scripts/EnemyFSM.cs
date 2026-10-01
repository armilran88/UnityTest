using System;
using System.Collections;
using UnityEngine;

//몬스터 유한상태머신
public class EnemyFSM : MonoBehaviour
{
    //FSM => 상태에 따라서 코딩하는것 (애니메이터에서 애니메이션 작업을 하는게 FSM기반이다)

    //몬스터 상태 이넘문
    enum EnemyState
    {
        Idle, Move, Attack, Return, Damaged, Die
    }

    EnemyState state;       //몬스터 상태변수


    public float findRange = 15f;           //플레이어를 찾는 범위
    public float moveRange = 30f;           //시작지점에서 최대 이동가능한 범위
    public float attackRange = 2f;          //공격 가능 범위
    Vector3 startPoint;                     //몬스터 시작위치
    Transform player;                       //플레이어를 찾기 위해
    CharacterController cc;                 //몬스터 이동을 위해

    //몬스터 일반변수
    int hp = 100;
    int att = 5;
    float speed = 5f;

    //공격 딜레이
    float attTime = 2f;    //2초에 한번 공격
    float timer = 0f;       //타이머

    void Start()
    {
        //몬스터 상태 초기화
        state = EnemyState.Idle;
        //시작지점 저장
        startPoint = transform.position;
        //플레이어 트렌스폼
        player = GameObject.Find("Player").transform;
        //캐릭터컨트롤러 컴포넌트
        cc = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        //상태에 따른 행동처리
        switch(state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                Damaged();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
    }

    private void Idle()
    {
        //1. 플레이어와 일정범위가 되면 이동상태로 변경 (탐지범위)
        //- 플레이어 찾기
        //- 일정거리 20미터 (거리비교: Distance, magnitude sqrMagnitude 아무거나)
        //- 상태변경 => 이동
        //- 상태전환 출력

        //
        if(Vector3.Distance(transform.position, player.position) < findRange)
        {
            state = EnemyState.Move;
            print("상태전환: Idle -> Move");
            //anim.SetTrigger("Move");
        }
    }

    private void Move()
    {
        //1. 플레이어를 향해 이동 후 공격범위 안에 들어오면 공격상태로 변경
        //2. 플레이어를 추격하더라도 처음위치에서 일정범위를 넘어가면 리턴상태로 변경
        //- 플레이어 처럼 캐릭터컨트롤러 이용하기
        //- 공격범위 1미터
        //- 상태변경
        //- 상태전환 출력

        //이동중 이동할수 있는 최대범위를 벗어났을때
        if(Vector3.Distance(transform.position, startPoint) > moveRange)
        {
            state = EnemyState.Return;
            print("상태전환: Move -> Return");
        }
        else if(Vector3.Distance(transform.position, player.position) > attackRange)
        {
            //플레이어를 향해 추격
            //이동방향 (벡터의 뺄셈)
            Vector3 dir = (player.position - transform.position);
            dir.Normalize();

            //몬스터가 백스텝으로 쫒아온다
            //몬스터가 타겟(플레이어)를 바라보도록 하자
            //방법1 젤 심플
            //transform.forward = dir;
            //방법2 LookAt
            //transform.LookAt(player);

            //좀더 자연스럽 회전처리 하고 싶다
            //transform.forward = Vector3.Lerp(transform.forward, dir, 10 * Time.deltaTime);
            //혹시나 타겟과 본인이 일직선상일경우 백덤플링으로 회전할때가 있다

            //최종적으로는 자연스런 회전처리를 위해 쿼터니온을 사용해야 한다
            transform.rotation = Quaternion.Lerp(
                transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);

            //캐릭터 컨트롤러 이용해서 이동
            //cc.Move(dir * speed * Time.deltaTime);
            //캐릭터컨트롤러안에 SimpleMove() => 최소한의 물리가 적용되서 중력문제도 해결할 수 있다.
            //단 내부적으로 시간처리를 하기때문에 Time.deltaTime을 곱하지 않는다
            cc.SimpleMove(dir * speed);
        }
        else //공격범위 안에 들어옴
        {
            state = EnemyState.Attack;
            print("상태전환: Move -> Attack");
        }
    }

    private void Attack()
    {
        //1. 플레이어가 공격범위 안에 있다면 일정한 시간 간격으로 플레이어 공격
        //2. 플레이어가 공격범위를 벗어나면 이동상태로 변경
        //- 공격범위 1미터
        //- 상태변경
        //- 상태전환 출력

        //공격범위안에 들어옴
        if(Vector3.Distance(transform.position, player.position) < attackRange)
        {
            //일정 시간마다 플레이어 공격하기
            timer += Time.deltaTime;
            if(timer > attTime)
            {
                print("공격");

                //플레이어의 필요한 스크립트 컴포넌트를 가져와서 데미지를 주면 된다
                //player.GetComponent<PlayerMove>().hitDamage(att);

                //타이머 초기화
                timer = 0f;
            }
        }
        else //현재 상태를 무브로 전환하기 (재추격)
        {
            state = EnemyState.Move;
            print("상태전환: Attack -> Move");
            //타이머 초기화
            timer = 0f;
        }
    }

    private void Return()
    {
        //1. 몬스터가 플레이어를 추격하더라도 처음 위치에서 일정 범위를 벗어나면 다시 돌아옴
        //- 처음 위치에서 일정범위 30미터
        //- 상태변경
        //- 상태전환 출력

        //시작위치로 돌아가기
        //돌아가면 대기상태로 변경
        if(Vector3.Distance(transform.position, startPoint) > 0.1f)
        {
            Vector3 dir = (startPoint - transform.position).normalized;
            cc.SimpleMove(dir * speed);
        }
        else
        {
            //위치값을 초기값으로 고정
            transform.position = startPoint;
            state = EnemyState.Idle;
            print("상태전환: Return -> Idle");
        }
    }

    public void HitDamage(int value)
    {
        //예외처리
        //피격상태이거나, 죽은 상태일때는 데미지 중첩으로 적용하지 않는다
        if (state == EnemyState.Damaged || state == EnemyState.Die) return;

        //체력깍기
        hp -= value;

        //몬스터의 체력이 1이상이면 피격상태 그외는 죽음뿐
        if(hp > 0)
        {
            state = EnemyState.Damaged;
            print("상태전환: AnyState -> Damaged");
            print("HP: " + hp);

            Damaged();
        }
        else
        {
            state = EnemyState.Die;
            print("상태전환: AnyState -> Die");

            Die();
        }
    }


    private void Damaged()
    {
        //코루틴을 사용하자
        //1. 몬스터 체력이 1이상
        //2. 다시 이전상태로 변경
        //- 상태변경
        //- 상태전환 출력

        //피격 상태를 처리하기 위한 코루틴
        StartCoroutine(DamageProc());
    }

    IEnumerator DamageProc()
    {
        //피격모션 시간만큼 기다리기
        yield return new WaitForSeconds(1.0f);
        //현재상태를 이동으로 전환
        state = EnemyState.Move;
        print("상태전환: Damaged -> Move");
    }

    private void Die()
    {
        //코루틴을 사용하자
        //1. 체력이 0이하
        //2. 몬스터 오브젝트 삭제
        //- 상태변경
        //- 상태전환 출력 (죽었다)

        //죽음상태를 처리하기 위한 코루틴
        StartCoroutine(DieProc());
    }

    IEnumerator DieProc()
    {
        //2초후에 자기자신을 제거한다
        yield return new WaitForSeconds(2.0f);
        print("죽었다!!!");
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        //공격가능범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        //플레이어 탐지 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, findRange);
        //시작지점에서 이동가능한 최대범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(startPoint, moveRange);
    }
}
