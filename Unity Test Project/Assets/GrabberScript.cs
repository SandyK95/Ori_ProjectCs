using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabberScript : MonoBehaviour
{
    public bool grabbed;
    RaycastHit2D hit;
    public float distance = 2f;
    public Transform holdpoint;
    public float throwForce;
    public LayerMask notgrabbed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrabbed();
    }

    void CheckGrabbed()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!grabbed)
            {
                Physics2D.queriesStartInColliders = false;
                //grab
                hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x,distance);

                if(hit.collider != null && hit.collider.tag == "grabbable")
                {
                    grabbed = true;
                }

            }
            else if (!Physics2D.OverlapPoint(holdpoint.position, notgrabbed))
            {
                //throw
                grabbed = false;

                if (hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
                {

                    hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x, 1) * throwForce;
                }
            }
        }

        if(grabbed)
        {
            hit.collider.gameObject.transform.position = holdpoint.position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * transform.localScale.x * distance);
    }
}
