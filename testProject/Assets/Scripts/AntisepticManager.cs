using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntisepticManager : MonoBehaviour
{
    [SerializeField]
    private float antisepticTime = 5000;
    public float AntisepticTime { get { return antisepticTime; } }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(AntisepticTime);
    }

    // Update is called once per frame
    void Update()
    {

    }
}