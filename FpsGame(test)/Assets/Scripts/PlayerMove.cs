using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 7f;

    CharacterController cc;

    //�߷º���
    float gravity = -20f;

    //���� �ӷ� ����
    float yVelocity = 0;

    //���� ����
    public float jumpPower = 10f;

    //���� ����
    public bool isJumping = false;

    public int hp = 20;
    int maxHp = 20;
    public Slider hpSlider;

    //�ǰ�
    public GameObject hitEffect;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        //�̵�Ű�� ������ ĳ���� �̵�

        //1. �Է¹ޱ�
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //2. �̵� ���� ����
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        //2-1. ���� ī�޶� �������� ������ ��ȯ�Ѵ�.

        dir = Camera.main.transform.TransformDirection(dir);

        //2-2. ���� �ٽ� �ٴڿ� �����ߴٸ�
        if(cc.collisionFlags == CollisionFlags.Below)
        {
            if (isJumping)
            {
                isJumping = false;
                yVelocity = 0;
            }
        }

        //2-3. ����, Ű����[spacebar] Ű�� �����ٸ�
        if(Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }

        //2-4. ĳ���� ���� �ӵ��� �߷°��� ����
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        //3. �̵��ӵ� ���� �̵�

        cc.Move(dir * moveSpeed * Time.deltaTime);

        hpSlider.value = (float)hp / (float)maxHp;
    }

    //�÷��̾� �ǰ��Լ�
    public void DamageAction(int damage)
    {
        hp -= damage;

       if(hp > 0)
        {
            StartCoroutine(PlayHitEffect());
        }
    }

    IEnumerator PlayHitEffect()
    {
        hitEffect.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        hitEffect.SetActive(false);
    }
}
