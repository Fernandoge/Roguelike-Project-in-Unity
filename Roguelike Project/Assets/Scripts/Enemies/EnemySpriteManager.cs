using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteManager : SpriteManager
{

    [Header("This Components")]
    [SerializeField]
    private EnemyMovement _clsEnemyMovement = default;
    [SerializeField]
    private EnemyWeapon _clsEnemyWeapon = default;

    private void Start()
    {
        UpdateWeaponSprite(_clsEnemyWeapon.equippedWeapon.GetComponent<SpriteRenderer>().sprite);
    }

    new void Update()
    {
        if (_clsEnemyMovement.moving)
            animator.enabled = true;
        else
            animator.enabled = false;

        if (_clsEnemyMovement.patrol)
        {
            switch (_clsEnemyMovement.spriteOrder)
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
        }

        if (_clsEnemyMovement.pursuingPlayer)
        {
            direction = _clsEnemyMovement.player.transform.position - _clsEnemyMovement.transform.position;
            base.Update();
        }
    }

    public void Death()
    {
        _clsEnemyMovement.moving = false;
        _clsEnemyMovement.pursuingPlayer = false;
        animator.enabled = false;
        sprRender.sprite = spDeath;
    }
}
    