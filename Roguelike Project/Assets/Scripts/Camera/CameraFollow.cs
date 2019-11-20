using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    bool follow_player = true;

    void Update()
    {
        if (player != null && follow_player == true)
        {
            CamFollowPlayer();
        }

    }

    void CamFollowPlayer()
    {
        Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
        this.transform.position = newPos;
    }

}

