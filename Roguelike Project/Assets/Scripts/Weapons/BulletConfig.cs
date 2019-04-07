using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletConfig : MonoBehaviour {
    public Vector3 direction;
    public float bulletSpeed;
    public float bulletDamage;
    public GameObject creator;
    HitpointsManager clsHitpointsManager;
    public GameObject bloodImpact, wallImpact;
    public float bulletDuration;
    public GameObject target;
    Rigidbody2D rb;
    Vector2 moveDirection;
    // Use this for initialization
    void Start () {
        if (creator.tag == "Enemy")
        {
            target = GameObject.FindGameObjectWithTag("Player");
            //rb = GetComponent<Rigidbody2D>();
            //moveDirection = (target.transform.position - transform.position).normalized * bulletSpeed;
            //rb.velocity = new Vector2(moveDirection.x, moveDirection.y);     

        }
        Destroy(gameObject, bulletDuration);

	}

    // Update is called once per frame
    void Update()
    {

        transform.Translate(direction * bulletSpeed * Time.deltaTime);
        /*
        bulletDuration -= Time.deltaTime;
        if(bulletDuration <= 0)
        {
            Destroy (this.gameObject);
        }
        */
    }

    public void SetVals (Vector3 dir, GameObject shooterName)
    {
        direction = dir;
        creator = shooterName;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject != null)
        {
            if (creator.tag == "Player")
            {
                if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Destroyable")
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
                if (col.gameObject.tag == "Player" || col.gameObject.tag == "Destroyable")
                {
                    clsHitpointsManager = col.gameObject.GetComponent<HitpointsManager>();
                    clsHitpointsManager.BulletHit(bulletDamage);
                    //enemyHit.killBullet(); esto es despues para la animacion de muerte
                    //Instantiate (bloodImpact, this.transform.position, this.transform.rotation);
                    Destroy(this.gameObject);
                }
            }

            if (col.gameObject.tag == "Wall")
            {
                //Instantiate (wallImpact, this.transform.position, this.transform.rotation);
                Destroy(this.gameObject);
            }
        }
          
    }
}
