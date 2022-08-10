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


        Player = GameObject.FindGameObjectWithTag("Player"); //プレイヤーゲームオブジェクトを取得
        PlayerScript = Player.GetComponent<PlayerController>();  //プレイヤースクリプトを取得
        nav = GetComponent<NavMeshAgent>();
        positionVector = Player.transform.position - transform.position;
        angle = Vector3.Angle(transform.forward, positionVector); //敵から見たプレイヤーの角度

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
            int TargetHP = Target.GetComponent<EnemyMovement>().GetHPValue();
            if (TargetHP <= 0)
            {
                ResetTarget();
            }
        }
        else
        {
            
            positionVector = Player.transform.position - transform.position;
            distance = positionVector.sqrMagnitude;//プレイヤーとの距離
            angle = Vector3.Angle(transform.forward, positionVector); //敵から見たプレイヤーの角度
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

        if (animationstate.IsTag("Idle") && !Rigidity) //プレイヤー発見状態,硬直がないか
        {
            LookRotate(turnSpeed);
            if (!IsSelect)
            {
                SelectAttack(); //行動が決まっていないなら行動を決める

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
