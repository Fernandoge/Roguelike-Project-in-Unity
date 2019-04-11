using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteManager : MonoBehaviour
{
    public GameObject enemyScriptsObject;
    private EnemyMovement clsEnemyMovement;
    private EnemyWeapon clsEnemyweapon;
    public Sprite up, down, left, right, deathSprite;
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
        if (clsEnemyMovement.patrol)
        {
            switch (clsEnemyMovement.spriteOrder)
            {
                case 0:
                    SprRender.sprite = right;
                    break;
                case 1:
                    SprRender.sprite = down;
                    break;
                case 2:
                    SprRender.sprite = left;
                    break;
                case 3:
                    SprRender.sprite = up;
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
                    break;
                case 1:
                    SprRender.sprite = up;
                    break;
                case 2:
                    SprRender.sprite = left;
                    break;
                case 3:
                    SprRender.sprite = down;
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

    public void DeathAnimation()
    {
        clsEnemyMovement.pursuingPlayer = false;
        SprRender.sprite = deathSprite;
    }
}
