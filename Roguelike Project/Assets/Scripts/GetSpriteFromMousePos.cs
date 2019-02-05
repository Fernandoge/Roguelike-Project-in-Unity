using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSpriteFromMousePos : MonoBehaviour {

	public Sprite spLeft, spRight, spUp, spDown;
    float mouseRatioX;
    float mouseRatioY;

    // Use this for initialization
    void Start () {
        
    }

    // Update is called once per frame
    void Update() {
        mouseRatioX = Input.mousePosition.x / Screen.width;
        mouseRatioY = Input.mousePosition.y / Screen.height;
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("x :" + mouseRatioX + "y :" + mouseRatioY);
        }
    
        

       if (mouseRatioY > 0.48)
       {
           if (mouseRatioX > 0.3 && mouseRatioX < 0.7 && mouseRatioY < 0.49)
           {
                if (mouseRatioX < 0.5)
                    GetComponent<SpriteRenderer>().sprite = spLeft;
                else if (mouseRatioX > 0.5)
                    GetComponent<SpriteRenderer>().sprite = spRight;
           }
           else if (mouseRatioX > 0.48 && mouseRatioX < 0.52 && mouseRatioY < 0.54)
           {
                GetComponent<SpriteRenderer>().sprite = spUp;
            }
           else if (mouseRatioX > 0.7)
               GetComponent<SpriteRenderer>().sprite = spRight;
           else if (mouseRatioX < 0.3)
               GetComponent<SpriteRenderer>().sprite = spLeft;
           else
               GetComponent<SpriteRenderer>().sprite = spUp;

       }

       if (mouseRatioY < 0.48)
       {
           if (mouseRatioX > 0.3 && mouseRatioX < 0.7 && mouseRatioY > 0.47)
           {
               if (mouseRatioX < 0.5)
                   GetComponent<SpriteRenderer>().sprite = spLeft;
               else if (mouseRatioX > 0.5)
                   GetComponent<SpriteRenderer>().sprite = spRight;
           }
            else if (mouseRatioX > 0.48 && mouseRatioX < 0.52 && mouseRatioY > 0.42)
            {
                GetComponent<SpriteRenderer>().sprite = spDown;
            }

            else if (mouseRatioX > 0.7)
               GetComponent<SpriteRenderer>().sprite = spRight;
           else if (mouseRatioX < 0.3)
               GetComponent<SpriteRenderer>().sprite = spLeft;
           else
               GetComponent<SpriteRenderer>().sprite = spDown;
       }

    }
}
