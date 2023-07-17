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
    private float jumpForce_ = 10f;

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
    private GroundCheck AttackCheck_;

    // 視界.
    [SerializeField]
    private bool isRecognition_ = false;
    [SerializeField]
    private GroundCheck sightCheck_;
    // Start is called before the first frame update
    void Start()
    {
        rbody2D_ = GetComponent<Rigidbody2D>();
        base_PosX_ = transform.position.x;
    }

    private void FixedUpdate()
    {

        Vector2 v = rbody2D_.velocity;

        var s = transform.localScale;
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
        transform.localScale = s;

        rbody2D_.velocity = v;
    }

    // Update is called once per frame
    void Update()
    {
        // 各種判定チェック.
        isGround_ = groundCheck_Down_.IsGround();
        isGround_Front_ = groundCheck_Front_.IsGround();
        isGround_FrontDown_ = groundCheck_DownFront_.IsGround();
        isRecognition_ = sightCheck_.IsGround();

        if (isAttack_) {
            isAttackHit_ = AttackCheck_.IsGround();
        }
        else
        {
            isAttackHit_ = false;
        }



        // 攻撃中か否か.
        if (isAttack_)
        {

        }
        else
        {
            // 左右反転.
            if (isGround_Front_ || !isGround_FrontDown_)
            {
                isLeft_ = !isLeft_;
            }
        }
    }
}
