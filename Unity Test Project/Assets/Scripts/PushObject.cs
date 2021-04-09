using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    //Push
    public float distance = 1f;
    public LayerMask pushMask;
    GameObject pushObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckPush();
    }

    void CheckPush()
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance, pushMask);

        //Debug.Log("pushMask: " + pushMask);
        if (hit.collider != null && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("keydown");
            pushObject = hit.collider.gameObject;
            pushObject.GetComponent<FixedJoint2D>().enabled = true;
            pushObject.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
            pushObject.GetComponent<PullObject>().beingPushed = true;
        }

        else if (Input.GetKeyUp(KeyCode.E))
        {
            Debug.Log("keyup");
            pushObject.GetComponent<FixedJoint2D>().enabled = false;
            pushObject.GetComponent<PullObject>().beingPushed = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * distance);

    }
}
