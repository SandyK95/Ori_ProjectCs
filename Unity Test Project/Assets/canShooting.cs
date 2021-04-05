﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canShooting : MonoBehaviour
{
    public GameObject Bullet;
    public Vector2 velocity;
    bool canShoot = true;
    public Vector2 offset = new Vector2(0.4f, 0.1f);
    public float cooldown = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T) && canShoot)
        {
            GameObject go = Instantiate(Bullet, (Vector2)transform.position + offset * transform.localScale.x, Quaternion.identity);
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * transform.localScale.x, velocity.y);
        }
    }

    IEnumerator CanShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision);
    }
}
