using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    [SerializeField]
    private Sprite nextSprite_;

    [SerializeField]
    private float waitTime_;

    float velY = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        waitTime_ += Time.deltaTime;

        if (waitTime_ > 2.5f){
            GetComponent<SpriteRenderer>().sprite = nextSprite_;            
        }

        if(waitTime_ > 4f)
        {
            var pos = transform.position;
            if (pos.y < 10f)
            {

                velY += 0.1f * Time.deltaTime;
                pos.y += velY * Time.deltaTime;
            }

            transform.position = pos;

        }

    }
}
