using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    //drag and drop the ground checking trigger (inside the player)
    public GroundCheckBehaviour groundChecker;

    //drag and drop the indicator in the scene
    public GameObject directionIndicator;

    //Player Speed for movement
    public float movementSpeed;
    public float runMultiplier;
    float speed = 5f;
    public GameObject runParticles;
    float maxVelocity = 10f;
    float force;

    //Normal jump, and bash
    public float jumpForce;
    public float jumpForceInteractive;

    bool canJump;
    public float timer;
    public float maxTime = 0.1f;
    public int curJump;
    public int TotalJumps = 2;

    public GameObject jumpsParticles;
    public Transform playerfeet;


    //other private variables
    private Rigidbody2D rb2D;
    private Vector2 mouseWorldPosition;
    private Vector2 indicatorDirection;
    private float horizontalMovement;
    float previousAxispos;
    private bool isOnInteractive;

    //Animation
    public Animator animator;

    //Player dead
    bool isDead;
    public GameObject restartText;

    //Player Lives
    public int lives;
    public bool isImmune;
    public float immuneTime;

    //Air Control
    private Vector2 m_WindVelocity = Vector2.zero;
    public float airControl = 0.5f;

    //Shooting
    public GameObject m_Bullet;

    //GroundState
    public enum GroundState 
    { 
        isGrounded, isJumping 
    };

    GroundState m_GroundState;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        //trailing effect
        //InvokeRepeating("SpawnTrailPart", 0, 0.2f);
    }

    private void Update()
    {
        CheckMousePosition();
        CheckIndicator();
        CheckMovement();
        CheckJump();
        Bash();
        Shooting();
        CheckAnimation();
        //SpawnTrailPart();
        switch (m_GroundState)
        {
            case GroundState.isGrounded: airControl = 1.0f; break;
            case GroundState.isJumping: airControl = 0.2f; break;
        }

        CheckonSprint();
        CheckPlayerDead();

    }


    void SwitchStates(GroundState groundstates)
    {
        groundstates = m_GroundState;
    }

    IEnumerator ImmuneTime()
    {
        yield return new WaitForSeconds(immuneTime);
        isImmune = false;

        //animator hurt
    }

    private void CheckPlayerDead()
    {
        if(isDead == true)
        {
            restartText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            return;
        }
    }


    private void CheckonSprint()
    {
        speed = Input.GetKey(KeyCode.LeftShift) ? movementSpeed * runMultiplier : movementSpeed;

        if (Input.GetKey(KeyCode.LeftShift) && rb2D.velocity.x != 0 && groundChecker.isOnGround)
        {
            Debug.Log("test, Speed: " + speed + " Velocity: " + rb2D.velocity.x + "Ground Bool: " + groundChecker.isOnGround);
            runParticles.SetActive(true);
            //RUN ANIMATE TRUE --> CheckAnimation
        }
        else
            runParticles.SetActive(false);
            
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
        force = horizontalMovement * speed * rb2D.mass * airControl;
        SwitchStates(GroundState.isGrounded);
        if (rb2D.velocity.x > maxVelocity || rb2D.velocity.x < -maxVelocity)
        {
            force = 0;
        }
        if (horizontalMovement != 0)
        {
            if (isOnInteractive == false)
            {
                rb2D.AddForce(new Vector2(force, 0));
            }
            if (horizontalMovement > 0)
            {
                transform.localScale = new Vector3(0.5f, 0.5f, 1);
            }
            else if (horizontalMovement < 0)
            {
                transform.localScale = new Vector3(-0.5f, 0.5f, 1);
            }
            //if (groundChecker.isOnGround == true)
            //{
            //    if (horizontalMovement > 0)
            //    {
            //        transform.localScale = new Vector3(0.5f, 0.5f, 1);
            //    }
            //    else if (horizontalMovement < 0)
            //    {
            //        transform.localScale = new Vector3(-0.5f, 0.5f, 1);
            //    }
            //}
            //else
            //{
            //    if (rb2D.velocity.x > 0)
            //    {
            //        transform.localScale = new Vector3(0.5f, 0.5f, 1);
            //    }
            //    else if (rb2D.velocity.x < 0)
            //    {
            //        transform.localScale = new Vector3(-0.5f, 0.5f, 1);
            //    }
            //}
        }
        else if (groundChecker.isOnGround == true && isOnInteractive == false)
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
        }
    }

    //this is basic jump for the player with LEFT MOUSE BUTTON CLICK (can be replaced with spacebar)
    private void CheckJump()
    {
        SwitchStates(GroundState.isJumping);
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (groundChecker.isOnGround)
            {
                //if not, can only perform normal jump if the player is on the ground
                rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                curJump = TotalJumps;
                timer = 0;
                canJump = true;
                Debug.Log("First Jump");
            }
            else if (curJump > 0)
            {
                rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                Instantiate(jumpsParticles, playerfeet.position, Quaternion.identity);
                --curJump;
                timer = 0;
                canJump = true;
                Debug.Log("Second Jump");
            }
        }
            
    }

    void Bash()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isOnInteractive == true)
            {
                //if player is on the interactive, set rigidbody back to dynamic and apply force towards the indicator direction
                rb2D.bodyType = RigidbodyType2D.Dynamic;
                rb2D.AddForce(indicatorDirection * jumpForceInteractive, ForceMode2D.Impulse);
                isOnInteractive = false;
            }
        }
    }

    void Shooting()
    {
        if (Input.GetKeyDown("z")) Instantiate(m_Bullet, transform.position, Quaternion.identity);
    }

    private void CheckAnimation()
    {
        animator.SetBool("IsJumping", !groundChecker.isOnGround);
        animator.SetFloat("Speed", Mathf.Abs(horizontalMovement));
        //animator dead
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "deadly" && !isDead && lives <= 1 && !isImmune)
        {
            rb2D.velocity = Vector2.zero;
            lives = 0;

            //Animation dead
            animator.SetBool("Dies", true);
            rb2D.AddForce(new Vector2(0, 500));
            isDead = true;
            Debug.Log("Trigger: dead");
        }

        else if (collision.tag == "deadly" && lives > 1 && !isImmune)
        {
            lives--;
            //animator immune
            StartCoroutine("ImmuneTime");
            isImmune = true;
            Debug.Log("2 Trigger: live minus");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "deadly" && !isDead && lives <= 1 && !isImmune)
        {
            rb2D.velocity = Vector2.zero;
            lives = 0;

            //Animation dead
            animator.SetBool("Dies", true);
            rb2D.AddForce(new Vector2(0, 500));
            isDead = true;
            Debug.Log("Trigger: dead");
        }

        else if (collision.gameObject.tag == "deadly" && lives > 1 && !isImmune)
        {
            lives--;
            StartCoroutine("ImmuneTime");
            //animator immune
            isImmune = true;
            Debug.Log("1 Trigger: live minus");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if player collide with an object with this InteractiveBehaviour
        if (collision.GetComponent<InteractiveBehaviour>() != null && isOnInteractive == false)
        {
            //isOnInteractive is then set to true to check the indicator, player movement, and player jump as done above
            rb2D.bodyType = RigidbodyType2D.Kinematic;
            rb2D.velocity = Vector2.zero;
            isOnInteractive = true;
        }
    }


        //Unnecessary
        void SpawnTrailPart()
    {
        GameObject trailPart = new GameObject();
        SpriteRenderer trailPartRenderer = trailPart.AddComponent<SpriteRenderer>();
        trailPartRenderer.sprite = GetComponent<SpriteRenderer>().sprite;
        trailPart.transform.position = transform.position;
        trailPart.transform.localScale = transform.localScale; // We forgot about this line!!!

        StartCoroutine(FadeTrailPart(trailPartRenderer));
        Destroy(trailPart, 0.5f); // replace 0.5f with needed lifeTime
    }

    IEnumerator FadeTrailPart(SpriteRenderer trailPartRenderer)
    {
        Color color = trailPartRenderer.color;
        color.a -= 0.5f; // replace 0.5f with needed alpha decrement
        trailPartRenderer.color = color;

        yield return new WaitForEndOfFrame();
    }


}

