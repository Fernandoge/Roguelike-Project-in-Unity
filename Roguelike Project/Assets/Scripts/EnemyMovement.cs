using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    GameObject player;
    public bool patrol = true, gaurd = false, clockwise = false;
    public bool moving = true;
    public bool pursuingPlayer = false, goingToLastLoc = false;
    Vector3 target;
    Rigidbody2D rid;
    public Vector3 playerLastPos;
    RaycastHit2D hit;
    float speed = 2.0f; //changed bullets to be kenimatic
    int layerMask = 1 << 0; //explain layermask for tutorial (how it works + changes to weapon attack)


    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerLastPos = this.transform.position;
        //hit=Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(dir.x, dir.y));
        rid = this.GetComponent<Rigidbody2D>();
        layerMask = ~layerMask;
	}
	
	// Update is called once per frame
	void Update () {
        movement();
        playerDetect();
		
	}

    void movement()
    {
        float dist = Vector3.Distance(player.transform.position, this.transform.position);
        Vector3 dir = player.transform.position - transform.position;
        hit = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(dir.x, dir.y), dist, layerMask);
        Debug.DrawRay(transform.position, dir, Color.red);

        Vector3 fwt = this.transform.TransformDirection(Vector3.right);
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(dir.x, dir.y), dist, layerMask);
        Debug.DrawRay(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(fwt.x, fwt.y), Color.cyan);

        if (moving == true)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        if (patrol == true)
        {
            Debug.Log("Patrolling normally");
            speed = 2.0f;

            if (hit2.collider != null)
            {
                //Debug2.LogError(hit2.collider.tag);
                if (hit2.collider.gameObject.tag == "Wall")
                {
                    //Quaternion rot = this.transform.rotation;

                    if (clockwise == false)
                    {
                        transform.Rotate(0, 0, 90);

                    }
                    else
                    {
                        transform.Rotate(0, 0, -90);
                    }
                }
            }
        }

        if (pursuingPlayer == true)
        {
            //transform.Translate(Vector3.right * speed * Time.deltaTime);
            Debug.Log("Pursuing Player");
            speed = 3.5f;
            rid.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((playerLastPos.y - transform.position.y), (playerLastPos.x - transform.position.x)) * Mathf.Rad2Deg);
            if (Vector3.Distance(this.transform.position, playerLastPos) < 1.5f)
            {
                //not found player, return to patrol
                patrol = true;
                goingToLastLoc = false;
            }
        }

    }

    void playerDetect()
    {
        Vector3 pos = this.transform.InverseTransformPoint(player.transform.position);
        //Debug.Log (pos.x); //more than 1.2

        if(hit.collider!=null)
        {
            //Debug.LogError(hit.collider.tag);
            if(hit.collider.gameObject.tag == "Player" && pos.x >1.2f && Vector3.Distance(this.transform.position, player.transform.position) < 9)
            {
                patrol = false;
                pursuingPlayer = true;
            }
            else
            {
                if (pursuingPlayer==true)
                {
                    goingToLastLoc = true;
                    pursuingPlayer = false;
                }
                //pursuingPlayer = false;
            }
        }
    }

}
