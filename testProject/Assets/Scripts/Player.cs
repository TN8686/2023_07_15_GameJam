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

    // �W�����v��.
    [SerializeField]
    private float jumpForce_ = 10f;

    // �ړ����x.
    [SerializeField]
    private float MoveSpeed_ = 5f;

    // �������x���.
    [SerializeField]
    private float FallMaxSpeed_ = 20f;

    // ���������n�_.
    [SerializeField]
    private Vector3 fallRespawnPoint;

    // �ڒn�֘A.
    [SerializeField]
    private bool isGround_ = false;
    private GroundCheck groundCheck_;


    // ���ݓx��.
    [SerializeField]
    float rot_;
    public float getRotRatio()
    {
        return rot_ / rot_max;
    }

    // ��b������̌�����.
    [SerializeField]
    float rot_decreasePerSecond = 0.5f;

    // ���ꂼ��̃��C��.
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
        //�n�ʓ����蔻��
        //�ڒn����𓾂�

        if (isDeath())
        {
            v.x = 0;
        }
        else
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))//�E��󂨂�����
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
        // �Ƃ肠�����̃V�[���J��.
        sceneMove();

        Vector2 v = rbody2D_.velocity;          // �����x���擾.
        isGround_ = groundCheck_.IsGround();    // �ڒn������擾.

        var pos = transform.position;

        // �����|�C���g�ݒ�.
        if (isGround_)
        {
            pos.y = 10;
            fallRespawnPoint = pos;
        }

        //�W�����v
        if (Input.GetKeyDown(KeyCode.Space) && isGround_ && !isDeath())
        {
            // �㉺���x��������.
            v.y = 0;
            rbody2D_.velocity = v;

            // �W�����v.
            rbody2D_.AddForce(new Vector2(0, jumpForce_), ForceMode2D.Impulse);
        }

        // �A�j���[�V�����Ǘ�.
        setAnimatorController();    // �A�j���[�^�[��I��.

        if (isGround_)  // �n��.
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
        else             // ��.
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

        // ���W�����ȉ��������畜�A�|�C���g�̏ォ�畜�A.
        pos = transform.position;
        if (pos.y <= -10f)
        {
            pos = fallRespawnPoint;
            transform.position = pos;
        }

        // �Q�[�W����.
        rot_ -= rot_decreasePerSecond * Time.deltaTime;
        if(isDeath())
        {
            rot_ = 0;
            animator_.SetBool("Death", true);
        }

    }

    // ���݂̏��ݓx���ɉ����ăA�j���[�^�[��I�����܂�.
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
        // T�Ń^�C�g���V�[��.
        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene("TitleScene");
        }
        //Esc�������ꂽ��
        if (Input.GetKey(KeyCode.Escape))
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
    Application.Quit();//�Q�[���v���C�I��
#endif
        }
    }
}
