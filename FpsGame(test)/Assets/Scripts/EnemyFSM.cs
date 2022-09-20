using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    EnemyState m_State;

    public float findDistance = 8f;

    public float attackDistance = 2f;

    public float moveSpeed = 5f;

    float currentTime = 0;

    float attackDelay = 2f;

    public int attackPower = 3;

    public int hp = 15;
    int maxHp = 15;

    public Slider hpSlider;

    CharacterController cc;

    Transform player;
    Vector3 originPos;
    Quaternion originRot;

    public float moveDistance = 20f;

    Animator anim;

    NavMeshAgent smith;

    // Start is called before the first frame update
    void Start()
    {
        m_State = EnemyState.Idle;

        cc = GetComponent<CharacterController>();

        player = GameObject.Find("Player").transform;

        originPos = transform.position;
        originRot = transform.rotation;
        hp = maxHp;

        anim = transform.GetComponentInChildren<Animator>();

        smith = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(m_State)
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
                //Damaged();
                break;
            case EnemyState.Die:
                //Die();
                break;
        }

        hpSlider.value = (float)hp / (float) maxHp;
    }

    void Idle()
    {
        if(Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            anim.SetTrigger("IdleToMove");
        }
    }

    void Move()
    {
        if(Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            m_State = EnemyState.Return;

        }
        else if(Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            ////���ⱸ�ϱ�
            //Vector3 dir = (player.position - transform.position).normalized;

            ////�̵�
            //cc.Move(dir * moveSpeed * Time.deltaTime);

            ////�÷��̾ ���� ���� ��ȯ
            //transform.forward = dir;

            //������̼� ������Ʈ�� �̵��� ���߰� ��θ� �ʱ�ȭ�Ѵ�.
            smith.isStopped = true;
            smith.ResetPath();

            //������̼����� �����ϴ� �ּ� �Ÿ��� ���� ���� �Ÿ��� �����Ѵ�.
            smith.stoppingDistance = attackDistance;

            //������̼��� �������� �÷��̾��� ��ġ�� �����Ѵ�.
            smith.destination = player.position;

        }
        else
        {
            m_State = EnemyState.Attack;
            currentTime = attackDelay;

            anim.SetTrigger("MoveToAttackDelay");
        }
    }

    void Attack()
    {
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                //player.GetComponent<PlayerMove>().DamageAction(attackPower);
                currentTime = 0;

                anim.SetTrigger("StartAttack");
            }
        }
        else
        {
            m_State = EnemyState.Move;
            currentTime = 0;

            anim.SetTrigger("AttackToMove");
        }
    }
    public void AttackAction()
    {
        player.GetComponent<PlayerMove>().DamageAction(attackPower);
    }

    void Return()
    {
        if(Vector3.Distance(transform.position, originPos)> 0.1f)
        {
            //Vector3 dir = (originPos - transform.position).normalized;

            //cc.Move(dir * moveSpeed * Time.deltaTime);
            //transform.forward = dir;

            //������̼��� �������� �ʱ� ����� ��ġ�� �����Ѵ�.
            smith.destination = originPos;

            //������̼����� �����ϴ� �ּ� �Ÿ��� '0'���� �����Ѵ�.
            smith.stoppingDistance = 0;
        }
        else
        {
            smith.isStopped = true;
            smith.ResetPath();

            transform.position = originPos;
            transform.rotation = originRot;
            hp = maxHp;
            m_State = EnemyState.Idle;

            anim.SetTrigger("MoveToIdle");
        }
    }
    public void HitEnemy(int hitPower)
    {
        if(m_State == EnemyState.Damaged || m_State == EnemyState.Die || m_State == EnemyState.Return)
        {
            return;
        }
        hp -= hitPower;

        //������̼� ������Ʈ�� �̵��� ���߰� ��θ� �ʱ�ȭ�Ѵ�.
        smith.isStopped = true;
        smith.ResetPath();
        
        if(hp > 0)
        {
            m_State = EnemyState.Damaged;
            anim.SetTrigger("Damaged");
            Damaged();
        }
        else
        {
            m_State = EnemyState.Die;

            anim.SetTrigger("Die");
            Die();
        }
    }

    void Damaged()
    {
        StartCoroutine(DamageProcess());
    }

    IEnumerator DamageProcess()
    {
        yield return new WaitForSeconds(1.0f);

        m_State = EnemyState.Move;
    }

    void Die()
    {
        StopAllCoroutines();
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        cc.enabled = false;

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
