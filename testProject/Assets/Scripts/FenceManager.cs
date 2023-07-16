using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceManager : MonoBehaviour
{
    [SerializeField]
    private float _velocity = 0.15f;

    [SerializeField]
    private float _downLimit = 0;

    //[SerializeField]
    //GameObject _switch_;

    [SerializeField]
    private bool _reverseFlag = false;

    [SerializeField]
    private SwitchManager sm;
    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        var pos = transform.GetChild(0).localPosition;
        if (_reverseFlag)
        {
            pos.y = _downLimit;
        }
        transform.GetChild(0).localPosition = pos;
    }

    // Update is called once per frame
    void Update()
    {
        var pos = transform.GetChild(0).localPosition;
        var downFlag = sm.IsPushed();
        if (_reverseFlag)
        {
            downFlag = !downFlag;
        }

        if (downFlag)
        {
            if (pos.y > _downLimit)
            {
                pos.y -= _velocity;
            }
            else
            {
                pos.y = _downLimit;
            }
        }
        else
        {
            if (pos.y < 0)
            {
                pos.y += _velocity;
            }
            else
            {
                pos.y = 0;
            }
        }
        transform.GetChild(0).localPosition = pos;
    }
}
