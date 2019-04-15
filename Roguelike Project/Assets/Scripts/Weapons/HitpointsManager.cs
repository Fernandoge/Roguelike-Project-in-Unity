using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitpointsManager : MonoBehaviour
{
    public float hitpoints;
    public GameObject objectToDestroy;
    public GameObject spriteManager;

    public void BulletHit(float bulletDamage)
    {
        hitpoints -= bulletDamage;
        if (hitpoints <= 0 && objectToDestroy != null)
        {
            if (objectToDestroy != null)
                objectToDestroy.SetActive(false);
            else
                gameObject.SetActive(false);

            if (gameObject.tag == "Enemy")
            {
                spriteManager.GetComponent<EnemySpriteManager>().EnemyDeathAnimation(); 
            }
                
        }

    }
}
