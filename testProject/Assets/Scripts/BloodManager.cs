using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodManager : MonoBehaviour
{
    [SerializeField]
    // フェードのスピード 
    private float fadingSpeed = 0.01f;

    // フェードしているか
    [SerializeField]
    private bool isFading = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Color color = GetComponent<SpriteRenderer>().color;
        if (isFading)
        {
            // 0までフェード
            if (color.a > 0)
            {
                color.a -= fadingSpeed;
            }
        }
        else
        {
            // フェードする前、まず透明度を1にする
            color.a = 1;
            isFading = true;
        }

        GetComponent<SpriteRenderer>().color = color;
    }


}
