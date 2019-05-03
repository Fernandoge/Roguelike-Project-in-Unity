using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public GameObject player;
    public float speed;
    public float patrolSpeed;
    public float pursuingSpeed;
    public float stopDistance;
    public float distanceBetweenPlayer;
    public bool patrol = true, clockwise = false, pursuingPlayer = false;
    public int spriteOrder = 0;
    Rigidbody2D rigBody;
    public Vector3 playerLastPos;
    RaycastHit2D DetectionRaycast;
    private Vector2 velocity;
    public bool moving;
    int layerMask = 1 << 8; //explain layermask for tutorial (how it works + changes to weapon attack)


    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //hit=Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(dir.x, dir.y));
        rigBody = this.GetComponent<Rigidbody2D>();
        layerMask = ~layerMask;
        speed = patrolSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        if (patrol == true)
        {
            PlayerDetect();
        }

    }

    void Movement()
    {
        float dist = Vector3.Distance(player.transform.position, this.transform.position);
        Vector3 dir = player.transform.position - transform.position;
        DetectionRaycast = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(dir.x, dir.y), dist, layerMask);
        Debug.DrawRay(transform.position, dir, Color.red);
        
        Vector3 fwt = this.transform.TransformDirection(Vector3.right);
        RaycastHit2D ColliderRaycast = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(fwt.x, fwt.y), 1.0f, layerMask);
        Debug.DrawRay(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(fwt.x, fwt.y), Color.cyan);

        if (patrol == true)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            if (ColliderRaycast.collider != null)
            {
                //Debug2.LogError(hit2.collider.tag);
                if (ColliderRaycast.collider.gameObject.tag == "Wall" || ColliderRaycast.collider.gameObject.tag == "Destroyable")
                {
                    //Quaternion rot = this.transform.rotation;
                    if (clockwise == false)
                    {
                        transform.Rotate(0, 0, 90);
                        if (spriteOrder == 3)
                            spriteOrder = 0;
                        else
                            spriteOrder++;
                    }
                    else
                    {
                        transform.Rotate(0, 0, -90);
                        if (spriteOrder == 0)
                            spriteOrder = 3;
                        else
                            spriteOrder--;
                    }
                }
            }
        }

        if (pursuingPlayer == true)
        {
            velocity = new Vector2(speed, speed);
            distanceBetweenPlayer = Vector2.Distance(transform.position, player.transform.position);
            //Debug.Log(distanceBetweenPlayer);
            rigBody.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((player.transform.position.y - transform.position.y), (player.transform.position.x - transform.position.x)) * Mathf.Rad2Deg);
            if (distanceBetweenPlayer > stopDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                moving = true;
            }
            else
            {
                moving = false;
            }
        }
    }

    void PlayerDetect()
    {
        Vector3 pos = this.transform.InverseTransformPoint(player.transform.position);
        //Debug.Log (Vector3.Distance(transform.position, player.transform.position)); //more than 1.2
        if (DetectionRaycast.collider != null)
        {
            //Debug.LogError(hit.collider.tag);
            if (DetectionRaycast.collider.gameObject.tag == "Player" && pos.x > 1.2f && (Vector3.Distance(transform.position, player.transform.position) < 13))
            {
                speed = pursuingSpeed;
                rigBody.bodyType = RigidbodyType2D.Dynamic;
                rigBody.simulated = true;
                patrol = false;
                pursuingPlayer = true;
            }
        }
    }
}
