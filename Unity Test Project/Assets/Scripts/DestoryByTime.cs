using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryByTime : MonoBehaviour
{

    public GameObject DestoryGameObject;
    public float destoryTime = 0.3f;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(DestoryGameObject, destoryTime);
    }
}
