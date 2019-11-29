using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public Rigidbody2D objRigidbody;
    public SpriteManager _clsSpriteManager;
    public LayerMask blockingLayers;
    public float moveTime;
    public bool canMove;
    public bool moving;
    public GameObject[,] tiles;
    private float inverseMoveTime;

    protected virtual void Start()
    {
        inverseMoveTime = 1f / moveTime;
    }

    protected abstract void Movement();

    protected void AttemptMove()
    {
        if (moving)
            return;

        Movement();
    }

    public void Move(int xDir, int yDir)
    {
        Vector3 _destiny = transform.position + new Vector3(xDir, yDir, 0f);
        if (Physics2D.OverlapBox(_destiny, new Vector2(0.95f, 0.95f), 0f, blockingLayers) == null)
        {
            moving = true;
            _clsSpriteManager.CheckMovement(xDir, yDir);
            StartCoroutine(SmoothMovement(_destiny));
        }  
    }

    protected IEnumerator SmoothMovement(Vector3 destiny)
    {
        float sqrRemainingDistance = (transform.position - destiny).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(objRigidbody.position, destiny, inverseMoveTime * Time.deltaTime);
            objRigidbody.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - destiny).sqrMagnitude;
            yield return null;
        }
        moving = false;
    }
}
