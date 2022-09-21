using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{

    public enum WeaponMode
    {
        Normal,
        Sniper
    }
    WeaponMode wMode;

    bool ZoomMode = false;

    public GameObject firePosition;

    public GameObject bombFactroy;

    public float throwPower = 15f;
    public int weaponPower = 5;

    public GameObject bulletEffect;

    ParticleSystem ps;
    Animator anim;

    public Text wModeText;

    public GameObject[] eff_Flash;

    private void Start()
    {
        ps = bulletEffect.GetComponent<ParticleSystem>();
        anim = GetComponentInChildren<Animator>();
        wMode = WeaponMode.Normal;
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        
        //노멀모드 : 마우스 오른쪽 버튼을 누르면 시ㅓㄴ 방향으로 수류탄을 던지고 싶다.
        //스나이퍼 모드 : 마우스 오른쪽 버튼을 누르면 화면으 확대하고 싶다.

        if (Input.GetMouseButtonDown(1))
        {
            switch(wMode)
            {
                case WeaponMode.Normal:
                    GameObject bomb = Instantiate(bombFactroy);
                    bomb.transform.position = firePosition.transform.position;

                    Rigidbody rb = bomb.GetComponent<Rigidbody>();

                    rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
                    break;

                case WeaponMode.Sniper:
                    
                    if(!ZoomMode)
                    {
                        Camera.main.fieldOfView = 15f;
                        ZoomMode = true;
                    }

                    else
                    {
                        Camera.main.fieldOfView = 60f;
                        ZoomMode = false;
                    }
                    break;
            }

        }

        if(Input.GetMouseButton(0))
        {
            if(anim.GetFloat("MoveMotion") == 0)
            {
                anim.SetTrigger("Attack");
            }
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

            StartCoroutine(ShootEffectOn(0.05f));
        }
        
        IEnumerator ShootEffectOn(float duration)
        {
            int num = Random.Range(0, eff_Flash.Length);
            eff_Flash[num].SetActive(true);
            yield return new WaitForSeconds(duration);
            eff_Flash[num].SetActive(false);
        }

        //숫자 1번키를 누르면
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            wMode = WeaponMode.Normal;
            Camera.main.fieldOfView = 60f;

            wModeText.text = "Normal Mode";
        }    
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            wMode = WeaponMode.Sniper;
            wModeText.text = "Sniper Mode";
        }
    }
}
