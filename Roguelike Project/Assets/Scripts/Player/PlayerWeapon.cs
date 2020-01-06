using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour {
    public GameObject bulletSpawn,equippedWeapon;
    [SerializeField]
    private GameObject weaponBullet;
    private BulletConfig clsBulletConfig;
    public PlayerSpriteManager clsPlayerSpriteManager;
    
    //bool gun = false;
    public float timer;
    float timerReset;
    //readonly float weaponChange = 0.5f;
    //bool changingWeapon = false;
    

    // Use this for initialization
    void Start () {
        timerReset = timer;
	}

    // Update is called once per frame
    void Update() {
        if (timer > 0)
            timer -= Time.deltaTime;

        if (Input.GetMouseButton(0) && timer <= 0 && equippedWeapon != null)
            Shoot();

	}

    public void SetWeapon(GameObject cur, string name, float fireRate, GameObject TypeOfBullet, Sprite weaponSprite)
    {
        //changingWeapon = true;
        equippedWeapon = cur;
        //this.gun = gun;
        timerReset = fireRate;
        timer = timerReset;
        weaponBullet = TypeOfBullet;
        clsBulletConfig = weaponBullet.GetComponent<BulletConfig>();
        clsPlayerSpriteManager.UpdateWeaponSprite(weaponSprite);



    }

    public GameObject GetCurrentWeapon()
    {
        return equippedWeapon;
    }

    public void DropWeapon()
    {
        equippedWeapon.transform.position = this.transform.position;
        equippedWeapon.SetActive(true);
        SetWeapon(null,"", 0.5f, null, null);
    }


    public void Shoot()
    {
        Vector3 dir;
        dir.x = Vector2.right.x;
        dir.y = Vector2.right.y;
        dir.z = 0;
        clsBulletConfig.SetVals(dir, gameObject.layer);
        Instantiate(weaponBullet, bulletSpawn.transform.position, this.transform.rotation);
        timer = timerReset;
    }
}
