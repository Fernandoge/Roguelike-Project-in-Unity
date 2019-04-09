using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteManager : MonoBehaviour
{

    public Sprite spLeft, spRight, spUp, spDown;
    public SpriteRenderer playerSprRender;
    public Animator animator;
    public PlayerMovement clsPlayerMovement;
    public SpriteRenderer playerWeaponSprRender;

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        int index = (int)((Mathf.Round(angle / 90f) + 4) % 4); //add a modulo over 4 to get a normalized index

        if (clsPlayerMovement.moving)
            animator.enabled = true;
        else
            animator.enabled = false;

        switch (index)
        {
            case 0:
                playerSprRender.sprite = spRight;
                animator.SetInteger("Direction", 0);
                break;
            case 1:
                playerSprRender.sprite = spUp;
                animator.SetInteger("Direction", 1);
                break;
            case 2:
                playerSprRender.sprite = spLeft;
                animator.SetInteger("Direction", 2);
                break;
            case 3:
                playerSprRender.sprite = spDown;
                animator.SetInteger("Direction", 3);
                break;
        }
        
}
