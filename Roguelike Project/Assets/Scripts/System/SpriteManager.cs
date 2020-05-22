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
        switch (xDir)
        {
            case -1:
            case 1:
                switch (yDir)
                {
                    case 1 when direction == 1:
                    case -1 when direction == 3:
                        return;
                    default:
                        var horizontalPos = xDir == 1 ? UpdateSpriteToDirection(0) : UpdateSpriteToDirection(2); break;
                }
                break;

            default:
                var verticalPos = yDir == 1 ? UpdateSpriteToDirection(1) : UpdateSpriteToDirection(3);
                break;
        }
    }

    public Sprite UpdateSpriteToDirection(int direction)
    {
        this.direction = direction;
        animator.SetInteger("Direction", direction);
        Sprite sprite = null;
        switch (direction)
        {
            case 0:
                sprRender.sprite = spRight;
                break;
            case 1:
                sprRender.sprite = spUp;
                break;
            case 2:
                sprRender.sprite = spLeft;
                break;
            case 3:
                sprRender.sprite = spDown;
                break;
        }
        return sprRender.sprite;
    }

    public void UpdateWeaponSprite(Sprite weaponSprite)
    {
        _weaponSprite = weaponSprite;
    }

}

