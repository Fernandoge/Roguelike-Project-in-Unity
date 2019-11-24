using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public SpriteManager _clsSpriteManager;
    public LayerMask blockingLayers;
    public float moveTime = 1000f;
    public GameObject[,] tiles;
    public Rigidbody2D _rigidbody;
    public bool moving;
    private float inverseMoveTime;

    protected virtual void Start()
    {
        inverseMoveTime = 1f / moveTime;
    }

    protected void Move(int xDir, int yDir)
    {
        Vector3 destiny = transform.position + new Vector3(xDir, yDir, 0f);
        if (Physics2D.Linecast(transform.position, destiny, blockingLayers) == false)
        {
            GameManager.Instance.playersTurn = false;
            moving = true;
            UpdateObjectSprite(xDir, yDir);
            StartCoroutine(SmoothMovement(destiny));
        }
    }

    protected IEnumerator SmoothMovement(Vector3 destiny)
    {
        float sqrRemainingDistance = (transform.position - destiny).sqrMagnitude;
        
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(_rigidbody.position, destiny, inverseMoveTime * Time.deltaTime);
            _rigidbody.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - destiny).sqrMagnitude;
            yield return null;
        }
        GameManager.Instance.playersTurn = true;
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
