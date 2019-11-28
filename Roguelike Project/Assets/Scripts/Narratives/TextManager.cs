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
    public PlayerMovement clsPlayerMovement;
    public NarrativesManager clsNarrativesManager;
    public Narrative clsNarrative;
    //public GameVariables Variables;
    public int narrativeID;
    public string[] textLines;
    public int currentLine;
    public int endAtLine;
    public int optionTriggerLine;
    public int optionLine;
    public int timesTalked;
    public int timesResponded;
    public bool isActive;
    public bool optionsIsActive;
    private bool isTyping = false;
    private bool cancelTyping = false;
    public float typeSpeed;


    // Use this for initialization
    void Start()
    {
        //player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        //Variables = GameObject.Find("GameVariables").GetComponent<GameVariables>();
        if (textFile != null)
        {
            textLines = (textFile.text.Split('\n'));
        }

        if (endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
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
                clsNarrativesManager.ManageText(narrativeID, 1);
                DisableOptionsBox();
                ChatLinesSystem();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                clsNarrativesManager.ManageText(narrativeID, 2);
                DisableOptionsBox();
                ChatLinesSystem();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                clsNarrativesManager.ManageText(narrativeID, 3);
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
        if (optionTriggerLine == currentLine)
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
                clsNarrative.timesTalked += 1;
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
        clsPlayerMovement.canMove = false;
        clsPlayerMovement.rigidbody.velocity = Vector2.zero;
        StartCoroutine(TextScroll(textLines[currentLine]));

    }

    public void DisableTextBox()
    {
        StartCoroutine(isActiveWithWait());
        textBox.SetActive(false);
        optionsBox.SetActive(false);
        clsPlayerMovement.canMove = true;
        clsNarrative.SprRender.sprite = clsNarrative.nonTalkingSprite;
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
        clsNarrative.timesResponded += 1;
    }

    public void ReloadScript(TextAsset theText)
    {
        if (theText != null)
        {
            textLines = new string[1];
            textLines = (theText.text.Split('\n'));
        }
    }

    public void GetNarrativeValues()
    {
        if (clsNarrative != null)
        {
            ReloadScript(clsNarrative.theText);
            narrativeID = clsNarrative.narrativeID;
            currentLine = clsNarrative.startLine - 1;
            endAtLine = clsNarrative.endLine - 1;
            optionTriggerLine = clsNarrative.optionTriggerLine - 1;
            optionLine = clsNarrative.optionReadLine - 1;
            timesTalked = clsNarrative.timesTalked;
            timesResponded = clsNarrative.timesResponded;
            EnableTextBox();
        }
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

}