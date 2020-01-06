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
    [SerializeField]
    private ParticleSystem _deathParticles = default;

    private void Start()
    {
        //UpdateWeaponSprite(_clsEnemyWeapon.equippedWeapon.GetComponent<SpriteRenderer>().sprite);
    }

    public void Death()
    {
        GameManager.Instance.tilesLayers[(int)transform.position.x, (int)transform.position.y] = 0;
        ParticleSystem deathParticlesInstance = Instantiate(_deathParticles, transform.position, Quaternion.identity);
        Destroy(deathParticlesInstance.gameObject, deathParticlesInstance.main.startLifetimeMultiplier);
        Destroy(gameObject);
    }
}
    