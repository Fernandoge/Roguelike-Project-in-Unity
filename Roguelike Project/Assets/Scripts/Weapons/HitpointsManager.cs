using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitpointsManager : MonoBehaviour
{
    public float hitpoints;
    public GameObject objectToDestroy;

    public void BulletHit(float bulletDamage)
    {
        hitpoints -= bulletDamage;
        if (hitpoints <= 0)
        {
            if (objectToDestroy)
                Destroy(objectToDestroy);
            else
                Destroy(gameObject);
        }

    }
}
