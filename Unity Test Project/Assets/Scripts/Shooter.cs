using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject projectTile;
    public float speedFactor;
    public float delay;

    public Vector3 rotation;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Shooting());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Shooting()
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);

            GameObject clone = (GameObject)Instantiate(projectTile, transform.position, Quaternion.Euler(rotation));

            clone.GetComponent<Rigidbody2D>().velocity = -transform.right * speedFactor;
        }
    }
}
