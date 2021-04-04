using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallJump : MonoBehaviour
{
    public float distancee = 1f;
    PlayerBehaviour movement;
    public float speed = 2f;
    bool wallJumping;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distancee);

        if(Input.GetKeyDown(KeyCode.W) && !movement.groundChecker && hit.collider != null)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(speed * hit.normal.x, speed);
            //StartCoroutine("TurnIt");
        }
    }

    IEnumerator TurnIt()
    {
        yield return new WaitForFixedUpdate();
        transform.localScale = transform.localScale.x == 1 ? new Vector2(-1, 1) : Vector2.one;
    }
}
