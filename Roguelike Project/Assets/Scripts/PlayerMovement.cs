using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Sprite spLeft, spRight, spUp, spDown;
    public Rigidbody2D myRigidbody;
    public bool canMove;
    public bool moving;
    public float moveSpeed;
    private float moveSpeedAux;
    public float boost;

    // Use this for initialization
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        moveSpeedAux = moveSpeed;
    }


    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            //Horizontal Movement
            if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
            {
                moving = true;
                myRigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, myRigidbody.velocity.y);
                /*if (Input.GetAxisRaw("Horizontal") > 0.5f)
                    GetComponent<SpriteRenderer>().sprite = spRight;
                if (Input.GetAxisRaw("Horizontal") < -0.5f)
                    GetComponent<SpriteRenderer>().sprite = spLeft;*/
            }
            else
                myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y);

            //Vertical Movement
            if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
            {
                moving = true;
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, Input.GetAxisRaw("Vertical") * moveSpeed);
                /*if (Input.GetAxisRaw("Vertical") > 0.5f)
                    GetComponent<SpriteRenderer>().sprite = spUp;
                if (Input.GetAxisRaw("Vertical") < -0.5f)
                    GetComponent<SpriteRenderer>().sprite = spDown;*/
            }
            else
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0f);

            //Diagonal Normalizer
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f && Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.5f)
                moveSpeed = moveSpeedAux * 0.707f;
            else
                moveSpeed = moveSpeedAux;

            //Boost
            if (Input.GetKeyDown("space"))          
            {
                moveSpeedAux = moveSpeedAux * boost;
            }
            if (Input.GetKeyUp("space"))
            {
                moveSpeedAux = moveSpeedAux / boost; 
            }

        }

        //No movement
        if (Input.GetAxisRaw("Horizontal") < 0.5f && Input.GetAxisRaw("Horizontal") > -0.5f && Input.GetAxisRaw("Vertical") < 0.5f && Input.GetAxisRaw("Vertical") > -0.5f)   
        {
            moving = false;
        }

    }

    
}