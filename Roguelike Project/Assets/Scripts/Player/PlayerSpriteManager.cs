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
    private GameObject joystickThumb;

    private void Awake()
    {
        joystickThumb = GameObject.FindGameObjectWithTag("GameController");
    }

    new void Update()
    {
        if (_clsPlayerMovement.canMove)
        {
            if (_clsPlayerMovement.moving)
                animator.enabled = true;
            else
                animator.enabled = false;

            //direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            if (joystickThumb.transform.localPosition.x != 0f && joystickThumb.transform.localPosition.y != 0f)
            {
                direction = joystickThumb.transform.localPosition;
            }   
            base.Update();
        }
    }
}
