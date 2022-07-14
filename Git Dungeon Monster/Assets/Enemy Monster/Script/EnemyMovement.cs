using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class EnemyMovement : MonoBehaviour
{
    [System.Serializable] public class status
    {
        public int HP = 10;
        public int AttackValue = 2;
        int Lebel;
    }
    protected int MaxHP;
    [SerializeField] protected status EnemyStatus;
    public Slider HPSlider;
    protected Animator animator;
    protected Collider attackcol;
    
    protected IsAttackRange RangeScript;

    [SerializeField] protected AnimationClip AttackMotion; //�A�^�b�N���[�V�����̎w��
    [SerializeField] protected AnimationClip SpecialAttackMotion; //�X�y�V�����A�^�b�N���[�V�����̎w��
    protected AnimatorStateInfo animationstate;
    protected AnimatorClipInfo[] animatorclip;
    protected float AnimationTime;
    public float AttackTime;  //�U���^�C�~���O
    public float SpecialAttackTime;
    protected bool IsAttack;
    protected bool IsSpAttack;
    protected bool IsNotice;
    protected bool die;
    protected bool IsSelect;
    protected bool isInvincible;
    protected GameObject Player;
    protected GameObject Target;
    protected int TargetHP;
    protected PlayerController PlayerScript;
    protected NavMeshAgent nav;
    protected Vector3 positionVector;
    protected float distance;
    protected float angle;
    [SerializeField] protected float turnSpeed = 4;
    [SerializeField] protected float AttackRange = 2.0f;
    [SerializeField] protected float SpAttackRange = 6.0f;
    [SerializeField] protected float NoticeAngle = 100.0f;
    [SerializeField] protected int AttackInterval = 2000;
    [SerializeField] protected int SpAttackInterval = 2500;
    protected bool Rigidity; //�d�������邩
                           // Start is called before the first frame update
    protected virtual void Start()
    {
        MaxHP = EnemyStatus.HP;
        HPSlider.value = 1;
        animator = GetComponent<Animator>();
        GameObject body = Getbody();          
        attackcol = body.GetComponent<Collider>();
        RangeScript = body.GetComponent<IsAttackRange>();
        
        attackcol.enabled = false;


        Player = GameObject.FindGameObjectWithTag("Player"); //�v���C���[�Q�[���I�u�W�F�N�g���擾
        PlayerScript = Player.GetComponent<PlayerController>();  //�v���C���[�X�N���v�g���擾
        nav = GetComponent<NavMeshAgent>();
        positionVector = Player.transform.position - transform.position;
        angle = Vector3.Angle(transform.forward, positionVector); //�G���猩���v���C���[�̊p�x
     
    }

    // Update is called once per frame
  protected virtual void Update()
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
       
        //  Debug.Log(IsNotice);

        if (!die) {

            StateIdle();
            JudgeAttackHit();
       
        }
        
        else if(!Rigidity)
        {
            Die();
        }
    }

    protected virtual void SelectAttack()
    {
        if (angle < 30)
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

    protected virtual void JudgeAttackHit()
    {
        if (SpecialAttackMotion != null)
        {

            if (animatorclip[0].clip == SpecialAttackMotion && AnimationTime > SpecialAttackTime) //�U�����������Ă��邩�m�F
            {
              

                animator.SetBool("SpecialAttack", false);
              
                IsSelect = false;
            }
            else
            {
                IsSpAttack = false;
            }
        }


        if (animatorclip[0].clip == AttackMotion && AnimationTime > AttackTime) //�U�����������Ă��邩�m�F
        {
            
           
            animator.SetBool("Attack", false);
    
            IsSelect = false;
        }
        else
        {
            IsAttack = false;
        }
    }

 
   async public virtual void Attack(Collider target)
    {

        if (!IsAttack)
        {
            IsAttack = true;

            if (target.gameObject.CompareTag("Player"))
            {
                PlayerScript.Takendamage(this.gameObject, EnemyStatus.AttackValue);
            }
            else if (target.gameObject.CompareTag("PlayerFriend")) 
            {
                target.gameObject.GetComponent<FriendMonster>().Takendamage(this.gameObject, EnemyStatus.AttackValue);
            }
            
            await WaitTime(AttackInterval);
        }
       

    }

   async public virtual void SpecialAttack(Collider target)
    {

        if (!IsSpAttack)
        {
            IsSpAttack = true;

            if (target.gameObject.CompareTag("Player"))
            {
                PlayerScript.Takendamage(this.gameObject, EnemyStatus.AttackValue);
            }
            else if (target.gameObject.CompareTag("PlayerFriend"))
            {
                target.gameObject.GetComponent<FriendMonster>().Takendamage(this.gameObject, EnemyStatus.AttackValue);
            }

            await WaitTime(SpAttackInterval);
        }


    }
    async protected UniTask WaitTime(int time)
    {
        if (Rigidity)
        {
            return;
        }
        Rigidity = true;

        await UniTask.Delay(time);
       
        Rigidity = false;
       
    }
    async protected UniTask InvincibleTime(int time)  //���G����
    {
        if (isInvincible)
        {
            return;
        }

        isInvincible = true;

        await UniTask.Delay(time);
        isInvincible = false;
    }

    void recover()
    {
        if (EnemyStatus.HP <= 0)
        {
            EnemyStatus.HP = MaxHP;
            HPSlider.value = (float)EnemyStatus.HP / (float)MaxHP;
            Debug.Log("��");
        }
    }

  async  protected void Die()
    {
       
        if (animationstate.IsName("Die") && animationstate.normalizedTime >= 1)
        {
            await WaitTime(1000);
            Destroy(this.gameObject);
        }
    }


  async public virtual void Takendamage(GameObject other,int AttackValue) //��_���[�W�̏���
    {
        if (!die && !isInvincible) {
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
            }
            LookRotate(turnSpeed);
            IsNotice = true;
            attackcol.enabled = false;
            await InvincibleTime(400); //���G���ԏI���܂Ń_���[�W���󂯂Ȃ�
        }
    }

   

    protected void FollowPlayer() //�v���C���[��Ǐ]
    {
        nav.destination = Player.transform.position;
        Target = Player;
        Navstart();
            
    }
    void FollowTarget()
    {
        if (Target != null) {
            nav.destination = Target.transform.position;
            Navstart();
        }

    }

    protected void LookRotate(float turnSpeed)  //��]�v�Z
    {
        Vector3 desiredForward =
           Vector3.RotateTowards(transform.forward, positionVector, turnSpeed * Time.deltaTime, 0f);

        transform.rotation = Quaternion.LookRotation(desiredForward);
    }

   

    protected virtual void StateIdle()
    {

        if (IsNotice && animationstate.IsTag("Idle")) //�v���C���[�������
        {
            LookRotate(turnSpeed);
            if (!IsSelect && !Rigidity && Target != null)
            {
                SelectAttack(); //�s�������܂��Ă��Ȃ��Ȃ�s�������߂�

            }
        }
    }

    public int GetHPValue()
    {
        return EnemyStatus.HP;
    }

    protected  void OffAttackCollider()  //AnimationEvent����Ăяo��
    {
      
        attackcol.enabled = false;
      
       
    }

    protected void OnAttackCollider()  //AnimationEvent����Ăяo��
    {
        attackcol.enabled = true;
    }

   
    public void Navstart()
    {

        nav.isStopped = false;


    }
    public virtual void Observer(Collider target)
    {
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

    public virtual void ResetTarget()
    {
        Target = null;
        IsNotice = false;
        nav.isStopped = true;
    }

    public string GetAttackstate()
    {
        if (animationstate.IsName("Attack")) return "Attack";
        else if (animationstate.IsName("SpecialAttack")) return "SpAttack";
        else return "nothing";
    }


    protected virtual GameObject Getbody()         //body���擾
    {
        return transform.Find("Body").gameObject;
    }
}
