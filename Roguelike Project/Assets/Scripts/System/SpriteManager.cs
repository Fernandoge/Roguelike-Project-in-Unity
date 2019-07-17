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

    [System.NonSerialized]
    public Vector2 direction;

    public void Update()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        int index = (int)((Mathf.Round(angle / 90f) + 4) % 4); //add a modulo over 4 to get a normalized index

        switch (index)
        {
            case 0:
                sprRender.sprite = spRight;
                animator.SetInteger("Direction", 0);
                break;
            case 1:
                sprRender.sprite = spUp;
                animator.SetInteger("Direction", 1);
                break;
            case 2:
                sprRender.sprite = spLeft;
                animator.SetInteger("Direction", 2);
                break;
            case 3:
                sprRender.sprite = spDown;
                animator.SetInteger("Direction", 3);
                break;
        }

        if (angle > -90 && angle < 90)
        {
            weaponLeftSprRender.sprite = null;
            weaponRightSprRender.sprite = _weaponSprite;
        }
        else
        {
            weaponRightSprRender.sprite = null;
            weaponLeftSprRender.sprite = _weaponSprite;
        }

    }

    public void UpdateWeaponSprite(Sprite weaponSprite)
    {
        _weaponSprite = weaponSprite;
    }

}

