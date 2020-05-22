using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public float hitpoints;
    public RoomController clsRoomController;
    [SerializeField]
    private ParticleSystem _destroyedParticles = default;

    public void AttackHit(float attackDamage)
    {
        hitpoints -= attackDamage;
        if (hitpoints <= 0)
        {
            if (gameObject.tag == "Enemy")
            {
                transform.parent.GetChild(1).GetComponent<EnemySpriteManager>().Death();
                gameObject.SetActive(false);
            }
            else
                Destroyed();
        }
    }

    public void SetRoomController(RoomController clsRoomController)
    {
        this.clsRoomController = clsRoomController;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.layer == LayerMask.NameToLayer("DestroyableObstacle") && collision.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            collision.GetComponent<PlayerController>().currentPositionOriginalLayer = LayerMask.NameToLayer("Floor");
            Destroyed();
        }
    }

    public void Destroyed()
    {
        GameManager.Instance.tilesLayers[(int)transform.position.x, (int)transform.position.y] = LayerMask.NameToLayer("Floor");
        Destroy(gameObject);
        ParticleSystem deathParticlesInstance = Instantiate(_destroyedParticles, transform.position, Quaternion.identity);
        Destroy(deathParticlesInstance.gameObject, deathParticlesInstance.main.startLifetimeMultiplier);
    }
}
