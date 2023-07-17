using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Dog : MonoBehaviour
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

    // 接地関連.
    [SerializeField]
    private bool isGround_ = false;
    private GroundCheck groundCheck_;

    // Start is called before the first frame update
    void Start()
    {
        rbody2D_ = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {

        Vector2 v = rbody2D_.velocity;

        v.x = -MoveSpeed_;

        rbody2D_.velocity = v;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
