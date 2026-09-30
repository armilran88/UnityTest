using System;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //몬스터 상태 초기화
        state = EnemyState.Idle;
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
    }

    private void Move()
    {
        //1. 플레이어를 향해 이동 후 공격범위 안에 들어오면 공격상태로 변경
        //2. 플레이어를 추격하더라도 처음위치에서 일정범위를 넘어가면 리턴상태로 변경
        //- 플레이어 처럼 캐릭터컨트롤러 이용하기
        //- 공격범위 1미터
        //- 상태변경
        //- 상태전환 출력
    }

    private void Attack()
    {
        //1. 플레이어가 공격범위 안에 있다면 일정한 시간 간격으로 플레이어 공격
        //2. 플레이어가 공격범위를 벗어나면 이동상태로 변경
        //- 공격범위 1미터
        //- 상태변경
        //- 상태전환 출력
    }

    private void Return()
    {
        //1. 몬스터가 플레이어를 추격하더라도 처음 위치에서 일정 범위를 벗어나면 다시 돌아옴
        //- 처음 위치에서 일정범위 30미터
        //- 상태변경
        //- 상태전환 출력
    }

    private void Damaged()
    {
        //코루틴을 사용하자
        //1. 몬스터 체력이 1이상
        //2. 다시 이전상태로 변경
        //- 상태변경
        //- 상태전환 출력
    }

    private void Die()
    {
        //코루틴을 사용하자
        //1. 체력이 0이하
        //2. 몬스터 오브젝트 삭제
        //- 상태변경
        //- 상태전환 출력 (죽었다)
    }
}
