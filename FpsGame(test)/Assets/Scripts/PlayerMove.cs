using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 7f;

    CharacterController cc;

    //중력변수
    float gravity = -20f;

    //수직 속력 변수
    float yVelocity = 0;

    //점프 변수
    public float jumpPower = 10f;

    //상태 변수

    public bool isJumping = false;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //이동키를 누르면 캐릭터 이동

        //1. 입력받기
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //2. 이동 방향 설정
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        //2-1. 메인 카메라를 기준으로 방향을 변환한다.

        dir = Camera.main.transform.TransformDirection(dir);

        //2-2. 만일 다시 바닥에 착지했다면
        if(cc.collisionFlags == CollisionFlags.Below)
        {
            if (isJumping)
            {
                isJumping = false;
                yVelocity = 0;
            }
        }

        //2-3. 만일, 키보드[spacebar] 키를 눌렀다면
        if(Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }

        //2-4. 캐릭터 수직 속도에 중력값을 적용
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        //3. 이동속동 맞춰 이동

        cc.Move(dir * moveSpeed * Time.deltaTime);
    }
}
