using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    //drag and drop the ground checking trigger (inside the player)
    public GroundCheckBehaviour groundChecker;

    //drag and drop the indicator in the scene
    public GameObject directionIndicator;
    
    //speed values for movement, normal jump, and bash
    public float movementSpeed;
    public float jumpForce;
    public float jumpForceInteractive;
    bool jump = false;

    //other private variables
    private Rigidbody2D rb2D;
    private Vector2 mouseWorldPosition;
    private Vector2 indicatorDirection;
    private float horizontalMovement;
    private bool isOnInteractive;

    //Animation
    public Animator animator;



    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckMousePosition();
        CheckIndicator();
        CheckMovement();
        CheckJump();
        CheckAnimation();
    }

    //this is to get the mouse position value in the world space (need to be used to calculate direction later on)
    private void CheckMousePosition()
    {
        Vector2 mousePos = Input.mousePosition;
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePos);
    }

    //this is to control the direction indicator
    private void CheckIndicator()
    {
        //set the indicator to true/false, depending on whether the player is on the interactive or not
        //if player is on interactive, set the direction indicator to true, and vice versa
        directionIndicator.SetActive(isOnInteractive);
        
        //make the indicator always follow the player position
        directionIndicator.transform.position = transform.position;

        //rotate the indicator based on the mouse position
        indicatorDirection = (mouseWorldPosition - (Vector2)transform.position).normalized;
        float angleValue = Mathf.Atan2(indicatorDirection.y, indicatorDirection.x) * Mathf.Rad2Deg;
        directionIndicator.transform.rotation = Quaternion.AngleAxis(angleValue, directionIndicator.transform.forward);
    }

    //This is for basic player left and right movement
    private void CheckMovement()
    {
        horizontalMovement = Input.GetAxis("Horizontal");
        transform.Translate(horizontalMovement * movementSpeed * Time.deltaTime, 0f, 0f);
        Vector3 characterScale = transform.localScale;
        if (isOnInteractive)
        {
            rb2D.velocity = Vector2.zero;
        }
        else
        {
            if(horizontalMovement < 0 )
            {
                characterScale.x = -0.5f;
            }
            if(horizontalMovement > 0)
            {
                characterScale.x = 0.5f;
            }
            transform.localScale = characterScale;
        }
    }

    //this is basic jump for the player with LEFT MOUSE BUTTON CLICK (can be replaced with spacebar)
    private void CheckJump()
    {
        if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0))
        {
            jump = true;
            Debug.Log("IsJumping");
            
            if (isOnInteractive == true)
            {
                //if player is on the interactive, set rigidbody back to dynamic and apply force towards the indicator direction
                rb2D.bodyType = RigidbodyType2D.Dynamic;
                rb2D.AddForce(indicatorDirection * jumpForceInteractive, ForceMode2D.Impulse);
                isOnInteractive = false;

                Debug.Log("click pls");
            }
            else if (groundChecker.isOnGround == true)
            {
                //if not, can only perform normal jump if the player is on the ground
                rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }

    private void CheckAnimation()
    {
        animator.SetBool("IsJumping", !groundChecker.isOnGround);
        animator.SetFloat("Speed", Mathf.Abs(horizontalMovement));
    }

    //this is basic trigger check to see if player is on the interactive object
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if player collide with an object with this InteractiveBehaviour
        if (collision.GetComponent<InteractiveBehaviour>() != null)
        {
            //set rigidbody to kinematic so that it does not react to physics (such as gravity)
            rb2D.bodyType = RigidbodyType2D.Kinematic;

            //isOnInteractive is then set to true to check the indicator, player movement, and player jump as done above
            isOnInteractive = true;
        }
    }
}