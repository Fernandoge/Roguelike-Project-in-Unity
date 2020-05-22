using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    [System.NonSerialized] public int currentPositionOriginalLayer;
    [System.NonSerialized] public Vector3 destinyPosition;
    public Rigidbody2D objRigidbody;
    public SpriteManager _clsSpriteManager;
    public LayerMask blockingLayers;
    public float moveTime;
    public bool canMove;
    public bool moving;
    private float inverseMoveTime;

    protected virtual void Start()
    {
        inverseMoveTime = 1f / moveTime;
    }

    protected abstract void Movement();

    public void AttemptMove()
    {
        if (moving)
            return;
        
        if (_clsSpriteManager.animator.enabled && _clsSpriteManager.sprRender.sprite == _clsSpriteManager.UpdateSpriteToDirection(_clsSpriteManager.direction))
            _clsSpriteManager.animator.enabled = false;

        Movement();
    }

    public void Move(int xDir, int yDir)
    {
        Vector3 positionToCheck = transform.position + new Vector3(xDir, yDir, 0f);
        //if (Physics2D.OverlapBox(_destiny, new Vector2(0.95f, 0.95f), 0f, blockingLayers) == null)
        if (((1 << GameManager.Instance.tilesLayers[(int)positionToCheck.x, (int)positionToCheck.y]) & blockingLayers) == 0)
        {
            destinyPosition = positionToCheck;
            GameManager.Instance.tilesLayers[(int)transform.position.x, (int)transform.position.y] = currentPositionOriginalLayer;
            currentPositionOriginalLayer = GameManager.Instance.tilesLayers[(int)destinyPosition.x, (int)destinyPosition.y];
            GameManager.Instance.tilesLayers[(int)destinyPosition.x, (int)destinyPosition.y] = gameObject.layer;

            if (gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                GameManager.Instance.playerDestinyPosition = destinyPosition;
            }
            GameManager.Instance.ResetPathfinding(this);

            moving = true;
            _clsSpriteManager.animator.enabled = true;
            _clsSpriteManager.CheckMovement(xDir, yDir);
            StartCoroutine(SmoothMovement(destinyPosition));
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
