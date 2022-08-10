using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Golem : EnemyMovement
{

    [SerializeField] protected Collider leftcol;
    [SerializeField] protected Collider rightcol;
    [SerializeField] protected GameObject RockPrehab;
    [SerializeField] protected Transform RockParent;


    protected GameObject Rock;
    protected Rigidbody Rockrb;
    protected bool throwmode = false;

    protected override void Start()
    {
        MaxHP = EnemyStatus.HP;
        HPSlider.value = 1;
        animator = GetComponent<Animator>();

        OffAttackCollider();
       


        Player = GameObject.FindGameObjectWithTag("Player"); //�v���C���[�Q�[���I�u�W�F�N�g���擾
        PlayerScript = Player.GetComponent<PlayerController>();  //�v���C���[�X�N���v�g���擾
        nav = GetComponent<NavMeshAgent>();
        positionVector = Player.transform.position - transform.position;
        angle = Vector3.Angle(transform.forward, positionVector); //�G���猩���v���C���[�̊p�x 
    }

    // Update is called once per frame
  protected override  void Update()
    {
        base.Update();
        animator.SetFloat("distance", distance);
        animator.SetBool("NavStop",nav.isStopped);
        //Debug.Log(HPSlider.value);
    }

    protected override void SelectAttack()
    {
        if (angle < 30)
        {

            if (AttackRange > distance)
            {
                animator.SetBool("Attack", true); //�U�������ɓ������Ƃ��ɃA�^�b�N�֑J��

                nav.isStopped = true;
                IsSelect = true;
            }
            else if (SpAttackRange > distance  && distance > 100)
            {
                animator.SetBool("SpecialAttack", true); //�U�������ɓ������Ƃ��ɃA�^�b�N�֑J��
                nav.isStopped = true;
                IsSelect = true;
            }
            else
            {
                FollowTarget();
            }
        }

    }

    protected override void StateIdle()
    {

        if (IsNotice && animationstate.IsTag("Idle")) //�v���C���[�������
        {
            LookRotate(turnSpeed);
            if (!IsSelect && !Rigidity && Target != null)
            {
                SelectAttack(); //�s�������܂��Ă��Ȃ��Ȃ�s�������߂�

            }
        }
        else if (throwmode)
        {
            LookRotate(turnSpeed);
        }
    }

    protected override void OffAttackCollider()  //AnimationEvent����Ăяo��
    {

        leftcol.enabled = false;
        rightcol.enabled = false;

    }

    protected override void OnAttackCollider()  //AnimationEvent����Ăяo��
    {
        leftcol.enabled = true;
        rightcol.enabled = true;
    }

    protected void CreateRock()
    {
       Rock = Instantiate(RockPrehab,RockParent.position, Quaternion.identity,RockParent);
        Rockrb = Rock.GetComponent<Rigidbody>();
        Rockhit Rockscipt = Rock.GetComponent<Rockhit>();
        Rockscipt.Set(this);
        throwmode = true;
    }
    
    protected void ThrowRock()
    {
        Vector3 thorwvec;

        Rock.transform.parent = null;
        Rockrb.useGravity = true;
        throwmode = false;

        if (Target != null)
        {
            thorwvec = Target.transform.position - Rock.transform.position;
        }
        else
        {
            thorwvec = positionVector;
        }
        Rockrb.AddForce(thorwvec * 150);
        Destroy(Rock, 2.0f);
    }
}
