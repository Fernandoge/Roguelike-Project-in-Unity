using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MovingObject
{
    public bool canMove;

    protected override void Start()
    {
        GameManager.Instance.player = this;
        base.Start();
    }

    void Update()
    {
        if (!GameManager.Instance.playersTurn) 
            return;

        int horizontal = 0;
        int vertical = 0;

        if (canMove)
        {
            if (SimpleInput.GetAxisRaw("Horizontal") > 0.5f)
                horizontal = 1;
            else if (SimpleInput.GetAxisRaw("Horizontal") < -0.5f)
                horizontal = -1;
            //delete this else for diagonal movement
            else if (SimpleInput.GetAxisRaw("Vertical") > 0.5f)
                vertical = 1;
            else if (SimpleInput.GetAxisRaw("Vertical") < -0.5f)
                vertical = -1;
        }

        if (horizontal != 0 || vertical != 0)
        {
            if (CheckNextMovement(horizontal, vertical))
            {
                Move();
                GameManager.Instance.playersTurn = false;
            }
        }
    }
}