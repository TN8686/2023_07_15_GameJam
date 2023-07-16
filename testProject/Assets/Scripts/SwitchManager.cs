using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchManager : MonoBehaviour
{
    [SerializeField]
    private int Count = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {

    }

    void onCollisionEnter2D(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Block")
        {
            Count++;
        }
    }

    void onCollisionExit2D(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Block")
        {
            Count--;
        }
    }
    // Update is called once per frame
    public bool IsPushed()
    {
        return (Count > 0);
    }
}
