using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private string groundTag = "Ground";
    private bool isGround = false;
    private bool isGroundEnter, isGroundStay, isGroundExit;

    //接地判定を返すメソッド
    //物理判定の更新毎に呼ぶ必要がある
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

    public bool IsGroundEntor()
    {
        return isGroundEnter;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == groundTag)
        {
            isGroundEnter = true;
            //Debug.Log("isGroundEntor");
        }
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == groundTag)
        {
            isGroundStay = true;
            //Debug.Log("isGroundStay");
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {


        if (collision.tag == groundTag)
        {
            isGroundExit = true;

            //Debug.Log("isGroundExit");
        }
    }

}
