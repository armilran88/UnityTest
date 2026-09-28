using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    //카메라를 마우스 움직이는 방향으로 회전하기
    public float speed = 150f;      //회전속도 (Time.DeltaTime 통해 1초당 150도 회전)
    //회전 각도를 직접 제어하기 위한 변수
    float angleX, angleY;


    void Update()
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
        //Vector3 dir = new Vector3(h, v, 0);
        //transform.Rotate(dir * speed * Time.deltaTime);

        //회전은 각각의 축을 기반으로 회전처리가 된다
        //Vector3 dir = new Vector3(-v, h, 0);
        //transform.Rotate(dir * speed * Time.deltaTime);

        //회전을 담당하는 Rotate함수를 사용하면
        //우리가 직접 제어 하기 힘들다
        //인스펙터창의 로테이션 값은 우리가 보기 편한 오일러각도로 표시되지만
        //내부적으로 회전처리는 쿼터니온으로 처리된다.

        angleX += h * speed * Time.deltaTime;
        angleY += v * speed * Time.deltaTime;
        angleY = Mathf.Clamp(angleY, -60, 60);
        //Mathf.Clamp
        //if(angleY < -60) angleY = -60;
        //if(angleY > 60) angleY = 60;
        transform.eulerAngles = new Vector3(-angleY, angleX, 0);
    }
}
