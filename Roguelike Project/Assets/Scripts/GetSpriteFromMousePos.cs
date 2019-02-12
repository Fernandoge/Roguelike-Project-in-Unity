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
    
        

       if (mouseRatioY >= 0.5)
       {
            if (mouseRatioY < 0.515)
            {
                if (mouseRatioX < 0.49)
                    GetComponent<SpriteRenderer>().sprite = spLeft;
                else if (mouseRatioX > 0.51)
                    GetComponent<SpriteRenderer>().sprite = spRight;
                else
                    GetComponent<SpriteRenderer>().sprite = spUp;
            }
            else if (mouseRatioY < 0.55)
            {
                if (mouseRatioX < 0.48)
                    GetComponent<SpriteRenderer>().sprite = spLeft;
                else if (mouseRatioX > 0.52)
                    GetComponent<SpriteRenderer>().sprite = spRight;
                else
                    GetComponent<SpriteRenderer>().sprite = spUp;
            }
            else if (mouseRatioX > 0.25 && mouseRatioX < 0.75 && mouseRatioY < 0.7)
            {
                if (mouseRatioX < 0.3)
                    GetComponent<SpriteRenderer>().sprite = spLeft;
                else if (mouseRatioX > 0.7)
                    GetComponent<SpriteRenderer>().sprite = spRight;
                else
                    GetComponent<SpriteRenderer>().sprite = spUp;

            }
            else if (mouseRatioX > 0.75)
                GetComponent<SpriteRenderer>().sprite = spRight;
            else if (mouseRatioX < 0.25)
                GetComponent<SpriteRenderer>().sprite = spLeft;
            else
                GetComponent<SpriteRenderer>().sprite = spUp;

       }

       if (mouseRatioY < 0.5)
       {
           if (mouseRatioY > 0.485)
           {
               if (mouseRatioX < 0.49)
                   GetComponent<SpriteRenderer>().sprite = spLeft;
               else if (mouseRatioX > 0.51)
                   GetComponent<SpriteRenderer>().sprite = spRight;
               else
                   GetComponent<SpriteRenderer>().sprite = spUp;
           }        
           else if (mouseRatioY > 0.45)
           {
               if (mouseRatioX < 0.48)
                   GetComponent<SpriteRenderer>().sprite = spLeft;
               else if (mouseRatioX > 0.52)
                   GetComponent<SpriteRenderer>().sprite = spRight;
               else
                   GetComponent<SpriteRenderer>().sprite = spDown;
           }
           else if (mouseRatioX > 0.25 && mouseRatioX < 0.75 && mouseRatioY > 0.3)
           {
               if (mouseRatioX < 0.3)
                   GetComponent<SpriteRenderer>().sprite = spLeft;
               else if (mouseRatioX > 0.7)
                   GetComponent<SpriteRenderer>().sprite = spRight;
               else
                   GetComponent<SpriteRenderer>().sprite = spDown;
           }
           else if (mouseRatioX > 0.75)
               GetComponent<SpriteRenderer>().sprite = spRight;
           else if (mouseRatioX < 0.25)
               GetComponent<SpriteRenderer>().sprite = spLeft;
           else
               GetComponent<SpriteRenderer>().sprite = spDown;
       }

    }
}
