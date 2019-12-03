using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MovingObject
{
    protected override void Start()
    {
        GameManager.Instance.player = this;
        base.Start();
    }

    private void Update()
    {
        AttemptMove();
    }

    protected override void Movement()
    {
        if (canMove)
        {
            int horizontal = 0;
            int vertical = 0;

            if (SimpleInput.GetAxisRaw("Horizontal") > 0.5f)
                horizontal = 1;
            else if (SimpleInput.GetAxisRaw("Horizontal") < -0.5f)
                horizontal = -1;
            //delete this else for diagonal movement
            if (SimpleInput.GetAxisRaw("Vertical") > 0.5f)
                vertical = 1;
            else if (SimpleInput.GetAxisRaw("Vertical") < -0.5f)
                vertical = -1;

            if (horizontal != 0 || vertical != 0)
            {
                Move(horizontal, vertical);
            }
        }

        
    }
}