using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCharacter_;
    [SerializeField]
    private GameObject grave_;
    [SerializeField]
    private GameObject grave_thunder_;
    [SerializeField]
    private GameObject dog_;
    [SerializeField]
    private GameObject hand_;
    [SerializeField]
    private GameObject white_;
    [SerializeField]
    private GameObject black_;


    [SerializeField]
    private GameObject logo_;
    [SerializeField]
    private GameObject text_;


    [SerializeField]
    private AudioSource thunderSE01_;
    [SerializeField]
    private AudioSource thunderSE02_;

    [SerializeField]
    private AudioSource handSE_;
    [SerializeField]
    private AudioSource OP_BGM_;





    [SerializeField]
    float waitTime_;

    [SerializeField]
    float[] eventWaitTimeList_;

    [SerializeField]
    int phase_ = 0;

    [SerializeField]
    bool play_ = false;


    private bool isSEPlayed01_;

    private bool isSEPlayed02_;

    float HAND_INIT_POS_Y;

    float MC_INIT_POS_Y;
    // Start is called before the first frame update
    void Start()
    {

#if !UNITY_EDITOR
        // カーソル非表示
        Cursor.visible = false;
#endif

        HAND_INIT_POS_Y = hand_.GetComponent<RectTransform>().position.y;
        MC_INIT_POS_Y = mainCharacter_.GetComponent<RectTransform>().position.y;
    }

    // Update is called once per frame
    void Update()
    {
        sceneMove();

        Color c;

        if (play_)
        {
            waitTime_ += Time.deltaTime;

            switch (phase_) {
                case 0:
                    c = logo_.GetComponent<Text>().color;
                    c.a = 1f - (waitTime_) / (eventWaitTimeList_[phase_]);
                    logo_.GetComponent<Text>().color = c;

                    /*
                    c = text_.GetComponent<Text>().color;
                    c.a = 1f - (waitTime_) / (eventWaitTimeList_[phase_]);
                    text_.GetComponent<Text>().color = c;
                    */
                    text_.GetComponent<Text>().enabled = false;

                    endCheck();

                    break;

                case 1:
                    if (!isSEPlayed01_)
                    {
                        thunderSE01_.Play();
                        isSEPlayed01_ = true;
                    }

                    if (waitTime_ > 4.1f)
                    {
                        dog_.SetActive(false);
                        white_.SetActive(false);

                        grave_.SetActive(true);

                        grave_thunder_.SetActive(true);
                        c = grave_.GetComponent<Image>().color;

                        float rgb = (waitTime_ - 4.1f) / (eventWaitTimeList_[phase_] - 4.1f);
                        c = new Color(rgb, rgb, rgb, 1);
                        grave_.GetComponent<Image>().color = c;


                        c = new Color(1, 1, 1, 1f - rgb);
                        grave_thunder_.GetComponent<Image>().color = c;

                        if (!isSEPlayed02_)
                        {
                            thunderSE02_.Play();
                            isSEPlayed02_ = true;
                        }

                    }
                    else if (waitTime_ > 4f)
                    {
                        dog_.SetActive(true);
                        white_.SetActive(true);
                        grave_.SetActive(false);
                    }

                    endCheck();
                    break;

                case 2:

                    if(waitTime_ > 3f)
                    {
                        float num2 = (waitTime_ - 3f) / (3.1f - 3f);

                        if(num2 > 1f)
                        {
                            num2 = 1f;
                        }

                        var handPos = hand_.GetComponent<RectTransform>().position;
                        handPos.y = HAND_INIT_POS_Y + (3) * num2;
                        hand_.GetComponent<RectTransform>().position = handPos;

                        hand_.SetActive(true);

                        if (!isSEPlayed01_)
                        {
                            handSE_.Play();
                            isSEPlayed01_ = true;
                        }
                    }

                    endCheck();
                    break;
                 case 3:

                    if (waitTime_ > 6.0f) {
                    
                    }
                    else
                    {
                        hand_.SetActive(false);
                        grave_.SetActive(false);
                        mainCharacter_.SetActive(true);

                        float num = (waitTime_ - 1f) / (6.0f - 1f);

                        if (num > 1f)
                        {
                            num = 1f;
                        }
                        else if (num < 0)
                        {
                            num = 0;
                        }
                        var cPos = mainCharacter_.GetComponent<RectTransform>().position;
                        cPos.y = MC_INIT_POS_Y + (-14.5f) * num;
                        mainCharacter_.GetComponent<RectTransform>().position = cPos;

                        if (!isSEPlayed01_)
                        {
                            OP_BGM_.Play();
                            isSEPlayed01_ = true;
                        }
                    }
                    endCheck();
                    break;

                case 4:
                    if (black_.GetComponent<ImageColorLeap>().TargetColor.a < 1f)
                    {
                        black_.GetComponent<ImageColorLeap>().ChangeColor(new Color(0f, 0f, 0f, 1f), 1f);
                    }
                    else if (black_.GetComponent<ImageColorLeap>().Ratio() >= 1f)
                    {
                        SceneManager.LoadScene("MainGameScene");
                    }
                    break;

            }

        }
        else
        {
            if (Input.GetKey(KeyCode.Space))
            {
                play_ = true;
            }
        }
    }

    private void endCheck()
    {
        if (waitTime_ >= eventWaitTimeList_[phase_])
        {
            ++phase_;
            waitTime_ = 0;

            isSEPlayed01_ = false;
            isSEPlayed02_ = false;
            
        }

    }

    public void sceneMove()
    {
        //Escが押された時
        if (Input.GetKey(KeyCode.Escape))
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
        }
    }
}
