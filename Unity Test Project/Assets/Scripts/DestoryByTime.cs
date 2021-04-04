using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryByTime : MonoBehaviour
{

    
    public float destoryTime = 0.3f;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, destoryTime);
    }
}