////this is basic trigger check to see if player is on the interactive object
//private void OnTriggerEnter2D(Collider2D collision)
//{
//    //if player collide with an object with this InteractiveBehaviour
//    if (collision.GetComponent<InteractiveBehaviour>() != null && isOnInteractive == false)
//    {
//        //isOnInteractive is then set to true to check the indicator, player movement, and player jump as done above
//        rb2D.bodyType = RigidbodyType2D.Kinematic;
//        rb2D.velocity = Vector2.zero;
//        isOnInteractive = true;
//    }

//    if (collision.tag == "deadly" && !isDead && lives <= 1 && !isImmune)
//    {
//        rb2D.velocity = Vector2.zero;

//        //Animation dead
//        animator.SetBool("Dies", true);
//        rb2D.AddForce(new Vector2(0, 500));
//        isDead = true;
//        Debug.Log("Trigger: dead");
//    }

//}

//private void OnCollisionEnter2D(Collision2D collision)
//{
//    if(collision.gameObject.tag == "deadly" && !isDead && lives <=1 && !isImmune)
//    {
//        rb2D.velocity = Vector2.zero;
//        animator.SetBool("Dies", true);

//        rb2D.AddForce(new Vector2(0, 500));
//        isDead = true;

//        Debug.Log("Collision stay: dead");
//    }
//    else if (collision.gameObject.tag == "deadly" && lives > 1 && !isImmune)
//    {
//        lives--;
//        //animator immune
//        isImmune = true;

//        Debug.Log("Collision stay: lives minus" + lives);
//    }
//}
