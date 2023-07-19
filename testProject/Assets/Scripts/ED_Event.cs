using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ED_Event : MonoBehaviour
{

    bool isPlay = false;
    
    [SerializeField]
    private Image[] images_;

    float waitTime_ = 0;

    private bool isBGMPlay_ = false;

    [SerializeField]
    private GameObject player_soul_;

    [SerializeField]
    private GameObject dog_soul_;


    [SerializeField]
    private AudioSource lain_;

    [SerializeField]
    private Text staffedRoll_;

    [SerializeField]
    private GameObject black_;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlay)
        {
            return;
        }

        waitTime_ += Time.deltaTime;

        var c = images_[0].color;
        c.a = (waitTime_ - 4f) / 1;

        if (c.a > 1)
        {
            c.a = 1;
        }
        images_[0].color = c;

        float vol = waitTime_ - 4f;
        if(vol < 0)
        {
            vol = 0;
        }
        lain_.volume = 0.25f * (1f - vol);

        c = images_[1].color;
        c.a = (waitTime_ - 5f) / 1;
        if (c.a > 1)
        {
            c.a = 1;
        }
        images_[1].color = c;


        if (waitTime_ > 5 && !isBGMPlay_)
        {
            isBGMPlay_ = true;
            GetComponent<AudioSource>().Play();
            lain_.Stop();
        }

        c = images_[2].color;
        c.a = (waitTime_ - 8.5f) / 1;
        if (c.a > 1)
        {
            c.a = 1;
        }
        c.a *= 0.8f;

        images_[2].color = c;


        if (waitTime_ > 15f)
        {
            c = images_[1].color;
            c.a = 1 - (waitTime_ - 15f) / 1;
            if (c.a < 0)
            {
                c.a = 0;
            }
            c.a *= 0.8f;

            images_[1].color = c;

            c = images_[2].color;
            c.a = 1 - (waitTime_ - 15f) / 1;
            if (c.a < 0)
            {
                c.a = 0;
            }
            c.a *= 0.8f;

            images_[2].color = c;

        }

        if (waitTime_ > 16f)
        {
            c = player_soul_.GetComponent<SpriteRenderer>().color;
            c.a = (waitTime_ - 16f) / 1;
            if (c.a > 1)
            {
                c.a = 1;
            }
            c.a *= 0.8f;
            player_soul_.GetComponent<SpriteRenderer>().color = c;

            c = dog_soul_.GetComponent<SpriteRenderer>().color;
            c.a = (waitTime_ - 16f) / 1;
            if (c.a > 1)
            {
                c.a = 1;
            }
            c.a *= 0.8f;
            dog_soul_.GetComponent<SpriteRenderer>().color = c;


            player_soul_.SetActive(true);
            dog_soul_.SetActive(true);

        }

        if(waitTime_ > 36.75f)
        {
            c = images_[0].color;
            c.a = 1 - (waitTime_ - 36.75f) / 1;

            if (c.a > 1)
            {
                c.a = 1;
            }
            images_[0].color = c;

            staffedRoll_.gameObject.SetActive(true);

            if (waitTime_ <= 66.5f) {
                var pos = staffedRoll_.rectTransform.position;
                pos.y += 1.15f * Time.deltaTime;
                staffedRoll_.rectTransform.position = pos;
            }
        }

        if(waitTime_ > 69f)
        {
            if (!lain_.isPlaying)
            {
                lain_.Play();
            }
            vol = (waitTime_ - 69f);
            if(vol > 1f)
            {
                vol = 1f;
            }
            lain_.volume = vol;
        }


        if (waitTime_ > 70f)
        {
            float a = black_.GetComponent<Image>().color.a;
            if (Input.GetKey(KeyCode.Space) && a <= 0)
            {
                black_.GetComponent<ImageColorLeap>().ChangeColor(new Color(0f, 0f, 0f, 1f), 1f);
            }
            lain_.volume = 1f - a;

            if (a >= 1f)
            {
                SceneManager.LoadScene("TitleScene");
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlay = true;
            foreach(Image img in images_)
            {
                img.enabled = true;
            }
        }
    }
}
