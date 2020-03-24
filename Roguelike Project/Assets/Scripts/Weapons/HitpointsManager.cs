using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitpointsManager : MonoBehaviour
{
    public float hitpoints;
    public RoomController clsRoomController;
    [SerializeField]
    private ParticleSystem _destroyedParticles = default;

    public void BulletHit(float bulletDamage)
    {
        hitpoints -= bulletDamage;
        if (hitpoints <= 0)
        {
            if (gameObject.tag == "Enemy")
            {
                transform.parent.GetChild(1).GetComponent<EnemySpriteManager>().Death();
                gameObject.SetActive(false);
                /*
                if (clsRoomController != null)
                    clsRoomController.EnemyKilled(gameObject);
                */
            }
            else
            {
                Destroy(gameObject);
            }  
        }
    }

    public void SetRoomController(RoomController clsRoomController)
    {
        this.clsRoomController = clsRoomController;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (CompareTag("GlobalWallAttached")) 
        {
            Destroy(gameObject);
            ParticleSystem deathParticlesInstance = Instantiate(_destroyedParticles, transform.position, Quaternion.identity);
            Destroy(deathParticlesInstance.gameObject, deathParticlesInstance.main.startLifetimeMultiplier);
        }
    }
}
