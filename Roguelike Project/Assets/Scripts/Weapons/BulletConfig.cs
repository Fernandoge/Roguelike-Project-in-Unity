using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletConfig : MonoBehaviour {
    public Vector3 direction;
    public float bulletSpeed;
    public float bulletDamage;
    public int creatorLayer;
    HitpointsManager clsHitpointsManager;
    public GameObject bloodImpact, wallImpact;
    public float bulletDuration;
    public GameObject target;
    Rigidbody2D rb;
    Vector2 moveDirection;
    // Use this for initialization
    void Start ()
    {
        Destroy(gameObject, bulletDuration);
	}

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * bulletSpeed * Time.deltaTime);
    }

    public void SetVals (Vector3 dir, int shooterLayer)
    {
        direction = dir;
        creatorLayer = shooterLayer;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (creatorLayer == LayerMask.NameToLayer("Player"))
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Enemy") || col.gameObject.layer == LayerMask.NameToLayer("DestroyableObstacle"))
            {
                clsHitpointsManager = col.gameObject.GetComponent<HitpointsManager>();
                clsHitpointsManager.BulletHit(bulletDamage);
                //enemyHit.killBullet(); esto es despues para la animacion de muerte
                //Instantiate (bloodImpact, this.transform.position, this.transform.rotation);
                Destroy(this.gameObject);
            }
        }

        else
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Player") || col.gameObject.layer == LayerMask.NameToLayer("DestroyableObstacle"))
            {
                clsHitpointsManager = col.gameObject.GetComponent<HitpointsManager>();
                clsHitpointsManager.BulletHit(bulletDamage);
                //enemyHit.killBullet(); esto es despues para la animacion de muerte
                //Instantiate (bloodImpact, this.transform.position, this.transform.rotation);
                Destroy(this.gameObject);
            }
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            //Instantiate (wallImpact, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }   
    }
}
