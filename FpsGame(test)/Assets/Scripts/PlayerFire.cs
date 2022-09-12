using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public GameObject firePosition;

    public GameObject bombFactroy;

    public float throwPower = 15f;

    public GameObject bulletEffect;

    ParticleSystem ps;

    private void Start()
    {
        ps = bulletEffect.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetMouseButtonDown(1))
        {
            GameObject bomb = Instantiate(bombFactroy);
            bomb.transform.position = firePosition.transform.position;

            Rigidbody rb = bomb.GetComponent<Rigidbody>();

            rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
        }

        if(Input.GetMouseButton(0))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            RaycastHit hitInfo = new RaycastHit();

            if(Physics.Raycast(ray, out hitInfo))
            {
                //�ǰ� ����Ʈ�� ��ġ�� ���̰� �ε��� �������� �̵���Ų��.
                bulletEffect.transform.position = hitInfo.point;
                //���
                ps.Play();
            }
        }
    }
}
