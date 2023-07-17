using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Dog : MonoBehaviour
{
    private Rigidbody2D rbody2D_;

    private bool isAttack_ = false;

    // 往復基準点.
    [SerializeField]
    private float base_PosX_;

    // 往復幅.
    [SerializeField]
    private float move_width_;

    [SerializeField]
    private bool isLeft_ = true;

    // ジャンプ力.
    [SerializeField]
    private Vector2 jumpForce_ = new Vector2(10f, 5f);

    // 移動速度.
    [SerializeField]
    private float MoveSpeed_ = 5f;

    // 落下速度上限.
    [SerializeField]
    private float FallMaxSpeed_ = 20f;

    // 接地関連.
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


    // 攻撃判定.
    [SerializeField]
    private bool isAttackHit_ = false;
    [SerializeField]
    private Enemy_Attack AttackCheck_;

    // 視界.
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

        // 各種判定チェック.
        isGround_ = groundCheck_Down_.IsGround();
        isGround_Front_ = groundCheck_Front_.IsGround();
        isGround_FrontDown_ = groundCheck_DownFront_.IsGround();

        // 視界チェック.
        if (!isAttack_) {
            isRecognition_ = sightCheck_.IsGround();
        }

        if (isRecognition_)
        {
            isRecognition_ = false;
            isAttack_ = true;
            attackWait_ = attackWaitInitTime_;
        }

        // 攻撃中か否か.
        if (isAttack_)
        {
            // 攻撃がヒットしたら.
            if (attackWait_ <= -1f && AttackCheck_.IsGroundEnter())
            {
                isDeath_ = true;
            }

            // 着地で終了.
            if (attackWait_ <= -1.5f && isGround_)
            {
                isAttack_ = false;
                base_PosX_ = transform.position.x;
            }

            attackWait_ -= Time.deltaTime;
            if (attackWait_ <= 0 && attackWait_ > -1f)
            {
                var v = jumpForce_;
                // ジャンプ.
                if (isLeft_)
                {
                    v.x *= -1;
                }
                GetComponent<Rigidbody2D>().AddForce(v, ForceMode2D.Impulse);
                GetComponent<AudioSource>().Play();
                attackWait_ = -1f;
            }


            // アニメーション.
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
            // 左右反転.
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
