using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotGauge : MonoBehaviour
{
    [SerializeField]
    private Player player_;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var rotRatio = player_.getRotRatio();
        var s = transform.GetChild(0).transform.localScale;
        s.x = rotRatio;
        transform.GetChild(0).transform.localScale = s;
    }
}
