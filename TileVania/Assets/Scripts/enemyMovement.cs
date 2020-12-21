using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D myRigidBoddy;
    void Start()
    {
        myRigidBoddy = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsfacingRight())
        {
            myRigidBoddy.velocity = new Vector2(moveSpeed, 0f);

        }
        else
        {
            myRigidBoddy.velocity = new Vector2(-moveSpeed, 0f);
        }

    }
    bool IsfacingRight() 
    {
        return transform.localScale.x > 0;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidBoddy.velocity.x)), 1f);
    }
}