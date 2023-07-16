using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    private Rigidbody2D rbody2D_;

    // ジャンプ力.
    [SerializeField]
    private float jumpForce_ = 10f;

    // 移動速度.
    [SerializeField]
    private float MoveSpeed_ = 5f;

    // 落下速度上限.
    [SerializeField]
    private float FallMaxSpeed_ = 20f;

    // 落下復活地点.
    [SerializeField]
    private Vector3 fallRespawnPoint;

    // 接地関連.
    [SerializeField]
    private bool isGround_ = false;
    private GroundCheck groundCheck_;


    [SerializeField]
    float rot;
    [SerializeField]
    float rot_max = 100f;
    [SerializeField]
    float rot_decreasePerSecond = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rbody2D_ = GetComponent<Rigidbody2D>();
        groundCheck_ = transform.GetChild(0).GetComponent<GroundCheck>();
        fallRespawnPoint = transform.position;

        rot = rot_max;
    }

    private void FixedUpdate()
    {
        Vector2 v = rbody2D_.velocity;
        //地面当たり判定
        //接地判定を得る

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))//右矢印おしたら
        {
            v.x = MoveSpeed_;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {

            v.x = -MoveSpeed_;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            v.x = 0;
        }

        if (v.y < -FallMaxSpeed_)
        {
            v.y = -FallMaxSpeed_;
        }

        rbody2D_.velocity = v;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 v = rbody2D_.velocity;
        isGround_ = groundCheck_.IsGround();

        var pos = transform.position;

        // 復活ポイント設定.
        if (isGround_)
        {
            pos.y = 10;
            fallRespawnPoint = pos;
        }

        //ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && isGround_)
        {
            // 上下速度を初期化.
            v.y = 0;
            rbody2D_.velocity = v;

            // ジャンプ.
            rbody2D_.AddForce(new Vector2(0, jumpForce_), ForceMode2D.Impulse);
        }

        // 座標が一定以下だったら復帰ポイントの上から復帰.
        pos = transform.position;
        if (pos.y <= -10f)
        {
            pos = fallRespawnPoint;
            transform.position = pos;
        }

        // ゲージ減少.
        rot -= rot_decreasePerSecond * Time.deltaTime;
        if(rot <= 0)
        {
            rot = 0;    // TODO　死亡.
        }

    }

}
