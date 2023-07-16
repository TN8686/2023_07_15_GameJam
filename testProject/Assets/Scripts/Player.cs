using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
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
        //�n�ʓ����蔻��
        //�ڒn����𓾂�

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

        // �����|�C���g�ݒ�.
        if (isGround_)
        {
            pos.y = 10;
            fallRespawnPoint = pos;
        }

        //�W�����v
        if (Input.GetKeyDown(KeyCode.Space) && isGround_)
        {
            // �㉺���x��������.
            v.y = 0;
            rbody2D_.velocity = v;

            // �W�����v.
            rbody2D_.AddForce(new Vector2(0, jumpForce_), ForceMode2D.Impulse);
        }

        // ���W�����ȉ��������畜�A�|�C���g�̏ォ�畜�A.
        pos = transform.position;
        if (pos.y <= -10f)
        {
            pos = fallRespawnPoint;
            transform.position = pos;
        }

        // �Q�[�W����.
        rot -= rot_decreasePerSecond * Time.deltaTime;
        if(rot <= 0)
        {
            rot = 0;    // TODO�@���S.
        }

    }

}
