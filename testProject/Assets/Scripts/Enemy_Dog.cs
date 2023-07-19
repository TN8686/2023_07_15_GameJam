using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Dog : MonoBehaviour
{
    private Rigidbody2D rigidBody2D_;

    private bool isAttack_ = false;

    // ������_.
    [SerializeField]
    private float base_PosX_;

    // ������.
    [SerializeField]
    private float move_width_;

    [SerializeField]
    private bool isLeft_ = true;

    // �W�����v��.
    [SerializeField]
    private Vector2 jumpForce_ = new Vector2(10f, 5f);

    // �ړ����x.
    [SerializeField]
    private float MoveSpeed_ = 5f;

    // �������x���.
    [SerializeField]
    private float FallMaxSpeed_ = 20f;

    // �ڒn�֘A.
    [SerializeField]
    private bool isGround_ = false;

    [SerializeField]
    private bool isGround_Front_ = false;
    
    [SerializeField]
    private bool isGround_FrontDown_ = false;


    [SerializeField]
    private GroundCheck groundCheck_Down_;
    [SerializeField]
    private GroundCheck groundCheck_Front_;
    [SerializeField]
    private GroundCheck groundCheck_DownFront_;


    // �U������.
    [SerializeField]
    private Enemy_Attack AttackCheck_;

    // ���E.
    [SerializeField]
    private bool isRecognition_ = false;
    [SerializeField]
    private GroundCheck sightCheck_;

    [SerializeField]
    private float attackWait_ = 0;

    [SerializeField]
    private float attackWaitInitTime_ = 1;

    [SerializeField]
    private bool isDeath_;

    private Animator animator_;

    private bool isAttackJump_ = false;

    [SerializeField]
    private float flipWait_ = 0;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D_ = GetComponent<Rigidbody2D>();
        base_PosX_ = transform.position.x;

        animator_ = GetComponent<Animator>();

        attackWait_ = attackWaitInitTime_;
    }

    private void FixedUpdate()
    {

        Vector2 v = rigidBody2D_.velocity;

        var s = transform.localScale;

        if (isDeath_ && isGround_)
        {
            v = Vector2.zero;
        }
        else
        {
            if (isAttack_)
            {
                if (!isAttackJump_)
                {
                    v.x = 0;
                }
            }
            else
            {
                if (isLeft_)
                {
                    s.x = 1;
                    v.x = -MoveSpeed_;
                }
                else
                {
                    s.x = -1;
                    v.x = MoveSpeed_;
                }

            }
        }

        if (v.y < -FallMaxSpeed_)
        {
            v.y = -FallMaxSpeed_;
        }

        transform.localScale = s;

        rigidBody2D_.velocity = v;

    }

    // Update is called once per frame
    void Update()
    {
        if (flipWait_ > 0)
        {
            flipWait_ -= Time.deltaTime;
        }

        // �e�픻��`�F�b�N.
        isGround_ = groundCheck_Down_.IsGround();
        isGround_Front_ = groundCheck_Front_.IsGround();
        isGround_FrontDown_ = groundCheck_DownFront_.IsGround();

        if (isDeath_ && isGround_)
        {
            GetComponent<Rigidbody2D>().simulated = false;
            animator_.SetBool("Death", true);
            return;
        }


        // ���E�`�F�b�N.
        if (!isAttack_) {
            isRecognition_ = sightCheck_.IsGround();
        }

        if (isRecognition_)
        {
            isRecognition_ = false;
            isAttack_ = true;
            attackWait_ = attackWaitInitTime_;
        }

        // �U�������ۂ�.
        if (isAttack_)
        {


            // �U�����q�b�g������.
            if (attackWait_ <= -1f && AttackCheck_.IsGroundEnter())
            {
                isDeath_ = true;
            }

            // ���n�ŏI��.
            if (attackWait_ <= -1.2f && isGround_)
            {
                isAttackJump_ = false;
                transform.GetChild(3).gameObject.SetActive(false);

                base_PosX_ = transform.position.x;

                if(attackWait_ <= -2f)
                {
                    isAttack_ = false;
                }
            }

            attackWait_ -= Time.deltaTime;

            if (isGround_ && attackWait_ <= 0 && attackWait_ >= -0.2f)
            {
                var jump = jumpForce_;
                // �W�����v.
                if (isLeft_)
                {
                    jump.x *= -1;
                }
                GetComponent<Rigidbody2D>().AddForce(jump, ForceMode2D.Impulse);
                GetComponent<AudioSource>().Play();
                attackWait_ = -1f;
                isGround_ = false;
                isAttackJump_ = true;
                transform.GetChild(3).gameObject.SetActive(true);
                
            }


            // �A�j���[�V����.
            if (isGround_)
            {
                animator_.SetTrigger("Wait");
            }
            else
            {
                animator_.SetTrigger("Attack");
            }


        }
        else
        {
            var pos = transform.position;

            // �ǂƊR�ō��E���].
            if (flipWait_ <= 0)
            {
                if ((isGround_Front_ || !isGround_FrontDown_))
                {
                    // ���]�����炻�̕������̊�_���Y����.
                    if (isLeft_)
                    {
                        base_PosX_ = pos.x + (move_width_ * 0.5f + 0.25f);
                    }
                    else
                    {
                        base_PosX_ = pos.x - (move_width_ * 0.5f + 0.25f);
                    }
                    isLeft_ = !isLeft_;
                    flipWait_ = 0.2f;
                }
                else
                {
                    if (pos.x <= base_PosX_ - move_width_ * 0.5f)
                    {
                        isLeft_ = false;
                        flipWait_ = 0.2f;
                    }
                    else if (pos.x >= base_PosX_ + move_width_ * 0.5f)
                    {
                        isLeft_ = true;
                        flipWait_ = 0.2f;
                    }
                }
            }

            animator_.SetTrigger("Walk");
        }

    }
}
