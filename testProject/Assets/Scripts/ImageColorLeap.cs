using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageColorLeap : MonoBehaviour
{
    // 目標カラー.
    [SerializeField]
    private Color targetColor_ = new Color(1f, 1f, 1f, 0f);
    public Color TargetColor
    {
        get { return targetColor_; }
        set { targetColor_ = value; }
    }

    // 線形補完の目標時間.
    [SerializeField]
    private float targetTime_s_ = 1;
    public float TargetTime_s
    {
        get { return targetTime_s_; }
        set { targetTime_s_ = value; }
    }

    // 線形補完の現在時間.
    [SerializeField]
    private float time_s_ = 1;
    public float Time_s
    {
        get { return time_s_; }
        set { time_s_ = value; }
    }

    // 補完開始前のカラー.
    [SerializeField]
    private Color preColor_;
    public Color PreColor
    {
        get { return preColor_; }
        set { preColor_ = value; }
    }

    private Image image_;

    // Start is called before the first frame update
    void Start()
    {
        image_ = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Color c = targetColor_;

        time_s_ += Time.deltaTime;

        if (time_s_ > targetTime_s_)
        {
            time_s_ = targetTime_s_;
        }

        c.r = Mathf.Lerp(preColor_.r, targetColor_.r, time_s_ / targetTime_s_);
        c.g = Mathf.Lerp(preColor_.g, targetColor_.g, time_s_ / targetTime_s_);
        c.b = Mathf.Lerp(preColor_.b, targetColor_.b, time_s_ / targetTime_s_);
        c.a = Mathf.Lerp(preColor_.a, targetColor_.a, time_s_ / targetTime_s_);

        image_.color = c;
    }

    // 目標色に目標時間かけて線形補完します.
    public void ChangeColor(Color targetColor, float targetTime_s)
    {
        var c = image_.color;
        preColor_ = c;
        targetColor_ = targetColor;
        time_s_ = 0;
        targetTime_s_ = targetTime_s;
        if (targetTime_s_ <= 0)
        {
            targetTime_s_ = 0.01f;
        }
    }

    // 設定を変えずに再度色補完を行います.
    public void Play()
    {
        image_.color = preColor_;
        time_s_ = 0;
    }

    // 現段階の比率を返します.
    public float Ratio()
    {
        return time_s_ / targetTime_s_;
    }
}
