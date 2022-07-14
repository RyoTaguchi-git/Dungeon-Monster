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

            if (Physics.Raycast(ray, out raycastHit)) //�I�u�W�F�N�g�ɉB��Ă��Ȃ���
            {

                if (raycastHit.collider.transform == Player.transform && angle < NoticeAngle)
                {

                    IsNotice = true;

                }
            }
            if (Target == null) //target���Z�b�g����Ă��Ȃ��Ȃ�target���Z�b�g
            {
                Target = target.gameObject;

            }
        }
    }

    async public override void Takendamage(GameObject other, int AttackValue)//��_���[�W�̏���
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
            IsChest = false; //�`�F�X�g���[�h����
            attackcol.enabled = false;
            await InvincibleTime(400); //���G���ԏI���܂Ń_���[�W���󂯂Ȃ�
        }

    }
    protected override void StateIdle()
    {

        if (IsNotice && animationstate.IsTag("Idle")  && !IsChest) //�v���C���[�������
        {
            LookRotate(turnSpeed);
            if (!IsSelect && !Rigidity)
            {
                SelectAttack(); //�s�������܂��Ă��Ȃ��Ȃ�s�������߂�

            }
        }
    }

    protected override GameObject Getbody()         //body���擾
    {
        return transform.Find("Root/Body/Head").gameObject;
    }



    void ChestMode()  //IdleChest�A�j���[�V��������Ăяo��
    {
        IsChest = true;
        UI.enabled = false;
        nav.ResetPath();
    }


}
