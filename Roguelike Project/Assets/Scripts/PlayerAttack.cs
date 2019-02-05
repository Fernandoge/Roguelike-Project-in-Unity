using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    public GameObject bulletSpawn,bullet,curWeapon;
    bool gun = false;
    public float timer;
    float timerReset;
    float weaponChange = 0.5f;
    bool changingWeapon = false;

	// Use this for initialization
	void Start () {
        timerReset = timer;
	}

    // Update is called once per frame
    void Update() {
        if (timer > 0)
            timer -= Time.deltaTime;

        if (Input.GetMouseButton(0) && timer <= 0 && curWeapon != null)
            Shoot();

	}

    public void SetWeapon(GameObject cur, string name, float fireRate, bool gun, GameObject TypeOfBullet)
    {
        changingWeapon = true;
        curWeapon = cur;
        this.gun = gun;
        timerReset = fireRate;
        timer = timerReset;
        bullet = TypeOfBullet;
        bullet = TypeOfBullet;


    }

    public GameObject getCur()
    {
        return curWeapon;
    }

    public void DropWeapon()
    {
        curWeapon.transform.position = this.transform.position;
        curWeapon.SetActive(true);
        SetWeapon(null,"", 0.5f, false,null);
    }


    public void Shoot()
    {
        Bullet bl = bullet.GetComponent<Bullet>();
        Vector3 dir;
        dir.x = Vector2.right.x;
        dir.y = Vector2.right.y;
        dir.z = 0;
        bl.SetVals(dir, "Player");
        Instantiate(bullet, bulletSpawn.transform.position, this.transform.rotation);
        timer = timerReset;
        
    }
}
