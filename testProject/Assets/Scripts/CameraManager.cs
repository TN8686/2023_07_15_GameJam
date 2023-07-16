using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private GameObject target_;

    private void LateUpdate()
    {
        var p = transform.position;
        p.x = target_.transform.position.x;

        // ¶ƒJƒƒ‰ŒÀŠE.
        if(p.x <= 0)
        {
            p.x = 0;
        }

        transform.position = p;
    }
}
