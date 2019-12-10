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
        //UpdateWeaponSprite(_clsEnemyWeapon.equippedWeapon.GetComponent<SpriteRenderer>().sprite);
    }

    public void Death()
    {
        _clsEnemyMovement.enabled = false;
        animator.enabled = false;
        sprRender.sprite = spDeath;
    }
}
    