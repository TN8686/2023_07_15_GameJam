using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchManager : MonoBehaviour
{
    [SerializeField]
    private bool isPushed;
    public bool isPublished { get { return isPushed; } set { isPushed = value; } }
    // Start is called before the first frame update
    void Start()
    {

    }

    void onCollisionEnter2D(Collision collision)
    {
        isPushed = true;
        Debug.Log("pushed");
    }

    void onCollisionExit2D(Collision collision)
    {
        isPushed = false;
        Debug.Log("exited");
    }
    // Update is called once per frame
    void Update()
    {

    }
}
