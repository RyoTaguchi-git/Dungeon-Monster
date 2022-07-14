using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestMonster : EnemyMovement
{
    bool IsChest;
    [SerializeField] private Canvas UI;
    // Start is called before the first frame update
  protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
   protected override void Update()
    {
        base.Update();
    }

  public override void Observer(Collider target) 
    {
        if (!IsChest) {
            Vector3 direction = Player.transform.position - transform.position;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit raycastHit;

            if (Physics.Raycast(ray, out raycastHit)) //オブジェクトに隠れていないか
            {

                if (raycastHit.collider.transform == Player.transform && angle < NoticeAngle)
                {

                    IsNotice = true;

                }
            }
            if (Target == null) //targetがセットされていないならtargetをセット
            {
                Target = target.gameObject;

            }
        }
    }

    async public override void Takendamage(GameObject other, int AttackValue)//被ダメージの処理
    {
        if (!die && !isInvincible)
        {
            animator.SetTrigger("taken damage");
            UI.enabled = true;
            OffAttackCollider();
            EnemyStatus.HP -= AttackValue;
            Target = other;
            HPSlider.value = (float)EnemyStatus.HP / (float)MaxHP;
            if (EnemyStatus.HP <= 0)
            {
                nav.isStopped = true;
                animator.SetTrigger("Die");
                die = true;
            }
            LookRotate(turnSpeed);
            IsNotice = true;
            IsChest = false; //チェストモード解除
            attackcol.enabled = false;
            await InvincibleTime(400); //無敵時間終了までダメージを受けない
        }

    }
    protected override void StateIdle()
    {

        if (IsNotice && animationstate.IsTag("Idle")  && !IsChest) //プレイヤー発見状態
        {
            LookRotate(turnSpeed);
            if (!IsSelect && !Rigidity)
            {
                SelectAttack(); //行動が決まっていないなら行動を決める

            }
        }
    }

    protected override GameObject Getbody()         //bodyを取得
    {
        return transform.Find("Root/Body/Head").gameObject;
    }



    void ChestMode()  //IdleChestアニメーションから呼び出す
    {
        IsChest = true;
        UI.enabled = false;
        nav.ResetPath();
    }


}
