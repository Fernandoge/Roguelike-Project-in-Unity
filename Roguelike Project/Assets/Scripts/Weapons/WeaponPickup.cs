using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour {
    public new string name;
    public float fireRate;
    PlayerWeapon clsPlayerWeapon;
    public bool onPositionToPickUp;
    //public bool gun;
    public GameObject weaponBullet;



    // Use this for initialization
    void Start () {
        clsPlayerWeapon = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWeapon>();
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
            clsPlayerWeapon.SetWeapon(this.gameObject, name, fireRate, weaponBullet);
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
