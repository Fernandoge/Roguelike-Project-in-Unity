using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAttacked : MonoBehaviour
{
    public float hitpoints;
    Bullet bl;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BulletHit(float bulletDamage)
    {
        hitpoints -= bulletDamage;
        if (hitpoints <= 0)
        {
            Destroy(this.gameObject);
        }

    }
}
