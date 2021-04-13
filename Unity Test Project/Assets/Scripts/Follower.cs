using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    Rigidbody2D rb;
    GameObject  follower;
    Vector2     m_AccForce = Vector2.zero;

    public GameObject followerBullet;
    public float m_Distance;
    public float m_Force;
    public float shootingRate;
    private float shootingRateCurrent;

    private bool canShoot;

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
        //transform.LookAt(follower.transform.position);
        if(Mathf.Abs(Dir.x) > 1)
        {
            if (Mathf.Abs(rb.velocity.x) < 3)
                m_AccForce += new Vector2((Dir.x > 0) ? m_Force : -m_Force, 0)
                    * rb.mass * Time.deltaTime;
        }

        if (Mathf.Abs(Dir.y) > 1)
        {
            if (Mathf.Abs(rb.velocity.y) < 3)
                m_AccForce += new Vector2(0, (Dir.y > 0) ? m_Force * 30 : -m_Force * 30)
                    * rb.mass * Time.deltaTime;
        }

        CheckTimer();
        if (DetectIfEnemyNear() == true && canShoot == true)
        {
            SpawnBullet();
            canShoot = false;
        }
    }

    private void FixedUpdate()
    {
        var Force = -(rb.velocity / Time.fixedDeltaTime + Physics2D.gravity);

        Force.x *= 0.02f;

        rb.AddForce(Force + m_AccForce);

        m_AccForce = Vector2.zero;
    }

    private bool DetectIfEnemyNear()
    {
        GameObject[] arrayOfObjects = GameObject.FindGameObjectsWithTag("Enemy");
        //replace InteractiveBehaviour to enemy's behaviour next time
        for (int i = 0; i < arrayOfObjects.Length; i++)
        {
            //change 5.0f to something. more value = further
            if (Vector2.Distance(transform.position, arrayOfObjects[i].transform.position) < 5.0f)
            {
                return true;
            }
        }
        return false;
    }

    private GameObject CheckNearestEnemy()
    {
        GameObject[] arrayOfObjects = GameObject.FindGameObjectsWithTag("Enemy");
        int savedArrayIndex = 0;
        float nearestDistance = 0.0f;
        //replace InteractiveBehaviour to enemy's behaviour next time
        for (int i = 0; i < arrayOfObjects.Length; i++)
        {
            if (nearestDistance == 0.0f || Vector2.Distance(transform.position, arrayOfObjects[i].transform.position) < nearestDistance)
            {
                nearestDistance = Vector2.Distance(transform.position, arrayOfObjects[i].transform.position);
                savedArrayIndex = i;
            }
        }
        return arrayOfObjects[savedArrayIndex].gameObject;
    }

    private void SpawnBullet()
    {
        Instantiate(followerBullet, transform.position, Quaternion.identity).GetComponent<FollowerBulletBehaviour>().target = CheckNearestEnemy();
    }

    private void CheckTimer()
    {
        if (canShoot == false)
        {
            if (shootingRateCurrent < shootingRate)
            {
                shootingRateCurrent += Time.deltaTime;
            }
            else
            {
                canShoot = true;
                shootingRateCurrent = 0.0f;
            }
        }
    }
}
