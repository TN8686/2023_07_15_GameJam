using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    private Camera camera_;
    // Start is called before the first frame update
    void Start()
    {
        camera_ = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        var p = transform.position;
        if (p.x < camera_.transform.position.x - 24)
        {
            p.x += 60;
        }
        else if(p.x > camera_.transform.position.x + 24)
        {
            p.x -= 60;
        }
        transform.position = p;
    }

}
