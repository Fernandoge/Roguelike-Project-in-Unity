using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    [System.NonSerialized] public int currentPositionOriginalLayer;
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
        else
        {
            if (_clsSpriteManager.animator.enabled && _clsSpriteManager.sprRender.sprite == _clsSpriteManager.UpdateDirectionSprite(_clsSpriteManager.direction))
                _clsSpriteManager.animator.enabled = false;
        }

        Movement();
    }

    public void Move(int xDir, int yDir)
    {
        Vector3 _destiny = transform.position + new Vector3(xDir, yDir, 0f);
        //if (Physics2D.OverlapBox(_destiny, new Vector2(0.95f, 0.95f), 0f, blockingLayers) == null)
        if (((1 << GameManager.Instance.tilesLayers[(int)_destiny.x, (int)_destiny.y]) & blockingLayers) == 0)
        {
            GameManager.Instance.tilesLayers[(int)transform.position.x, (int)transform.position.y] = currentPositionOriginalLayer;
            currentPositionOriginalLayer = GameManager.Instance.tilesLayers[(int)_destiny.x, (int)_destiny.y];
            GameManager.Instance.tilesLayers[(int)_destiny.x, (int)_destiny.y] = gameObject.layer;

            moving = true;
            _clsSpriteManager.animator.enabled = true;
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
