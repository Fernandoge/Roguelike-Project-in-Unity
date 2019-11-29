using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite spUp;
    public Sprite spDown;
    public Sprite spLeft;
    public Sprite spRight;
    public Sprite spDeath;

    [Header("Components")]
    public Animator animator;
    public SpriteRenderer sprRender;
    public SpriteRenderer weaponRightSprRender, weaponLeftSprRender;
    private Sprite _weaponSprite;

    public int direction;

    public void CheckMovement(int xDir, int yDir)
    {
        if (xDir == -1)
        {
            if (yDir == 1 && direction == 1)
                return;
            else if (yDir == -1 && direction == 3)
                return;
            else
                UpdateSpriteDirection(2);
        }
        else if (xDir == 1)
        {
            if (yDir == 1 && direction == 1)
                return;
            else if (yDir == -1 && direction == 3)
                return;
            else
                UpdateSpriteDirection(0);
        }
        else if (yDir == 1)
            UpdateSpriteDirection(1);
        else if (yDir == -1)
            UpdateSpriteDirection(3);
    }

    public void UpdateSpriteDirection(int direction)
    {
        this.direction = direction;
        animator.SetInteger("Direction", direction);
        switch (direction)
        {
            case 0:
                sprRender.sprite = spRight; break;
            case 1:
                sprRender.sprite = spUp; break;
            case 2:
                sprRender.sprite = spLeft; break;
            case 3:
                sprRender.sprite = spDown; break;
        }
    }

    public void UpdateWeaponSprite(Sprite weaponSprite)
    {
        _weaponSprite = weaponSprite;
    }

}

