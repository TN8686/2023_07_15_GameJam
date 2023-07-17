using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Dog : MonoBehaviour
{
    private Rigidbody2D rbody2D_;

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
    private bool isAttackHit_ = false;
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

    // Start is called before the first frame update
    void Start()
    {
        rbody2D_ = GetComponent<Rigidbody2D>();
        base_PosX_ = transform.position.x;

        animator_ = GetComponent<Animator>();

        attackWait_ = attackWaitInitTime_;
    }

    private void FixedUpdate()
    {

        Vector2 v = rbody2D_.velocity;

        var s = transform.localScale;

        if (isDeath_)
        {
            v = Vector2.zero;
        }
        else
        {
            if (isAttack_)
            {

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


        transform.localScale = s;

        rbody2D_.velocity = v;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDeath_)
        {
            animator_.SetBool("Death", true);
            return;
        }

        // �e�픻��`�F�b�N.
        isGround_ = groundCheck_Down_.IsGround();
        isGround_Front_ = groundCheck_Front_.IsGround();
        isGround_FrontDown_ = groundCheck_DownFront_.IsGround();

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
            if (attackWait_ <= -1.5f && isGround_)
            {
                isAttack_ = false;
                base_PosX_ = transform.position.x;
            }

            attackWait_ -= Time.deltaTime;
            if (attackWait_ <= 0 && attackWait_ > -1f)
            {
                var v = jumpForce_;
                // �W�����v.
                if (isLeft_)
                {
                    v.x *= -1;
                }
                GetComponent<Rigidbody2D>().AddForce(v, ForceMode2D.Impulse);
                GetComponent<AudioSource>().Play();
                attackWait_ = -1f;
            }


            // �A�j���[�V����.
            if (isGround_)
            {
                if (attackWait_ > 0)
                {
                    animator_.SetTrigger("Wait");
                }
            }
            else
            {
                animator_.SetTrigger("Attack");
            }


        }
        else
        {
            isAttackHit_ = false;
            // ���E���].
            if (isGround_Front_ || !isGround_FrontDown_)
            {
                isLeft_ = !isLeft_;
            }

            var pos = transform.position;

            if (pos.x <= base_PosX_ - move_width_ * 0.5)
            {
                isLeft_ = false;
            }
            else if (pos.x >= base_PosX_ + move_width_ * 0.5)
            {
                isLeft_ = true;
            }
            animator_.SetTrigger("Walk");
        }

    }
}
