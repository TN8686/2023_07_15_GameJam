using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchManager : MonoBehaviour
{
    [SerializeField]
    private int _count = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        Debug.Log(_count);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Block")
        {
            _count++;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Block")
        {
            _count--;
        }
    }
    // Update is called once per frame
    public bool IsPushed()
    {
        return (_count > 0);
    }
}
