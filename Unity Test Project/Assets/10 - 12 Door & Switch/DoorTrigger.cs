using UnityEngine;
using System.Collections;

public class DoorTrigger : MonoBehaviour {

	public DoorScript door;


	public bool ignoreTrigger;
	public bool isOpen = false;


	void OnTriggerEnter2D(Collider2D other){

		if (ignoreTrigger)
						return;

		if (other.tag == "Follower" && isOpen == false)
        {
			door.DoorOpens();
			isOpen = true;
		}

	}

	void OnTriggerExit2D(Collider2D other){


		if (ignoreTrigger)
			return;

		if (other.tag == "Player")
			door.DoorCloses();
		
	}

	public void Toggle(bool state)
	{
		if (state)
						door.DoorOpens ();
				else
						door.DoorCloses ();
		}


	void OnDrawGizmos()
	{
		if (!ignoreTrigger) {
			BoxCollider2D box = GetComponent<BoxCollider2D>();

			Gizmos.DrawWireCube(transform.position, new Vector2(box.size.x,box.size.y));

				}


	}
}
