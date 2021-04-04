using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallSlide : MonoBehaviour
{
    public float distance = 0.4f;
    PlayerBehaviour movement;
    public float speed = -3;
    public float gravityScale = 0.5f;

    public bool onlyDown;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance);

        if(onlyDown)
        {
            if(!movement.groundChecker && hit.collider != null && GetComponent<Rigidbody2D>().velocity.y < speed)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed);
            }
        }
        else
        {
            if (!movement.groundChecker && hit.collider != null)
            {
                GetComponent<Rigidbody2D>().gravityScale = gravityScale;
            }
            else
                GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }
}
