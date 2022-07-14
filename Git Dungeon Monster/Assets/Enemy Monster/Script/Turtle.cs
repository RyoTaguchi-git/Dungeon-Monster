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
        AnimationTime = animatorclip[0].clip.length * animationstate.normalizedTime; //アニメーションの時間の取得
        if (Target != null)
        {
            positionVector = Target.transform.position - transform.position;
            distance = positionVector.sqrMagnitude;
            angle = Vector3.Angle(transform.forward, positionVector); //Targetの角度
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
        IsDefence = (animationstate.IsName("Defend") && AnimationTime > 0.25) ? true : false; //一定時間後に防御判定出現
    }

  async  public override void Takendamage(GameObject other, int AttackValue) //被ダメージの処理
    {
        if (!die && !isInvincible)
        {
            if (!IsDefence) {
                Debug.Log("攻撃された");
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
                Debug.Log("防御した");
                animator.SetTrigger("Defence damage");
                EnemyStatus.HP -= (int)(AttackValue * 0.1f);
                Countercount++;
                if (Countercount > 3)   //防御時に三回ダメージで反撃
                {
                    animator.SetBool("SpecialAttack", true);
                    LookRotate(100);
                    attackcol.enabled = true;
                }
              
            }
            OffAttackCollider();
            HPSlider.value = (float)EnemyStatus.HP / (float)MaxHP;
            await InvincibleTime(400); //無敵時間終了までダメージを受けない
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
        if (IsNotice && animationstate.IsTag("Idle")) //プレイヤー発見状態ならプレイヤーを追従
        {
            LookRotate(turnSpeed);
            Countercount = 0;
            animator.ResetTrigger("Defence damage");
            if (!IsDefence && !IsSelect &&  !Rigidity) //防御中は攻撃しない
            {
                SelectAttack();  //行動が決まっていないなら行動を決める
            }
        }
    }
}
