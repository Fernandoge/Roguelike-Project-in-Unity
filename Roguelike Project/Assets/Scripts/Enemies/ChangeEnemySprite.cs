using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEnemySprite : MonoBehaviour
{
    public EnemyMovement clsEnemyMovement;
    public Sprite up, down, left, right;
    public SpriteRenderer SprRender;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (clsEnemyMovement.patrol)
        {
            switch (clsEnemyMovement.spriteOrder)
            {
                case 0:
                    SprRender.sprite = right;
                    break;
                case 1:
                    SprRender.sprite = down;
                    break;
                case 2:
                    SprRender.sprite = left;
                    break;
                case 3:
                    SprRender.sprite = up;
                    break;
            }
        }

        if (clsEnemyMovement.pursuingPlayer)
        {
            Vector2 v = clsEnemyMovement.player.transform.position - clsEnemyMovement.transform.position;
            float a = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            int index = (int)((Mathf.Round(a / 90f) + 4) % 4); //add a modulo over 4 to get a normalized index
            Debug.Log(index);
            switch (index)
            {
                case 0:
                    SprRender.sprite = right;
                    break;
                case 1:
                    SprRender.sprite = up;
                    break;
                case 2:
                    SprRender.sprite = left;
                    break;
                case 3:
                    SprRender.sprite = down;
                    break;
            }
        }
        
    }
}
