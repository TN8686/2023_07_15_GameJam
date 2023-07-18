using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EDStart : MonoBehaviour
{
    [SerializeField]
    private Image[] gauge_;

    [SerializeField]
    //private AudioSource[] audio_;
    private GameObject audioObj_;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {
            var p = collision.gameObject.GetComponent<Player>();

            // ä˘Ç…éÄÇÒÇ≈ÇΩÇÁâΩÇ‡ÇµÇ»Ç¢.
            if (p.isDeath())
            {
                return;
            }

            // Ç¢Ç´Ç»ÇËè¡ÇµÇøÇ·Ç§.
            var c = gauge_[0].color;

            c.a = 0;
            gauge_[0].color = c;
            gauge_[1].color = c;

            p.UnDead = true;
            p.IsEvent = true;

            p.Rot_decreasePerSecond = 2.5f;

            audioObj_.GetComponent<AudioSource>().enabled = false;
            //audio_[0].Stop();
        }
    }
}
