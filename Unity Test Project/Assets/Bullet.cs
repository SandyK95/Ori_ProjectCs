using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    GameObject m_Target;
    Rigidbody2D m_RB;
    // Start is called before the first frame update
    void Start()
    {
        m_Target = GameObject.FindGameObjectsWithTag("Enemy")[0];
        m_RB = GetComponent<Rigidbody2D>();

        System.Random random = new System.Random();
        float sx = 10 + (float)(random.NextDouble() * 12);
        float sy = 10 + (float)(random.NextDouble() * 22);

        float DirX = m_Target.transform.position.x - transform.position.x;
        sx = DirX < 0 ? -sx : sx;

        if ((random.Next() & 1) != 0) sy = -sy;

        m_RB.velocity = new Vector2(sx, sy);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 Dir = m_Target.transform.position - transform.position;
        Vector2 ForceDir = Dir.normalized - m_RB.velocity.normalized;

        m_RB.AddForce(ForceDir * (m_RB.mass * 6200 * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Destroy(collision.gameObject);//destroy collided enemy
            Destroy(gameObject); //destroy self (bullet)
        }
    }
}
