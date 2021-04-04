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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            objectsNear = Physics2D.CircleCastAll(transform.position, reachRadius, Vector3.forward);

            foreach (RaycastHit2D obj in objectsNear)
            {
                if (obj.collider.gameObject.GetComponent<bashable>() != null)
                {
                    
                    Time.timeScale = 0;

                    Debug.Log("bash!");
                }

            }
        }
        else if(Input.GetButtonDown("Fire2"))
        {
            Time.timeScale = 1;

            Debug.Log("fIRE!");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, reachRadius);
    }
}
