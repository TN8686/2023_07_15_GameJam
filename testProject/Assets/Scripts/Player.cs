using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigidBody2D_;

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
    float rot_decreasePerSecond_ = 0.5f;

    public float Rot_decreasePerSecond
    {
        get { return rot_decreasePerSecond_; }
        set { rot_decreasePerSecond_ = value; }
    }

    [SerializeField]
    private float antisepticTime_ = 0;
    public float AntisepticTime {
        get { return antisepticTime_; }
        set { antisepticTime_ = value; }
    }

    [SerializeField]
    private Color antisepticColor_;

    private Color defaultColor_;

    // ���ꂼ��̃��C��.
    [SerializeField]
    private float rot_max = 100f;
    public float Rot_max
    {
        get { return rot_max; }
    }

    [SerializeField]
    private float rotLine_01 = 60f;
    public float RotLine_01
    {
        get { return rotLine_01; }
    }

    [SerializeField]
    private float rotLine_02 = 30f;
    public float RotLine_02
    {
        get { return rotLine_02; }
    }

    [SerializeField]
    private RuntimeAnimatorController[] animatorControllerList_;

    private Animator animator_;

    float restartTime_ = 4f;

    [SerializeField]
    bool unDead_ = false;
    public bool UnDead
    {
        get { return unDead_; }
        set { unDead_ = value; }
    }

    [SerializeField]
    bool isEvent_ = false;
    public bool IsEvent
    {
        get { return isEvent_; }
        set { isEvent_ = value; }
    }

    [SerializeField]
    private ImageColorLeap black_;

    private bool isChangeCurtain_ = false;

    [SerializeField]
    private AudioSource bgm_;

    [SerializeField]
    private GameObject[] blood_;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D_ = GetComponent<Rigidbody2D>();
        groundCheck_ = transform.GetChild(0).GetComponent<GroundCheck>();
        defaultColor_ = GetComponent<SpriteRenderer>().color;
        animator_ = GetComponent<Animator>();
        fallRespawnPoint = transform.position;

        rot_ = rot_max;

        animator_.runtimeAnimatorController = animatorControllerList_[0];
    }

    private void FixedUpdate()
    {
        Vector2 v = rigidBody2D_.velocity;

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


        rigidBody2D_.velocity = v;
    }

    // Update is called once per frame
    void Update()
    {
        // �Ƃ肠�����̃V�[���J��.
        sceneMove();

        Vector2 v = rigidBody2D_.velocity;          // �����x���擾.
        isGround_ = (v.y < 0.1) && groundCheck_.IsGround();    // �������̂ݐڒn������擾.

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
            rigidBody2D_.velocity = v;

            // �W�����v.
            rigidBody2D_.AddForce(new Vector2(0, jumpForce_), ForceMode2D.Impulse);
        }

        // ED�C�x���g���Ŏ��S���C���܂ŗ����瑬�x�ቺ.
        if(isEvent_ && rot_ <= 1f)
        {
            MoveSpeed_ = 2.5f;
            animator_.SetFloat("WalkSpeed", 0.5f);
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

        var c = GetComponent<SpriteRenderer>().color;
        // �Q�[�W����.
        if (antisepticTime_ > 0)
        {
            c = antisepticColor_;
            antisepticTime_ -= Time.deltaTime;
        }
        else
        {
            c = defaultColor_;
            antisepticTime_ = 0;
            rot_ -= rot_decreasePerSecond_ * Time.deltaTime;
            if (unDead_ && rot_ <= 1)
            {
                rot_ = 1f;
            }
        }

        if(isDeath())
        {
            c = defaultColor_;

            rot_ = 0;
            animator_.SetBool("Death", true);

            if (!isEvent_)
            {
                bgm_.Stop();
                restartTime_ -= Time.deltaTime;
            }
            if (!isChangeCurtain_ && restartTime_ <= 1.1f)
            {
                black_.ChangeColor(new Color(0,0,0,1), 1f);
                isChangeCurtain_ = true;
            }
            if (restartTime_ <= 0f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        GetComponent<SpriteRenderer>().color = c;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �h���܏���. 
        if (collision.gameObject.tag == "Antiseptic")
        {
            antisepticTime_ += collision.gameObject.GetComponent<AntisepticManager>().AntisepticTime;
        }

        // �h���܏���. 
        if (collision.gameObject.tag == "ED_Bone")
        {
            rot_ = 0;
            unDead_ = false;
        }

    }

    public void Damage(float damage)
    {
        rot_ -= damage;
        if (rot_ < 0)
        {
            rot_ = 0;
        }

        foreach (GameObject blood in blood_)
        {
            blood.transform.position = transform.position + (Vector3)Random.insideUnitCircle;

            float s = Random.value * 0.5f + 0.5f;   // 0.5 �` 1.0
            blood.transform.localScale = new Vector2(s, s);
            blood.GetComponent<AudioSource>().Play();
            blood.GetComponent<SpriteColorLeap>().Play();
        }
    }
}
