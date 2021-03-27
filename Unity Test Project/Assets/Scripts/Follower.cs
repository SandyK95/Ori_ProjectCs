using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    Rigidbody2D rb;
    GameObject  follower;
    Vector2     m_AccForce = Vector2.zero;

    public float m_Distance;
    public float m_Force;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        follower = FindObjectOfType<PlayerBehaviour>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        var Dir = (follower.transform.position + new Vector3(0, m_Distance, 0)) - transform.position;

        if(Mathf.Abs(Dir.x) > 1)
        {
            if (Mathf.Abs(rb.velocity.x) < 3)
                m_AccForce += new Vector2((Dir.x > 0) ? m_Force : -m_Force, 0)
                    * rb.mass * Time.deltaTime;
        }

        else if(Mathf.Abs(rb.velocity.y) > 0.5f)
        {
            if (Mathf.Abs(rb.velocity.y) < 3)
                m_AccForce += new Vector2(0, (Dir.y > 0) ? m_Force * 15 : -m_Force * 15) * rb.mass * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        var Force = -(rb.velocity / Time.fixedDeltaTime + Physics2D.gravity);

        Force.x *= 0.02f;

        rb.AddForce(Force + m_AccForce);

        m_AccForce = Vector2.zero;
    }
}
