using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotGauge : MonoBehaviour
{
    [SerializeField]
    private Player player_;

    [SerializeField]
    private Color[] colors_;

    [SerializeField]
    private Color antisepticColor_;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var gauge = transform.GetChild(0);
        var rotRatio = player_.getRotRatio();
        var s = gauge.transform.localScale;
        s.x = rotRatio;
        gauge.transform.localScale = s;

        // 色選択.
        Color c;
        var a = gauge.GetComponent<Image>().color.a;
        if (player_.AntisepticTime > 0)
        {
            c = antisepticColor_;
        }
        else
        {
            if (rotRatio * player_.Rot_max > player_.RotLine_01)
            {
                c = colors_[0];
            }
            else if (rotRatio * player_.Rot_max > player_.RotLine_02)
            {
                c = colors_[1];
            }
            else
            {
                c = colors_[2];
            }
        }

        c.a = a;    // 透明度だけ変更しない.
        gauge.GetComponent<Image>().color = c;

    }
}
