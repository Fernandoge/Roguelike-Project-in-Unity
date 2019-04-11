using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTextAtLine : MonoBehaviour {

    public TextAsset theText;
    public int ChatNumber;
    public int startLine;
    public int endLine;
    public int optionTriggerLine;
    public int optionReadLine;
    public TextManager theTextManager;
    public bool positionForChat;
    public bool DestroyAfterActive;
    public bool requireButtonPress;
    

	// Use this for initialization
	void Start ()
    {
        theTextManager = FindObjectOfType<TextManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (positionForChat && Input.GetKeyDown(KeyCode.E) && !theTextManager.isActive)
        {

            theTextManager.ReloadScript(theText);
            theTextManager.currentLine = startLine;
            theTextManager.endAtLine = endLine;
            theTextManager.optionAtLine = optionTriggerLine;
            theTextManager.optionLine = optionReadLine;
            theTextManager.ChatNum = ChatNumber;
            theTextManager.EnableTextBox();


            if (DestroyAfterActive)
            {
                Destroy(gameObject);
            }
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (requireButtonPress)
        {
            positionForChat = true;
        }
        else
        {
            theTextManager.ReloadScript(theText);
            theTextManager.currentLine = startLine;
            theTextManager.endAtLine = endLine;
            theTextManager.optionAtLine = optionTriggerLine;
            theTextManager.optionLine = optionReadLine;
            theTextManager.ChatNum = ChatNumber;
            theTextManager.EnableTextBox();
        }
        
        if (DestroyAfterActive)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        positionForChat = false;
    }


    public void CutsceneATAL()
    {
        theTextManager.ReloadScript(theText);
        theTextManager.currentLine = startLine;
        theTextManager.endAtLine = endLine;
        theTextManager.optionAtLine = optionTriggerLine;
        theTextManager.optionLine = optionReadLine;
        theTextManager.ChatNum = ChatNumber;
        theTextManager.EnableTextBox();
    }
}
