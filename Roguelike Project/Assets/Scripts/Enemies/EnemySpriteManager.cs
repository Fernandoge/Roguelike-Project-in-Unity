using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemySpriteManager : SpriteManager
{
    [Header("This Components")]
    [SerializeField]
    private EnemyController clsEnemyController = default;
    [SerializeField]
    private EnemyWeapon _clsEnemyWeapon = default;
    //TODO: death particles should be handled in hitpoints manager
    [SerializeField]
    private ParticleSystem _deathParticles = default;

    private void Start()
    {
        //UpdateWeaponSprite(_clsEnemyWeapon.equippedWeapon.GetComponent<SpriteRenderer>().sprite);
    }

    public void Death()
    {
        GameManager.Instance.tilesLayers[(int)clsEnemyController.destinyPosition.x, (int)clsEnemyController.destinyPosition.y] = clsEnemyController.currentPositionOriginalLayer;
        ParticleSystem deathParticlesInstance = Instantiate(_deathParticles, transform.position, Quaternion.identity);
        Destroy(deathParticlesInstance.gameObject, deathParticlesInstance.main.startLifetimeMultiplier);
        Destroy(gameObject);
    }
}
    