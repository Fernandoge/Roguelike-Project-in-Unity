using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour {
    public new string name;
    public float fireRate;
    PlayerAttack attack;
    public bool gun,onPosition;
    public GameObject weaponBullet;



    // Use this for initialization
    void Start () {
        attack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
	}
	
	// Update is called once per frame
	void Update () {
        if (onPosition && Input.GetKeyDown(KeyCode.G))
        {
            if (attack.getCur() != null)
            {
                attack.DropWeapon();

            }
            attack.SetWeapon(this.gameObject, name, fireRate, gun, weaponBullet);
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            onPosition = true;
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            onPosition = false;
    }
}
