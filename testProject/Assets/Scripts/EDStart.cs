using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EDStart : MonoBehaviour
{
    [SerializeField]
    private Image[] gauge_;

    [SerializeField]
    private GameObject audioObj_;

    [SerializeField]
    private float targetPosX_ = 40f;

    private GameObject player_;
    private float baseRotSpeed_ = 0;
    // Start is called before the first frame update
    void Start()
    {
        player_ = null;
        if(targetPosX_ == 0)
        {
            targetPosX_ = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player_ != null)
        {
            var rel = player_.transform.position.x - transform.position.x;
            if(rel < 0)
            {
                rel = 0;
            }

            var ratio = rel / targetPosX_;

            audioObj_.GetComponent<AudioSource>().volume = 1f - ratio;

            player_.GetComponent<Player>().Rot_decreasePerSecond = baseRotSpeed_ + ratio * ratio * 5;

            // ゲージの色変更.
            var c = gauge_[0].color;
            c.a = 1f - ratio;
            gauge_[0].color = c;
            c = gauge_[1].color;
            c.a = 1f - ratio;
            gauge_[1].color = c;

        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {
            player_ = collision.gameObject;
            var p = player_.GetComponent<Player>();

            // 既に死んでたら何もしない.
            if (p.isDeath())
            {
                return;
            }

            p.UnDead = true;
            p.IsEvent = true;
            baseRotSpeed_ = player_.GetComponent<Player>().Rot_decreasePerSecond;
        }
    }
}
