using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteManager : SpriteManager
{
    [Header("This Components")]
    [SerializeField]
    private PlayerMovement _clsPlayerMovement = default;
    public GameObject directionSelector = default;
    public GameObject corridorParticles = default;

    new void Update()
    {
        if (_clsPlayerMovement.canMove)
        {
            if (_clsPlayerMovement.moving)
                animator.enabled = true;
            else
                animator.enabled = false;

            direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            base.Update();
        }
    }
}
