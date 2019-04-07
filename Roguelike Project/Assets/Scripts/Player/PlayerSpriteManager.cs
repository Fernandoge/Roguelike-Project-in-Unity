using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteManager : MonoBehaviour
{

    public Sprite spLeft, spRight, spUp, spDown;
    public SpriteRenderer SprRender;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        int index = (int)((Mathf.Round(angle / 90f) + 4) % 4); //add a modulo over 4 to get a normalized index
                                                          
        switch (index)
        {
            case 0:
                SprRender.sprite = spRight;
                break;
            case 1:
                SprRender.sprite = spUp;
                break;
            case 2:
                SprRender.sprite = spLeft;
                break;
            case 3:
                SprRender.sprite = spDown;
                break;
        }
    }
}
