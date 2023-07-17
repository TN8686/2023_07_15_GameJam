using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack : MonoBehaviour
{
    //private string groundTag = "Ground";
    [SerializeField]
    private string[] groundTag_ = { "Player"};

    private bool isGround = false;
    private bool isGroundEnter, isGroundStay, isGroundExit;

    [SerializeField]
    private float power_ = 10f;

    //�ڒn�����Ԃ����\�b�h
    //��������̍X�V���ɌĂԕK�v������
    public bool IsGround()
    {

        if (isGroundEnter || isGroundStay)
        {
            isGround = true;
        }
        if (isGroundExit)
        {
            isGround = false;
        }

        isGroundEnter = false;
        isGroundStay = false;
        isGroundExit = false;
        return isGround;
    }

    public bool IsGroundEnter()
    {
        return isGroundEnter;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (string tag in groundTag_)
        {
            if (collision.tag == tag)
            {
                isGroundEnter = true;
                collision.gameObject.GetComponent<Player>().Damage(power_);
                GetComponent<BoxCollider2D>().enabled = false;
                //Debug.Log("isGroundEnter");
            }
        }
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        foreach (string tag in groundTag_)
        {
            if (collision.tag == tag)
            {
                isGroundStay = true;
                //Debug.Log("isGroundStay");
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        foreach (string tag in groundTag_)
        {
            if (collision.tag == tag)
            {
                isGroundExit = true;
                //Debug.Log("isGroundExit");
            }
        }
    }

}
