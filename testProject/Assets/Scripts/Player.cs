using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
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


    // 傷み度合.
    [SerializeField]
    float rot_;
    public float getRotRatio()
    {
        return rot_ / rot_max;
    }

    // 一秒あたりの減少量.
    [SerializeField]
    float rot_decreasePerSecond = 0.5f;

    // それぞれのライン.
    [SerializeField]
    float rot_max = 100f;
    [SerializeField]
    float rotLine_01 = 60f;
    [SerializeField]
    float rotLine_02 = 30f;

    [SerializeField]
    private AnimatorController[] animatorControllerList_;

    private Animator animator_;

    // Start is called before the first frame update
    void Start()
    {
        rbody2D_ = GetComponent<Rigidbody2D>();
        groundCheck_ = transform.GetChild(0).GetComponent<GroundCheck>();

        animator_ = GetComponent<Animator>();
        fallRespawnPoint = transform.position;

        rot_ = rot_max;

        animator_.runtimeAnimatorController = animatorControllerList_[0];
    }

    private void FixedUpdate()
    {
        Vector2 v = rbody2D_.velocity;
        //地面当たり判定
        //接地判定を得る

        if (isDeath())
        {
            v.x = 0;
        }
        else
        {
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
        // とりあえずのシーン遷移.
        sceneMove();

        Vector2 v = rbody2D_.velocity;          // 加速度を取得.
        isGround_ = groundCheck_.IsGround();    // 接地判定を取得.

        var pos = transform.position;

        // 復活ポイント設定.
        if (isGround_)
        {
            pos.y = 10;
            fallRespawnPoint = pos;
        }

        //ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && isGround_ && !isDeath())
        {
            // 上下速度を初期化.
            v.y = 0;
            rbody2D_.velocity = v;

            // ジャンプ.
            rbody2D_.AddForce(new Vector2(0, jumpForce_), ForceMode2D.Impulse);
        }

        // アニメーション管理.
        setAnimatorController();    // アニメーターを選択.

        if (isGround_)  // 地上.
        {
            if (v.x >= -0.1 && v.x <= 0.1)
            {
                animator_.SetTrigger("Wait");
            }
            else
            {
                animator_.SetTrigger("Walk");
            }
        }
        else             // 空中.
        {
            if (v.y > 0)
            {
               animator_.SetTrigger("JumpUp");
            }
            else
            {
                animator_.SetTrigger("JumpDown");
            }
        }

        // 座標が一定以下だったら復帰ポイントの上から復帰.
        pos = transform.position;
        if (pos.y <= -10f)
        {
            pos = fallRespawnPoint;
            transform.position = pos;
        }

        // ゲージ減少.
        rot_ -= rot_decreasePerSecond * Time.deltaTime;
        if(isDeath())
        {
            rot_ = 0;
            animator_.SetBool("Death", true);
        }

    }

    // 現在の傷み度合に応じてアニメーターを選択します.
    private void setAnimatorController()
    {
        if (rot_ > rotLine_01)
        {
            animator_.runtimeAnimatorController = animatorControllerList_[0];
        }
        else if(rot_ > rotLine_02)
        {
            animator_.runtimeAnimatorController = animatorControllerList_[1];
        }
        else
        {
            animator_.runtimeAnimatorController = animatorControllerList_[2];
        }
    }

    public bool isDeath()
    {
        return (rot_ <= 0);
    }

    public void sceneMove()
    {
        // Tでタイトルシーン.
        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene("TitleScene");
        }
        //Escが押された時
        if (Input.GetKey(KeyCode.Escape))
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
        }
    }
}
