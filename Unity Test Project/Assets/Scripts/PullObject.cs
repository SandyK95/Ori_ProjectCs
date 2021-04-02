using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullObject : MonoBehaviour
{
    public float defaultMass;
    public float imoveableMass;
    public bool beingPushed;
    float xPos;

    public Vector3 lastPos;

    public int mode;
    public int colliding;

    // Start is called before the first frame update
    void Start()
    {
        xPos = transform.position.x;
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Pusbable();
    }

    void Pusbable()
    {
        if(mode == 0)
        {
            if (beingPushed == false)
            {
                transform.position = new Vector3(xPos, transform.position.y);
            }

            else xPos = transform.position.x;
        }

        else if(mode == 1)
        {
            if (beingPushed == false)
            {
                GetComponent<Rigidbody2D>().mass = imoveableMass;
            }
            else GetComponent<Rigidbody2D>().mass = defaultMass;
        }
    }
}
