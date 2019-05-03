using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteManager : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite up;
    public Sprite down;
    public Sprite left;
    public Sprite right;
    public Sprite deathSprite;
    [Header("Components")]
    public GameObject enemyScriptsObject;
    public Animator animator;
    private EnemyMovement clsEnemyMovement;
    private EnemyWeapon clsEnemyweapon;
    public SpriteRenderer SprRender;
    public SpriteRenderer EnemyWeaponRightSprRender, EnemyWeaponLeftSprRender;
    private Sprite _weaponSprite;

    void Start()
    {
        clsEnemyMovement = enemyScriptsObject.GetComponent<EnemyMovement>();
        clsEnemyweapon = enemyScriptsObject.GetComponent<EnemyWeapon>();
        _weaponSprite = clsEnemyweapon.equippedWeapon.GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {

        if (clsEnemyMovement.moving)
            animator.enabled = true;
        else
            animator.enabled = false;

        if (clsEnemyMovement.patrol)
        {
            switch (clsEnemyMovement.spriteOrder)
            {
                case 0:
                    SprRender.sprite = right;
                    animator.SetInteger("Direction", 0);
                    break;
                case 1:
                    SprRender.sprite = up;
                    animator.SetInteger("Direction", 1);
                    break;
                case 2:
                    SprRender.sprite = left;
                    animator.SetInteger("Direction", 2);
                    break;
                case 3:
                    SprRender.sprite = down;
                    animator.SetInteger("Direction", 3);
                    break;
            }
        }

        if (clsEnemyMovement.pursuingPlayer)
        {
            Vector2 direction = clsEnemyMovement.player.transform.position - clsEnemyMovement.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            int index = (int)((Mathf.Round(angle / 90f) + 4) % 4); //add a modulo over 4 to get a normalized index

            switch (index)
            {
                case 0:
                    SprRender.sprite = right;
                    animator.SetInteger("Direction", 0);
                    break;
                case 1:
                    SprRender.sprite = up;
                    animator.SetInteger("Direction", 1);
                    break;
                case 2:
                    SprRender.sprite = left;
                    animator.SetInteger("Direction", 2);
                    break;
                case 3:
                    SprRender.sprite = down;
                    animator.SetInteger("Direction", 3);
                    break;
            }
          
            if (angle > -90 && angle < 90)
            {
                EnemyWeaponLeftSprRender.sprite = null;
                EnemyWeaponRightSprRender.sprite = _weaponSprite;
            }
            else
            {
                EnemyWeaponRightSprRender.sprite = null;
                EnemyWeaponLeftSprRender.sprite = _weaponSprite;
            }
        }

    }

    public void EnemyDeathAnimation()
    {
        animator.enabled = false;
        clsEnemyMovement.pursuingPlayer = false;
        SprRender.sprite = deathSprite;
    }
}
    