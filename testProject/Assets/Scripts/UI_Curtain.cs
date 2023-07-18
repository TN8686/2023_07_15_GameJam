using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Curtain : MonoBehaviour
{
    [SerializeField]
    private Color targetColor_;

    [SerializeField]
    private float targetTime_s_;

    [SerializeField]
    private float time_s_;

    [SerializeField]
    private Color preColor_;

    // Start is called before the first frame update
    void Start()
    {
        var c = GetComponent<Image>().color;
        preColor_ = c;
        //targetColor_ = c;
        time_s_ = 0;
        //targetTime_s_ = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (time_s_ < targetTime_s_)
        {
            time_s_ += Time.deltaTime;

            Color c;
            c.r = Mathf.Lerp(preColor_.r, targetColor_.r, time_s_ / targetTime_s_);
            c.g = Mathf.Lerp(preColor_.g, targetColor_.g, time_s_ / targetTime_s_);
            c.b = Mathf.Lerp(preColor_.b, targetColor_.b, time_s_ / targetTime_s_);
            c.a = Mathf.Lerp(preColor_.a, targetColor_.a, time_s_ / targetTime_s_);

            GetComponent<Image>().color = c;
        }
    }

    // ñ⁄ïWêFÇ…ê¸å`ï‚äÆÇµÇ‹Ç∑.
    public void ChangeColor(Color color, float targetTime_s)
    {
        var c = GetComponent<Image>().color;
        preColor_ = c;
        targetColor_ = color;
        time_s_ = 0;
        targetTime_s_ = targetTime_s;
        if(targetTime_s_ <= 0)
        {
            targetTime_s_ = 0.01f;
        }
    }
}
