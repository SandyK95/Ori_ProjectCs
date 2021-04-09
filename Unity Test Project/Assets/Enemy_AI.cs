using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    Animator m_Animator;
    string m_CurrAnim;
    Rigidbody2D     m_RB;
    Vector2         m_Force = Vector2.zero;
    int             m_IndexCheckPoint = 0;
    Vector3[]       m_Points = new Vector3[2];
    float           m_SquashTimer = -1;

    const float MAX_SPEED = 3;
    const float SPEED_FORCE = 400;
    const string ANIM_LEFT = "Enemy_Left";
    const string ANIM_RIGHT = "Enemy_Right";
    const string ANIM_BEEN_SQUASH = "Enemy_BeenSquashed";
    const string ANIM_PUSH_PLAYER = "Enemy_PushPlayer";

    // Start is called before the first frame update
    void Start()
    {
        m_RB = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        ChangeAnimationState(ANIM_LEFT);

        m_Points[0] = transform.position;
        m_Points[0].x -= 5;

        m_Points[1] = transform.position;
        m_Points[1].x -= 5;
        
    }

    public void ChangeAnimationState(string Anim)
    {
        if (Anim == m_CurrAnim) return;
        m_CurrAnim = Anim;
        m_Animator.Play(m_CurrAnim);
    }

    //DetermineAnimation()

    void DetermineGold()
    {
        float Vx = m_Points[m_IndexCheckPoint].x - transform.position.x;
        if (Mathf.Abs(Vx) < 1.3f)
        {
            m_IndexCheckPoint++;
            if (m_IndexCheckPoint == m_Points.Length) m_IndexCheckPoint = 0;
            DetermineGold();
        }
        else
        {
            if (m_RB.velocity.magnitude > MAX_SPEED && (m_RB.velocity.x * Vx) > 0) return;

            if (Vx > 0) m_Force.x += Time.deltaTime * m_RB.mass * SPEED_FORCE;
            else m_Force.x -= Time.deltaTime * m_RB.mass * SPEED_FORCE;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(m_SquashTimer < 0)
        {
            DetermineGold();
            //DetermineAnimation();
        }

        else
        {
            m_SquashTimer += Time.deltaTime;
            if(m_SquashTimer > 0.5f && m_SquashTimer<1)
            {
                GetComponent<CapsuleCollider2D>().enabled = false; ;
                GetComponents<BoxCollider2D>()[1].enabled = true;

                m_SquashTimer = 10;
            }
        }
    }

    private void FixedUpdate()
    {
        m_RB.AddForce(m_Force);
        m_Force = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var RB = collision.attachedRigidbody;

        Vector2 F;
        F.x = RB.velocity.x > 0 ? 300 : -300;
        F.y = 500;
        F *= RB.mass;

        RB.AddForce(F);

        if(m_SquashTimer < 0)
        {
            m_SquashTimer = 0;
            ChangeAnimationState(ANIM_BEEN_SQUASH);
        }
        else
        {
            m_CurrAnim = string.Empty;
            ChangeAnimationState(ANIM_PUSH_PLAYER);
        }
    }
}
