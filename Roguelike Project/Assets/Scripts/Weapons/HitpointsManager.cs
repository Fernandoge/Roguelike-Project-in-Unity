using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitpointsManager : MonoBehaviour
{
    public float hitpoints;

    public void BulletHit(float bulletDamage)
    {
        hitpoints -= bulletDamage;
        if (hitpoints <= 0)
        {
            if (gameObject.tag == "Enemy")
            {
                transform.parent.GetChild(0).GetComponent<EnemySpriteManager>().Death();
                gameObject.SetActive(false);
            }
            else
                Destroy(gameObject);
                

            
        }

    }
}
