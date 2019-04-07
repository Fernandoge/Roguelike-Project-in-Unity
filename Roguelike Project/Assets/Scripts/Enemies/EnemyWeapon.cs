using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public float distanceToShoot;
    public float fireRate;
    public GameObject bulletSpawn, equippedWeapon;
    [SerializeField]
    private GameObject weaponBullet;
    private BulletConfig clsBulletConfig;
    public EnemyMovement clsEnemyMovement;
    float timerReset;

    // Use this for initialization
    void Start()
    {
        timerReset = fireRate;
        clsBulletConfig = weaponBullet.GetComponent<BulletConfig>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fireRate > 0)
            fireRate -= Time.deltaTime;

        if (clsEnemyMovement.distanceBetweenPlayer <= distanceToShoot && fireRate <= 0 && equippedWeapon != null && clsEnemyMovement.pursuingPlayer)
            Shoot();

    }

    public void SetWeapon(GameObject cur, string name, float fireRate, GameObject TypeOfBullet)
    {
        //changingWeapon = true;
        equippedWeapon = cur;
        //this.gun = gun;
        timerReset = fireRate;
        this.fireRate = timerReset;
        weaponBullet = TypeOfBullet;
        clsBulletConfig = weaponBullet.GetComponent<BulletConfig>();

    }

    public GameObject GetCurrentWeapon()
    {
        return equippedWeapon;
    }

    public void DropWeapon()
    {
        equippedWeapon.transform.position = this.transform.position;
        equippedWeapon.SetActive(true);
        SetWeapon(null, "", 0.5f, null);
    }


    public void Shoot()
    {
        clsBulletConfig = weaponBullet.GetComponent<BulletConfig>();
        Vector3 dir;
        dir.x = Vector2.right.x;
        dir.y = Vector2.right.y;
        dir.z = 0;
        clsBulletConfig.SetVals(dir, gameObject);
        Instantiate(weaponBullet, bulletSpawn.transform.position, this.transform.rotation);
        fireRate = timerReset;

    }
}
