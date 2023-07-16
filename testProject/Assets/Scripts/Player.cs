using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    private Rigidbody2D rbody2D_;

    [SerializeField]
    private float jumpForce_ = 10f;
    [SerializeField]
    private float MoveSpeed_ = 5f;
    [SerializeField]
    private float FallMaxSpeed_ = 20f;

    //プライベート変数
    private bool isGround_ = false;
    // Start is called before the first frame update
    void Start()
    {
        rbody2D_ = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 v = rbody2D_.velocity;
        //地面当たり判定
        //接地判定を得る

        isGround_ = true;   // TODO.

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

        //ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && isGround_)
        {
            // 上下速度を初期化.
            v.y = 0;
            rbody2D_.velocity = v;

            // ジャンプ.
            rbody2D_.AddForce(new Vector2(0, jumpForce_), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            Vector2 v = rbody2D_.velocity;
            if(v.y < 0)
            {
                var p = transform.position;
                
                transform.position = p;
            }
        }
    }
}
