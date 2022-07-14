using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beholder : EnemyMovement
{
    public LineRenderer LightningEffect;
    [Range(1, 100)]
    public float Rangedistance;
    // Start is called before the first frame update
protected  override  void Start()
    {
        base.Start();
        LightningEffect.enabled = false;

    }

    // Update is called once per frame
protected override void Update()
    {
        base.Update();
       
     //   Debug.Log(distance);
        
        
    
    }


    protected override void SelectAttack()
    {
        if (angle < 30)
        {
            if (AttackRange > distance)
            {
                animator.SetBool("Attack", true); //攻撃距離に入ったときにアタックへ遷移
                
                nav.isStopped = true;
                IsSelect = true;
            }
            else if (SpAttackRange > distance)
            {
                animator.SetBool("SpecialAttack", true); //攻撃距離に入ったときにアタックへ遷移
               
                nav.isStopped = true;
                IsSelect = true;
            }

            else if (Rangedistance > distance){
                animator.SetBool("LongRangeAttack", true);
                nav.isStopped = true;
                IsSelect = true;
            }
            else
            {
                FollowPlayer();
            }
        }
    }

    protected override void JudgeAttackHit()
    {
        base.JudgeAttackHit();
        Lightning();
    }

    void Lightning()
    {
        if (animationstate.IsName("Attack02ST") && animationstate.normalizedTime > 1 && !LightningEffect.enabled )
        {
            animator.SetTrigger("Lightning");
            LightningEffect.enabled = true;
           
            animator.SetBool("LongRangeAttack", false);
            IsSelect = false;
        }

        else if (animationstate.IsTag("Idle") && animationstate.normalizedTime > 0)
        {
           
            LightningEffect.enabled = false;
        }
    }

    async public override void Takendamage(GameObject other, int AttackValue)//被ダメージの処理
    {

        if (!die && !isInvincible)
        {
            animator.SetTrigger("taken damage");
            OffAttackCollider();
            EnemyStatus.HP -= AttackValue;
            Target = other;
           
            HPSlider.value = (float)EnemyStatus.HP / (float)MaxHP;
            if (EnemyStatus.HP <= 0)
            {
                nav.isStopped = true;
                animator.SetTrigger("Die");
                die = true;
                LightningEffect.enabled = false;
            }
            LookRotate(turnSpeed);
            IsNotice = true;
            attackcol.enabled = false;
            await InvincibleTime(400); //無敵時間終了までダメージを受けない
        }
    }

    protected override GameObject Getbody()         //bodyを取得
    {
        return transform.Find("Root/Body").gameObject;
    }


    public void LazerAttack()
    {
        if (LightningEffect.enabled)
        {
            PlayerScript.Takendamage(this.gameObject, EnemyStatus.AttackValue);
        }
    }
}
