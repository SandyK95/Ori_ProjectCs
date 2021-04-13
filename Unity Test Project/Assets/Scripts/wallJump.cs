using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallJump : MonoBehaviour
{
    public float distancee = 1f;
    PlayerBehaviour movement;
    public float speed = 2f;
    public float forceAmount;
    bool wallJumping;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        //Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (Vector2.right * transform.localScale.x).normalized, distancee);

        if(Input.GetKeyDown(KeyCode.W) && !movement.groundChecker.isOnGround && hit.collider != null)
        {
            GetComponent<Rigidbody2D>().gravityScale = 1.0f;
            GetComponent<Rigidbody2D>().AddForce(((Vector2.right * -transform.localScale.x).normalized * forceAmount) + (Vector2.up * forceAmount/2), ForceMode2D.Impulse);
            //transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            //GetComponent<Rigidbody2D>().velocity = new Vector2(speed * hit.normal.x, speed);
            //StartCoroutine("TurnIt");
        }
    }

    IEnumerator TurnIt()
    {
        yield return new WaitForFixedUpdate();
        transform.localScale = transform.localScale.x == 1 ? new Vector2(-1, 1) : Vector2.one;
    }
}
