using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerBulletBehaviour : MonoBehaviour
{
    public GameObject target;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 2.0f * Time.deltaTime);
        }
        else
        {
            Destroy(gameObject); //destroy self (bullet)
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Destroy(collision.gameObject);//destroy collided enemy
            Destroy(gameObject); //destroy self (bullet)
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.collider.GetComponent<Enemy_AI>();
        if (enemy)
        {
            enemy.TakeHit(1);
        }

        Destroy(gameObject);
    }
}
