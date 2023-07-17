using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceManager : MonoBehaviour
{
    // 柵が降りる速度
    [SerializeField]
    private float _velocity = 0.15f;

    // 柵が降りるマップチップ数の限度
    [SerializeField]
    private float _downLimit = -2f;

    // ボタンを押したときに開く(false)のか、ボタンを押したときに閉じるのか(true)
    [SerializeField]
    private bool _isReversed = false;

    // 連携するボタンを設定
    [SerializeField]
    private SwitchManager sm = null;

    // ドアに物体が挟まらないように検知するオブジェクト
    private FenceBlockingManager fbm = null;

    [SerializeField]
    private AudioClip Fence;
    AudioSource audioSource;

    // 下降しているか
    private bool isDuringDownward;
    // 上昇しているか
    private bool isDuringUpward;

    // Start is called before the first frame update
    void Start()
    {
        fbm = GetComponentInChildren<FenceBlockingManager>();
        audioSource = GetComponent<AudioSource>();
    }

    void Awake()
    {
        var pos = transform.GetChild(0).localPosition;
        if (_isReversed)
        {
            pos.y = _downLimit;
        }
        transform.GetChild(0).localPosition = pos;
    }

    // Update is called once per frame
    void Update()
    {
        var pos = transform.GetChild(0).localPosition;

        var existsSignal = sm.IsPushed();
        if (_isReversed)
        {
            existsSignal = !existsSignal;
        }

        // ボタンが押されているか、間になにか挟まっている間は閉まるように
        if (existsSignal || fbm.IsBlocked())
        {
            if (pos.y == 0)
            {
                audioSource.PlayOneShot(Fence);
            }
            if (pos.y > _downLimit)
            {
                if (isDuringUpward)
                {
                    audioSource.PlayOneShot(Fence);
                }
                pos.y -= _velocity;
                isDuringDownward = true;
                isDuringUpward = false;
            }
            else
            {
                pos.y = _downLimit;
            }
        }
        else
        {
            if (pos.y == _downLimit)
            {
                audioSource.PlayOneShot(Fence);
            }
            if (pos.y < 0)
            {
                if (isDuringDownward)
                {
                    audioSource.PlayOneShot(Fence);
                }
                pos.y += _velocity;
                isDuringDownward = false;
                isDuringUpward = true;
            }
            else
            {
                pos.y = 0;
            }
        }
        transform.GetChild(0).localPosition = pos;
    }
}
