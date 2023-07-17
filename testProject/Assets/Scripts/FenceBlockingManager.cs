using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceBlockingManager : MonoBehaviour
{
    [SerializeField]
    private int _count = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Block")
        {
            _count++;
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Block")
        {
            _count--;
        }
    }
    // Update is called once per frame
    public bool IsBlocked()
    {
        return (_count > 0);
    }
}
