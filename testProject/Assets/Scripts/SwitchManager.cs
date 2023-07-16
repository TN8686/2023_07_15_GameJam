using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchManager : MonoBehaviour
{
    [SerializeField]
    private bool isPushed = false;
    private int count = 0;
    public bool IsPushed { get { return isPushed; } set { isPushed = value; } }
    // Start is called before the first frame update
    void Start()
    {
    }

    void onCollisionEnter2D(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Block")
        {
            count++;
        }
    }

    void onCollisionExit2D(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Block")
        {
            count--;
        }
    }
    // Update is called once per frame
    void Update()
    {
        IsPushed = (count > 0);
    }
}
