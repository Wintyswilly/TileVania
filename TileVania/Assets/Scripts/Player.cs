using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    //config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathkick = new Vector2(25f, 25f);
    //state
    bool isAlive = true;

    // cached component ref
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyColider;
    BoxCollider2D myFeet;
    float gravityScaleAtStart;
    // message the method
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyColider = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;
        myFeet = GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
     if(!isAlive) { return; }
       
        Run();
        FlipScript();
        Jump();
        ClimbingLadder();
        Die();
    }
    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        bool playerhorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerhorizontalSpeed);
    }
    private void ClimbingLadder() 
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        {
            myAnimator.SetBool("Climbing", false); ;
            myRigidBody.gravityScale = gravityScaleAtStart;
           return; 
        }

        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
        myRigidBody.velocity = climbVelocity;
        myRigidBody.gravityScale = 0f;
        bool playerMasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", playerMasVerticalSpeed);
    }
    private void Jump() 
    { 
        if(!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))){ return; }

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f,jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;

        }
    }

    private void Die() 
    {
        if (myBodyColider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            GetComponent<Rigidbody2D>().velocity = deathkick;
        }
    }

    private void FlipScript()
    {
        bool playerhorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if(playerhorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x),1);
        }

    }
}