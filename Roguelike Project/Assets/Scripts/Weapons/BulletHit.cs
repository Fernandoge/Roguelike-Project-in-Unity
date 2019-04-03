using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour
{
    public float hitpoints;
    public GameObject objectToDestroy;

    public void DamageDone(float bulletDamage)
    {
        hitpoints -= bulletDamage;
        if (hitpoints <= 0)
        {
            Destroy(objectToDestroy);
        }

    }
}
