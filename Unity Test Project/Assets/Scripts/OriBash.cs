using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriBash : MonoBehaviour
{
    public float reachRadius = 5f;
    RaycastHit2D[] objectsNear;
    Vector3 direction;
    public float speed = 20f;
    public bool canBash;
    GameObject bashableObj;
    public float maxTime = 1f;
    public Transform arrow;
    public GameObject effect;

    // Start is called before the first frame update
    void Start()
    {
        arrow.gameObject.SetActive(false);
    }

    IEnumerator Counter()
    {
        float pauseTime = Time.realtimeSinceStartup + maxTime;
        while(Time.realtimeSinceStartup < pauseTime)
        {
            yield return null;
        }

        if(Time.timeScale == 0)
        {
            Time.timeScale = 1f;
            canBash = false;
            arrow.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            objectsNear = Physics2D.CircleCastAll(transform.position, reachRadius, Vector3.forward);

            foreach (RaycastHit2D obj in objectsNear)
            {
                if (obj.collider.gameObject.GetComponent<bashable>() != null)
                {
                    bashableObj = obj.collider.gameObject;
                    StartCoroutine("Counter");
                    Time.timeScale = 0;

                    canBash = true;
                    arrow.position = bashableObj.transform.position;
                    arrow.Translate(0, 0, 10);

                    arrow.gameObject.SetActive(true);
                    
                    Debug.Log("bash!");
                    
                    break;
                }

            }
        }
        else if((Input.GetMouseButtonDown(0)) && canBash)
        {
            Time.timeScale = 1;

            direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - bashableObj.transform.position);
            direction.z = 0;
            direction = direction.normalized;

            transform.position = bashableObj.transform.position + direction * 1.2f;

            //GetComponent<PlayerBehaviour>()
            GetComponent<Rigidbody2D>().velocity = direction * speed;

            bashableObj.GetComponent<Rigidbody2D>().velocity = direction * (-1) * bashableObj.GetComponent<Rigidbody2D>().velocity.magnitude;

            canBash = false;
            arrow.gameObject.SetActive(false);
            Debug.Log("fIRE!");
        }

        else if((Input.GetMouseButtonDown(0)) && canBash)
        {
            Vector3 different = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            different.Normalize();

            float rotation_z = Mathf.Atan2(different.y, different.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0f, 0f, rotation_z);
            Instantiate(effect, bashableObj.transform.position, Quaternion.identity);
        }
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, reachRadius);
    }
}
