using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;



public class PlayerController : MonoBehaviour
{
    [System.Serializable] private class status
    {
        public int HP = 100;
        public int AttackValue = 2;
        int Lebel;
    }

   [SerializeField] private status PlayerStatus;
    int MaxHP;
    public Slider HPSlider;
    Animator animator;
    AnimatorStateInfo animationstate;
    AnimatorClipInfo[] animatorclip;
    public float turnSpeed = 20; //�L�����N�^�[��1�b������ɉ�]����p�x�i���W�A��)
    float AttackturnSpeed = 15;
    public float WalkSpeed = 1;
    float RunningSpeed;
    bool isAttack;
    bool isInvincible;
    private bool Move = true;
    bool die;
    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    [SerializeField] private Collider WeaponCollider;
    [SerializeField] private GameObject GameOverButton;
    int InputCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        HPSlider.value = 1;
        MaxHP = PlayerStatus.HP;
    }

    // Update is called once per frame
    void Update()
    {
        if (Move) {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            animationstate = animator.GetCurrentAnimatorStateInfo(0);
            animatorclip = animator.GetCurrentAnimatorClipInfo(0);
            m_Movement.Set(horizontal, 0f, vertical);
            m_Movement.Normalize();
            bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
            bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
            bool isWalking = hasHorizontalInput || hasVerticalInput;
            bool isRunninng = Mathf.Abs(horizontal) > 0.5 || Mathf.Abs(vertical) > 0.5;
            isAttack = animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
            bool damaged = animationstate.IsName("Taken damage");
            animator.SetBool("IsWalking", isWalking);
            animator.SetBool("IsRunning", isRunninng);

            if (!die)
            {
                if (animationstate.IsName("Idle") && !animator.IsInTransition(0))
                {
                    InputCount = 0;
                    OffColliderAttack();
                }

                //��]�����邽�߂̃x�N�g���̍쐬
                if (isWalking)
                {
                    RunningSpeed = 1.0f;

                }

                if (isRunninng)
                {
                    RunningSpeed = 3.0f;
                }

                if (!damaged)  //�_���[�W���󂯂ĂȂ��Ƃ��ɏ��������s
                {


                    if (Input.GetKeyDown("space") && InputCount < 2) //�U������
                    {
                        Attack();
                        InputCount++;

                    }

                    AttackRotateTime(0.5f);

                    if (!isAttack)
                    {
                        LookRotate(turnSpeed);
                        PlayerMove();
                        transform.rotation = m_Rotation;

                    }

                }
            }
            else
            {
                Die();
            }
        }
    }

    void PlayerMove()
    {
        
            m_Rigidbody.MovePosition
          (m_Rigidbody.position + WalkSpeed * RunningSpeed * m_Movement * Time.deltaTime);
        
    }

    void LookRotate(float turnSpeed)  //��]�v�Z
    {
        Vector3 desiredForward =
           Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);

        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    void AttackRotateTime(float time) //�U�����̉�]��t����
    {
        if (animationstate.IsName("Attack2") && animationstate.normalizedTime < time)
        {
            LookRotate(AttackturnSpeed);
            m_Rigidbody.MoveRotation(m_Rotation);
        }
    }

    void Attack()
    {
        animator.SetTrigger("attack");
        m_Rigidbody.velocity = Vector3.zero;
    }

  async  public void Takendamage(GameObject other, int AttackValue) //��_���[�W�̏���
    {
        if (!die && !isInvincible) {
            animator.SetTrigger("taken damage");
            PlayerStatus.HP -= AttackValue;
            if (PlayerStatus.HP <= 0)
            {
                animator.SetTrigger("Die");
                die = true;
            }
            HPSlider.value = (float)PlayerStatus.HP / (float)MaxHP;
            await InvincibleTime(); //���G���ԏI���܂Ń_���[�W���󂯂Ȃ�
           
        }
    
    }

    async UniTask InvincibleTime()  //���G����
    {
        if (isInvincible)
        {
            return;
        }

        isInvincible = true;

        await UniTask.Delay(1000);
        isInvincible = false;
    }

   async void Die()
    {
        if (animationstate.IsName("Die") && animationstate.normalizedTime >= 1)
        {

            await InvincibleTime();

            GameOverButton.SetActive(true);
            Destroy(this);
        }
    }

    public int GetAttackValue()
    {
        return PlayerStatus.AttackValue;
    }

        //����̔����L��or�����؂�ւ���
        public void OffColliderAttack()
    {
        WeaponCollider.enabled = false;
    }
    public void OnColliderAttack()
    {
        WeaponCollider.enabled = true;
    }

    public void SwitchingMove()
    {
        Move = !Move;
    }
    public int GetHPValue()
    {
        return PlayerStatus.HP;
    }

}
