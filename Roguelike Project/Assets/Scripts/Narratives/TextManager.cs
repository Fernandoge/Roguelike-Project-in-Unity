using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextManager : MonoBehaviour
{
    public GameObject textBox;
    public GameObject optionsBox;
    public Text theText;
    public Text OptionText;
    public TextAsset textFile;
    public PlayerMovement player;
    public GameVariables Variables;
    public ActivateTextAtLine ATAL;
    public string[] textLines;
    public int currentLine;
    public int endAtLine;
    public int optionAtLine;
    public int optionLine;
    public int ChatNum;
    public bool isActive;
    public bool optionsIsActive;
    private bool isTyping = false;
    private bool cancelTyping = false;
    public float typeSpeed;

    public GameObject Chicorita;


    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        Variables = GameObject.Find("GameVariables").GetComponent<GameVariables>();
        if (textFile != null)
        {
            textLines = (textFile.text.Split('\n'));
        }

        if (endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }

        if (isActive)
        {
            EnableTextBox();
        }
        else
        {
            DisableTextBox();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (isActive && !optionsIsActive)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ChatLinesSystem();
            }
        }
        if (isActive && optionsIsActive)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ChoiceMade(ChatNum, 1);
                DisableOptionsBox();
                ChatLinesSystem();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChoiceMade(ChatNum, 2);
                DisableOptionsBox();
                ChatLinesSystem();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ChoiceMade(ChatNum, 3);
                DisableOptionsBox();
                ChatLinesSystem();
            }
        }
    }

    private IEnumerator isActiveWithWait()
    {
        yield return new WaitForSeconds(typeSpeed);
        isActive = false;
    }

    private IEnumerator NPCMovement()
    {
        yield return new WaitForSecondsRealtime(5);
        
    }   
     
    private IEnumerator TextScroll(string lineOfText)
    {
        int letter = 0;
        theText.text = "";
        isTyping = true;
        cancelTyping = false;
        while (isTyping && !cancelTyping && (letter < lineOfText.Length - 1))
        {
            theText.text += lineOfText[letter];
            letter += 1;
            yield return new WaitForSeconds(typeSpeed);
        }
        theText.text = lineOfText;
        isTyping = false;
        cancelTyping = false;
        if (optionAtLine == currentLine)
        {
            EnableOptionsBox();
        }
    }

    public void ChatLinesSystem()
    {
        if (!isTyping)
        {
            currentLine += 1;

            if (currentLine > endAtLine)
            {
                DisableTextBox();
            }
            else
            {
                StartCoroutine(TextScroll(textLines[currentLine]));
            }
        }

        else if (isTyping && !cancelTyping)
        {
            cancelTyping = true;
        }

    }

    public void EnableTextBox()
    {
        isActive = true;
        textBox.SetActive(true);
        player.canMove = false;
        player.myRigidbody.velocity = Vector2.zero;
        StartCoroutine(TextScroll(textLines[currentLine]));

    }

    public void DisableTextBox()
    {
        StartCoroutine(isActiveWithWait());
        textBox.SetActive(false);
        optionsBox.SetActive(false);
        player.canMove = true;
    }

    public void EnableOptionsBox()
    {
        optionsIsActive = true;
        optionsBox.SetActive(true);
        OptionText.text = textLines[optionLine];
    }

    public void DisableOptionsBox()
    {
        optionsIsActive = false;
        optionsBox.SetActive(false);
    }

    public void ReloadScript(TextAsset theText)
    {
        if (theText != null)
        {
            textLines = new string[1];
            textLines = (theText.text.Split('\n'));
        }
    }

    void ChangeLines(int cl, int el, int atal_sl)                                    //We change the startline to decide at what line the chat will begin                                                                       
    {
        currentLine = cl;                                   // currentLine must be 2 less than the wanted because the +1 addition and text starting with Element 0                                
        endAtLine = el;                                     // endAtLine must be 1 less because the text starting with Element 0    
        ATAL.startLine = atal_sl;                           // ATAL.startline must be 1 less because the text starting with Element 0
    }

    void ChangeOptionPanel(int otl, int orl)
    {
        optionAtLine = otl;
        optionLine = orl;
        ATAL.optionReadLine = orl;
        ATAL.optionTriggerLine = otl;
    }


    //**********************************************************************************************************************************************************************//
    //**********************************************************************************************************************************************************************//
    //**********************************************************************************************************************************************************************//
    //**********************************************************************************************************************************************************************//
    //**********************************************************************************************************************************************************************//
    //**********************************************************************************************************************************************************************//
    //**********************************************************************************************************************************************************************//
    //**********************************************************************************************************************************************************************//
    //**********************************************************************************************************************************************************************//




    public void ChoiceMade(int n, int c)                                                                           // n = Chat Number | c = Option choiced                        
    {
        if (n == 0) //Chicorita                                                                                                     
        {
            ATAL = GameObject.Find("Chicorita").GetComponent<ActivateTextAtLine>();     //We control the ATAL script for this NPC
            Chicorita = GameObject.Find("Chicorita");
            if (c == 1)                 //hacer switch aca
            {
                Variables.PrimeraVariable = true;
                ChangeLines(12, 14, 19);
            }
            else if (c == 2)
            {
                Variables.SegundaVariable = false;
                ChangeLines(15, 17, ATAL.startLine);
            }
        }

        if (n == 1) //otro chat
        {


        }
    }
}