using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public GameObject firePosition;

    public GameObject bombFactroy;

    public float throwPower = 15f;
    public int weaponPower = 5;

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
                if(hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(weaponPower);
                }
                else
                {
                    //피견 이펙트의 위치를 레이가 부딪힌 지점으로 이동시킨다.
                    bulletEffect.transform.position = hitInfo.point;

                    bulletEffect.transform.forward = hitInfo.normal;
                    //재생
                    ps.Play();
                }

            }
        }
    }
}
