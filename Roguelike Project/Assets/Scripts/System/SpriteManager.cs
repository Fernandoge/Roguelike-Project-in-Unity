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
                sprRender.sprite = UpdateDirectionSprite(2);
        }
        else if (xDir == 1)
        {
            if (yDir == 1 && direction == 1)
                return;
            else if (yDir == -1 && direction == 3)
                return;
            else
                sprRender.sprite = UpdateDirectionSprite(0);
        }
        else if (yDir == 1)
            sprRender.sprite = UpdateDirectionSprite(1);
        else if (yDir == -1)
            sprRender.sprite = UpdateDirectionSprite(3);
    }

    public Sprite UpdateDirectionSprite(int direction)
    {
        this.direction = direction;
        animator.SetInteger("Direction", direction);
        Sprite sprite = null;
        switch (direction)
        {
            case 0:
                sprite = spRight; break;
            case 1:
                sprite = spUp; break;
            case 2:
                sprite = spLeft; break;
            case 3:
                sprite = spDown; break;
        }
        return sprite;
    }

    public void UpdateWeaponSprite(Sprite weaponSprite)
    {
        _weaponSprite = weaponSprite;
    }

}

