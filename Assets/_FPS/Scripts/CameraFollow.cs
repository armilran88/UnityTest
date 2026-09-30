using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //카메라가 플레이어를 따라다니기
    //플레이어한테 바로 카메라를 자식으로 붙여서 이동해도 상관없다
    //하지만 게임에 따라 드라마틱한 연출이 필요한 경우
    //타겟을 따라다니도록 해서 처리를 한다
    //또한 1인칭, 3인칭 으로 변경도 자유롭게 할 수 있다
    //지금은 우리 눈 역할을 할거라서 그냥 순간이동 시킨다

    public float speed = 5f;            //카메라 이동 속도
    //public Transform target;          //카메라가 따라다닐 타겟
    public Transform target1st;         //카메라가 따라다닐 타겟 1인칭 시점
    public Transform target3rd;         //카메라가 따라다닐 타겟 3인칭 시점
    bool isFPS = true;



    void Update()
    {
        //1인칭 to 3인칭, 3인칭 to 1인칭으로 카메라 변경
        ChageView();
    }

    void ChageView()
    {
        if(Input.GetKeyDown("1"))
        {
            isFPS = true;
        }
        if(Input.GetKeyDown("3"))
        {
            isFPS = false;
        }

        if(isFPS) //1인칭이냐?
        {
            //카메라 위치를 강제로 타겟위치로 고정하자
            transform.position = target1st.position;
        }
        else //3인칭이냐?
        {
            //카메라 위치를 강제로 타겟위치로 고정하자
            transform.position = target3rd.position;
        }
    }



    private void LateUpdate()
    {
        //타겟 따라다니기
        //FollowTarget();
    }

    //void FollowTarget()
    //{
    //    //카메라가 플레이어를 따라다닐때 주의사항
    //    //이때는 Update()가 아닌 LateUpdate()에 넣어주는게 좋다

    //    //타겟의 방향 구하기 (벡터의 뺄셈)
    //    //방향 = 타겟 - 자기자신
    //    Vector3 dir = target.position - transform.position;
    //    dir.Normalize();
    //    transform.Translate(dir * speed * Time.deltaTime);

    //    if(Vector3.Distance(transform.position, target.position) < 1.0f)
    //    {
    //        transform.position = target.position;
    //    }
    //}
}
