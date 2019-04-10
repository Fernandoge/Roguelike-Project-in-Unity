using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour {
    public PlayerWeapon clsPlayerWeapon;
    public new string name;
    public float fireRate;
    public bool onPositionToPickUp;
    //public bool gun;
    public GameObject weaponBullet;
    private Sprite weaponEquippedSprite;

    void Start()
    {
        weaponEquippedSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update () {
        if (onPositionToPickUp && Input.GetKeyDown(KeyCode.G))
        {
            if (clsPlayerWeapon.GetCurrentWeapon() != null)
            {
                clsPlayerWeapon.DropWeapon();

            }
            //attack.SetWeapon(this.gameObject, name, fireRate, gun, weaponBullet);
            clsPlayerWeapon.SetWeapon(this.gameObject, name, fireRate, weaponBullet, weaponEquippedSprite);
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            onPositionToPickUp = true;
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            onPositionToPickUp = false;
    }
}
