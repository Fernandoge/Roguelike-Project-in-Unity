using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitpointsManager : MonoBehaviour
{
    public float hitpoints;
    public RoomController clsRoomController;

    public void BulletHit(float bulletDamage)
    {
        hitpoints -= bulletDamage;
        if (hitpoints <= 0)
        {
            if (gameObject.tag == "Enemy")
            {
                transform.parent.GetChild(1).GetComponent<EnemySpriteManager>().Death();
                gameObject.SetActive(false);
                if (clsRoomController != null)
                    clsRoomController.EnemyKilled();
            }
            else
            {
                Destroy(gameObject);
            }
                
        }
    }

    public void SetRoomController(RoomController clsRoomController)
    {
        this.clsRoomController = clsRoomController;
    }
}
