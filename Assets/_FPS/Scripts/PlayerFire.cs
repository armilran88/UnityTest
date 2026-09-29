using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    //1. 총알발사 (레이) / 충돌지점에 파편만 튀기기
    //2. 수류탄발사


    public Transform firePoint;         //총알 발사 위치
    public GameObject bImpactFactory;   //총알 파편 프리팹
    public GameObject bombFactory;      //수류탄 프리팹
    public float power = 10f;           //던질 파워

    void Update()
    {
        //마우스 왼쪽버튼 (총알발사)
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;
            
            //레이랑 충돌했냐?
            if(Physics.Raycast(ray, out hit))
            {
                print("충돌오브젝트: " + hit.collider.name);

                //충돌지점에 총알 파편만 생성하면 된다.
                GameObject bulletImpact = Instantiate(bImpactFactory);
                //부딪힌 지점
                bulletImpact.transform.position = hit.point;
                //파편이 부딪힌 지점이 향하는 방향으로 총알파편이 튀어야 자연스럽다
                //Hit 정보안에 노멀벡터의 값도 알 수 있다.
                //법선벡터, 노멀벡터 => 평면에 수직인 벡터
                bulletImpact.transform.forward = hit.normal;

                //레이어 마스크 사용 충돌처리 (최적화)
                //tag보다 약 20배 빠르다 (비트연산)
                //총 32비트를 사용하기때문에 32개까지 추가 가능
                //int layer = gameObject.layer;
                //layer = 1 << 6;  //플레이어
                ////0000 0000 0000 0001 => 0000 0000 0010 0000
                ////0000 0000 0010 0000 => 플레이어
                ////0000 0000 0000 1000 => 에너미
                ////0000 0000 1000 0000 => 보스
                ////0000 0000 1010 1000
                //layer = 1 << 8 | 1 << 4 | 1 << 6;
                //if(Physics.Raycast(ray, out hit, 100, layer)) //모두다 충돌처리
                //{
                //}

                //if (Physics.Raycast(ray, out hit, 100, ~layer)) //모두다 제외처리
                //{
                //    //if(player.layer)
                //    //if(enemy.layer)
                //}
            }

        }
        //마우스 우측버튼 (수류탄발사)
        if(Input.GetMouseButtonDown(1))
        {
            //폭탄 생성
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePoint.position;

            //폭탄은 플레이어가 던지기 때문에
            //폭탄이 들고 있는 리지드바디를 이용하면 된다
            Rigidbody rb = bomb.GetComponent<Rigidbody>();

            //45도 정도의 각도로 발사
            //벡터의 덧셈 (UP + Foward)
            //각도를 높이고 싶다 => Up의 길이를 늘려준다
            //각도를 낮추고 싶다 => Foward의 길이를 늘려준다
            //Vector3 dir = Camera.main.transform.up + Camera.main.transform.forward;
            Vector3 dir = Camera.main.transform.up + (Camera.main.transform.forward * 2f);
            dir.Normalize();
            rb.AddForce(dir * power, ForceMode.Impulse);

            //ForceMode.Impulse => 순간적인 힘을 가한다 (질량의 영향을 받는다)
            //ForceMode.VelocityChange => 순간적인 힘을 가한다 (질량의 영향을 받지 않는다)
            //ForceMode.Force => 연속적인 힘을 가한다 (질량의 영향을 받는다)
            //ForceMode.Acceleration => 연속적인 힘을 가한다 (질량의 영향을 받지 않는다)

        }

        //스나이퍼 모드
        if(Input.GetKey(KeyCode.Escape))
        {
            Camera.main.fieldOfView = 20f; //기본60 => 3배 줌
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Camera.main.fieldOfView = 60f;
        }

    }
}
