using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    //플레이어 좌우 회전처리
    public float speed = 150f;
    //회전각도 직접 제어하기
    float angleX;

    void Update()
    {
        float h = Input.GetAxis("Mouse X");
        angleX += h * speed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, angleX, 0);
    }
}
