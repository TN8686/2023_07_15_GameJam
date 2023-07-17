using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    //private string groundTag = "Ground";
    [SerializeField]
    private string[] groundTag_ = { "Ground", "Block" };

    private bool isGround = false;
    private bool isGroundEnter, isGroundStay, isGroundExit;

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
                //Debug.Log("isGroundEntor");
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
