using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public SpriteManager _clsSpriteManager;
    public LayerMask blockingLayers;
    public float moveTime;
    public GameObject[,] tiles;
    public Rigidbody2D rigidbody;
    public Collider2D collider;
    private Vector2 _auxColliderOffset;
    [System.NonSerialized] public bool validMove;
    private Vector3 _destiny;
    public bool moving;
    private float inverseMoveTime;

    protected virtual void Start()
    {
        inverseMoveTime = 1f / moveTime;
        _auxColliderOffset = collider.offset;
    }

    protected bool CheckNextMovement(int xDir, int yDir)
    {
        _destiny = transform.position + new Vector3(xDir, yDir, 0f);
        if (Physics2D.Linecast(transform.position, _destiny, blockingLayers) == false)
        {
            collider.offset += new Vector2(xDir, yDir);
            UpdateObjectSprite(xDir, yDir);
            return true;
        }
        else
            return false;
    }

    public void Move()
    {     
        moving = true;
        collider.offset = _auxColliderOffset;
        StartCoroutine(SmoothMovement(_destiny));
    }

    protected IEnumerator SmoothMovement(Vector3 destiny)
    {
        float sqrRemainingDistance = (transform.position - destiny).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rigidbody.position, destiny, inverseMoveTime * Time.deltaTime);
            rigidbody.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - destiny).sqrMagnitude;
            yield return null;
        }
        moving = false;
    }

    private void UpdateObjectSprite(int xDir, int yDir)
    {
        if (xDir == -1)
        {
            if (yDir == 1 && _clsSpriteManager.direction == 1)
                return;
            else if (yDir == -1 && _clsSpriteManager.direction == 3)
                return;
            else
                _clsSpriteManager.UpdateSpriteDirection(2);
        }
        else if (xDir == 1)
        {
            if (yDir == 1 && _clsSpriteManager.direction == 1)
                return;
            else if (yDir == -1 && _clsSpriteManager.direction == 3)
                return;
            else
                _clsSpriteManager.UpdateSpriteDirection(0);
        }
        else if (yDir == 1)
            _clsSpriteManager.UpdateSpriteDirection(1);
        else if (yDir == -1)
            _clsSpriteManager.UpdateSpriteDirection(3);
    }
}
