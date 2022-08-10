using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.AI;
using UnityEngine.UI;
public class FriendMonster : EnemyMovement
{


    // Start is called before the first frame update
  protected override void Start()
    {
        MaxHP = EnemyStatus.HP;
        HPSlider.value = 1;
        animator = GetComponent<Animator>();
        GameObject body = Getbody();
        attackcol = body.GetComponent<Collider>();
     

        attackcol.enabled = false;


        Player = GameObject.FindGameObjectWithTag("Player"); //�v���C���[�Q�[���I�u�W�F�N�g���擾
        PlayerScript = Player.GetComponent<PlayerController>();  //�v���C���[�X�N���v�g���擾
        nav = GetComponent<NavMeshAgent>();
        positionVector = Player.transform.position - transform.position;
        angle = Vector3.Angle(transform.forward, positionVector); //�G���猩���v���C���[�̊p�x

    }

    // Update is called once per frame
protected override void Update()
    {
        animationstate = animator.GetCurrentAnimatorStateInfo(0);
        animatorclip = animator.GetCurrentAnimatorClipInfo(0);
        AnimationTime = animatorclip[0].clip.length * animationstate.normalizedTime; //�A�j���[�V�����̎��Ԃ̎擾
        if (Target != null) 
        {
            positionVector = Target.transform.position - transform.position;
            distance = positionVector.sqrMagnitude;
            angle = Vector3.Angle(transform.forward, positionVector); //Target�̊p�x
            int TargetHP = Target.GetComponent<EnemyMovement>().GetHPValue();
            if (TargetHP <= 0)
            {
                ResetTarget();
            }
        }
        else
        {
            
            positionVector = Player.transform.position - transform.position;
            distance = positionVector.sqrMagnitude;//�v���C���[�Ƃ̋���
            angle = Vector3.Angle(transform.forward, positionVector); //�G���猩���v���C���[�̊p�x
        }
      
        animator.SetBool("IsNotice", IsNotice);

        //  Debug.Log(IsNotice);

        if (!die)
        {

            StateIdle();
            JudgeAttackHit();

        }

        else if (!Rigidity)
        {
            Die();
        }
       
    }

    protected override void StateIdle()
    {

        if (animationstate.IsTag("Idle") && !Rigidity) //�v���C���[�������,�d�����Ȃ���
        {
            LookRotate(turnSpeed);
            if (!IsSelect)
            {
                SelectAttack(); //�s�������܂��Ă��Ȃ��Ȃ�s�������߂�

            }
        }
    }

    protected override void SelectAttack()
    {
        if (Target == null && angle < 30)
        {
            nav.destination = Player.transform.position;
            Navstart();
           
        }

       else if (angle < 30)
        {

            if (AttackRange > distance)
            {
                animator.SetBool("Attack", true); //�U�������ɓ������Ƃ��ɃA�^�b�N�֑J��
                nav.isStopped = true;
                IsSelect = true;
            }
            else if (SpAttackRange > distance)
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

    async public override void Attack(Collider target)
    {

        if (!IsAttack)
        {
            IsAttack = true;

            target.gameObject.GetComponent<EnemyMovement>().Takendamage(this.gameObject, EnemyStatus.AttackValue);
            

            await WaitTime(AttackInterval);
        }


    }

    async public override void SpecialAttack(Collider target)
    {

        if (!IsSpAttack)
        {
            IsSpAttack = true;



            target.gameObject.GetComponent<EnemyMovement>().Takendamage(this.gameObject, EnemyStatus.AttackValue * 2);



            await WaitTime(SpAttackInterval);
        }


    }

  

    
}
