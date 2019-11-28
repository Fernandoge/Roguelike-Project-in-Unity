using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MovingObject
{

    public Transform target;
    public float stopDistance;
    public float distanceBetweenPlayer;
    

    protected override void Start()
    {
        GameManager.Instance.enemies.Add(this);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    public bool CheckEnemyMovement()
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon)
        {
            yDir = target.position.y > transform.position.y ? 1 : -1;
        }
        else
        {
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }

        return CheckNextMovement(xDir, yDir);
    }

    void Movement()
    {
        distanceBetweenPlayer = Vector2.Distance(transform.position, target.transform.position);
        //Debug.Log(distanceBetweenPlayer);
        rigidbody.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((target.transform.position.y - transform.position.y), (target.transform.position.x - transform.position.x)) * Mathf.Rad2Deg);
        if (distanceBetweenPlayer > stopDistance)
        {
            //transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            moving = true;
        }
        else
        {
            moving = false;
        }
        
    }
}
