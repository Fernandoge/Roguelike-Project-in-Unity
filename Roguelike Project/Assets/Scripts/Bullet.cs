using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public Vector3 direction;
    public float bulletSpeed;
    public float bulletDamage;
    string creator;
    EnemyAttacked enemyHit;
    public GameObject bloodImpact, wallImpact;
    public float bulletDuration;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(direction * bulletSpeed * Time.deltaTime);
        bulletDuration -= Time.deltaTime;
        if(bulletDuration <= 0)
        {
            Destroy (this.gameObject);
        }
           
	}

    public void setVals (Vector3 dir, string name)
    {
        direction = dir;
        creator = name;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            enemyHit = col.gameObject.GetComponent<EnemyAttacked>();
            enemyHit.BulletHit(bulletDamage);
            //enemyHit.killBullet(); esto es despues para la animacion de muerte
            //Instantiate (bloodImpact, this.transform.position, this.transform.rotation);
            Destroy (this.gameObject);


        }
        else if (col.gameObject.tag == "Wall")
        {
            //Instantiate (wallImpact, this.transform.position, this.transform.rotation);
            Destroy (this.gameObject);
        }
    }
}
