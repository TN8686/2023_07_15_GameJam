using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntisepticManager : MonoBehaviour
{
    [SerializeField]
    private float antisepticTime = 10;
    public float AntisepticTime { get { return antisepticTime; } }

    [SerializeField]
    bool isGot_;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isGot_)
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                gameObject.SetActive(false);
            }

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.tag == "Player")
        {
            isGot_ = true;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<AudioSource>().Play();
        }
    }
}