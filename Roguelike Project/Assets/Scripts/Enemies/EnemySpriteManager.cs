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

    void Update()
    {
        if (_clsEnemyMovement.moving)
            animator.enabled = true;
        else
            animator.enabled = false;
    }

    public void Death()
    {
        _clsEnemyMovement.moving = false;
        animator.enabled = false;
        sprRender.sprite = spDeath;
    }
}
    