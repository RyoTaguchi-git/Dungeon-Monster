using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : EnemyMovement
{

    bool IsDefence;
    int Countercount;
    // Start is called before the first frame update
  protected override void Start()
    {
        base.Start();
        
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
            if (Target.CompareTag("Player"))
            {
                TargetHP = PlayerScript.GetHPValue();
            }
            else
            {
                 TargetHP = Target.GetComponent<EnemyMovement>().GetHPValue();
            }

            if (TargetHP <= 0)
            {
                Target = null;
            }
        }
        animator.SetBool("IsNotice", IsNotice);


        if (!die)
        {
            StateIdle();
            Defence();
            JudgeAttackHit();
         
        }

        else if(!Rigidity)
        {
            Die();
        }
       
    }


    public void Defence()
    {
        IsDefence = (animationstate.IsName("Defend") && AnimationTime > 0.25) ? true : false; //��莞�Ԍ�ɖh�䔻��o��
    }

  async  public override void Takendamage(GameObject other, int AttackValue) //��_���[�W�̏���
    {
        if (!die && !isInvincible)
        {
            if (!IsDefence) {
                Debug.Log("�U�����ꂽ");
                animator.SetTrigger("taken damage");
                EnemyStatus.HP -= AttackValue;
                Target = other;
                animator.SetTrigger("Defence");
                LookRotate(turnSpeed);
                IsNotice = true;
                attackcol.enabled = false;
                Navstart();
            }
            else
            {
                Debug.Log("�h�䂵��");
                animator.SetTrigger("Defence damage");
                EnemyStatus.HP -= (int)(AttackValue * 0.1f);
                Countercount++;
                if (Countercount > 3)   //�h�䎞�ɎO��_���[�W�Ŕ���
                {
                    animator.SetBool("SpecialAttack", true);
                    LookRotate(100);
                    attackcol.enabled = true;
                }
              
            }
            OffAttackCollider();
            HPSlider.value = (float)EnemyStatus.HP / (float)MaxHP;
            await InvincibleTime(400); //���G���ԏI���܂Ń_���[�W���󂯂Ȃ�
            if (EnemyStatus.HP <= 0)
            {
                nav.isStopped = true;
                animator.SetTrigger("Die");
                die = true;
            }
          
           
     
            
        }

    }

    protected override void StateIdle()
    {
        if (IsNotice && animationstate.IsTag("Idle")) //�v���C���[������ԂȂ�v���C���[��Ǐ]
        {
            LookRotate(turnSpeed);
            Countercount = 0;
            animator.ResetTrigger("Defence damage");
            if (!IsDefence && !IsSelect &&  !Rigidity) //�h�䒆�͍U�����Ȃ�
            {
                SelectAttack();  //�s�������܂��Ă��Ȃ��Ȃ�s�������߂�
            }
        }
    }
}
