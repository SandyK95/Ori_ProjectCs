using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckBehaviour : MonoBehaviour
{
    //DO NOT NEED THIS IF YOU ALREADY HAVE A PROPER GROUND CHECKING SCRIPT ON YOUR SIDE

    //bool to check if the trigger below the player is touching ground
    public bool isOnGround;

    //getting the player object
    private GameObject ownerObject;

    private void Start()
    {
        //getting the player object, who is the parent
        ownerObject = transform.parent.gameObject;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //check if ground checking trigger is touching a solid surface, which is not the player himself
        if (collision.gameObject != ownerObject && collision.gameObject.GetComponent<Collider2D>().isTrigger == false)
        {
            //If so, the player is on a solid surface
            isOnGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //when the trigger leaves the solid surface
        if (collision.gameObject != ownerObject && collision.gameObject.GetComponent<Collider2D>().isTrigger == false)
        {
            //the player is no longer touching the ground
            isOnGround = false;
        }
    }
}