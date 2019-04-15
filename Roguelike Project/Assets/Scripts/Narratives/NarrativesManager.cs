using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativesManager : MonoBehaviour
{
    public TextManager clsTextManager;

    private void ChangeCurrentLines(int currentLine, int endAtLine)
    {
        clsTextManager.currentLine = currentLine - 2;  // -2 to ignore first index and ChatLinesSystem() currentLine addition 
        clsTextManager.endAtLine = endAtLine - 1; // -1 to ignore first array index
    }

    private void ChangeStarterLines(int currentLine, int endAtLine, Narrative clsNarrative)                                       
    {
        clsNarrative.startLine = currentLine;
        clsNarrative.endLine = endAtLine;

    }

    private void ChangeCurrentOptionPanel(int optionTriggerLine, int optionLine)
    {
        clsTextManager.optionTriggerLine = optionTriggerLine - 1; // -1 to ignore first array index
        clsTextManager.optionLine = optionLine - 1; // -1 to ignore first array index

    }

    private void ChangeStarterOptionPanel(int optionTriggerLine, int optionLine, Narrative clsNarrative)
    {
        clsNarrative.optionTriggerLine = optionTriggerLine;
        clsNarrative.optionReadLine = optionLine;

    }
   

    //**********************************************************************************************************************************************************************//
    //**********************************************************************************************************************************************************************//

    public void ManageText(int narrativeId, int optionNumber)
    {
        switch (narrativeId)
        {
            case 0:

                //ChangeStarterLines(10, 10, clsTextManager.clsNarrative);
                if (optionNumber == 1)
                    ChangeCurrentLines(7, 7);
                else if (optionNumber == 2)
                    ChangeCurrentLines(8, 8);
                else if (optionNumber == 3)
                    ChangeCurrentLines(9, 9);
                break;

            case 1:

                ChangeCurrentOptionPanel(3, 7);
                ChangeStarterOptionPanel(1, 10, clsTextManager.clsNarrative);
                break;

            case 2:

                //ChangeStarterLines(10, 10, clsTextManager.clsNarrative);
                if (optionNumber == 1)
                    ChangeCurrentLines(7, 7);
                else if (optionNumber == 2)
                    ChangeCurrentLines(8, 8);
                else if (optionNumber == 3)
                    ChangeCurrentLines(9, 9);
                break;

            case 3:

                ChangeCurrentOptionPanel(3, 7);
                ChangeStarterOptionPanel(1, 10, clsTextManager.clsNarrative);
                break;

            case 4:

                //ChangeStarterLines(10, 10, clsTextManager.clsNarrative);
                if (optionNumber == 1)
                    ChangeCurrentLines(7, 7);
                else if (optionNumber == 2)
                    ChangeCurrentLines(8, 8);
                else if (optionNumber == 3)
                    ChangeCurrentLines(9, 9);
                break;

            case 5:

                ChangeCurrentOptionPanel(3, 7);
                ChangeStarterOptionPanel(1, 10, clsTextManager.clsNarrative);
                break;

            case 6:

                //ChangeStarterLines(10, 10, clsTextManager.clsNarrative);
                if (optionNumber == 1)
                    ChangeCurrentLines(7, 7);
                else if (optionNumber == 2)
                    ChangeCurrentLines(8, 8);
                else if (optionNumber == 3)
                    ChangeCurrentLines(9, 9);
                break;

            case 7:

                ChangeCurrentOptionPanel(3, 7);
                ChangeStarterOptionPanel(1, 10, clsTextManager.clsNarrative);
                break;

            case 8:

                //ChangeStarterLines(10, 10, clsTextManager.clsNarrative);
                if (optionNumber == 1)
                    ChangeCurrentLines(7, 7);
                else if (optionNumber == 2)
                    ChangeCurrentLines(8, 8);
                else if (optionNumber == 3)
                    ChangeCurrentLines(9, 9);
                break;

            case 9:

                ChangeCurrentOptionPanel(3, 7);
                ChangeStarterOptionPanel(1, 10, clsTextManager.clsNarrative);
                break;

            case 10:

                //ChangeStarterLines(10, 10, clsTextManager.clsNarrative);
                if (optionNumber == 1)
                    ChangeCurrentLines(7, 7);
                else if (optionNumber == 2)
                    ChangeCurrentLines(8, 8);
                else if (optionNumber == 3)
                    ChangeCurrentLines(9, 9);
                break;

            case 11:

                ChangeCurrentOptionPanel(3, 7);
                ChangeStarterOptionPanel(1, 10, clsTextManager.clsNarrative);
                break;

            case 12:

                //ChangeStarterLines(10, 10, clsTextManager.clsNarrative);
                if (optionNumber == 1)
                    ChangeCurrentLines(7, 7);
                else if (optionNumber == 2)
                    ChangeCurrentLines(8, 8);
                else if (optionNumber == 3)
                    ChangeCurrentLines(9, 9);
                break;

            case 13:

                ChangeCurrentOptionPanel(3, 7);
                ChangeStarterOptionPanel(1, 10, clsTextManager.clsNarrative);
                break;

            case 14:

                //ChangeStarterLines(10, 10, clsTextManager.clsNarrative);
                if (optionNumber == 1)
                    ChangeCurrentLines(7, 7);
                else if (optionNumber == 2)
                    ChangeCurrentLines(8, 8);
                else if (optionNumber == 3)
                    ChangeCurrentLines(9, 9);
                break;

            case 15:

                ChangeCurrentOptionPanel(3, 7);
                ChangeStarterOptionPanel(1, 10, clsTextManager.clsNarrative);
                break;

            case 16:

                //ChangeStarterLines(10, 10, clsTextManager.clsNarrative);
                if (optionNumber == 1)
                    ChangeCurrentLines(7, 7);
                else if (optionNumber == 2)
                    ChangeCurrentLines(8, 8);
                else if (optionNumber == 3)
                    ChangeCurrentLines(9, 9);
                break;

            case 17:

                ChangeCurrentOptionPanel(3, 7);
                ChangeStarterOptionPanel(1, 10, clsTextManager.clsNarrative);
                break;

            case 18:

                //ChangeStarterLines(10, 10, clsTextManager.clsNarrative);
                if (optionNumber == 1)
                    ChangeCurrentLines(7, 7);
                else if (optionNumber == 2)
                    ChangeCurrentLines(8, 8);
                else if (optionNumber == 3)
                    ChangeCurrentLines(9, 9);
                break;

            case 19:

                ChangeCurrentOptionPanel(3, 7);
                ChangeStarterOptionPanel(1, 10, clsTextManager.clsNarrative);
                break;

            case 20:

                //ChangeStarterLines(10, 10, clsTextManager.clsNarrative);
                if (optionNumber == 1)
                    ChangeCurrentLines(7, 7);
                else if (optionNumber == 2)
                    ChangeCurrentLines(8, 8);
                else if (optionNumber == 3)
                    ChangeCurrentLines(9, 9);
                break;

            case 21:

                ChangeCurrentOptionPanel(3, 7);
                ChangeStarterOptionPanel(1, 10, clsTextManager.clsNarrative);
                break;

            case 22:

                //ChangeStarterLines(10, 10, clsTextManager.clsNarrative);
                if (optionNumber == 1)
                    ChangeCurrentLines(7, 7);
                else if (optionNumber == 2)
                    ChangeCurrentLines(8, 8);
                else if (optionNumber == 3)
                    ChangeCurrentLines(9, 9);
                break;

            case 23:

                ChangeCurrentOptionPanel(3, 7);
                ChangeStarterOptionPanel(1, 10, clsTextManager.clsNarrative);
                break;

            case 24:

                //ChangeStarterLines(10, 10, clsTextManager.clsNarrative);
                if (optionNumber == 1)
                    ChangeCurrentLines(7, 7);
                else if (optionNumber == 2)
                    ChangeCurrentLines(8, 8);
                else if (optionNumber == 3)
                    ChangeCurrentLines(9, 9);
                break;

            case 25:

                ChangeCurrentOptionPanel(3, 7);
                ChangeStarterOptionPanel(1, 10, clsTextManager.clsNarrative);
                break;

            default:

                Debug.Log("Wrong narrative ID");
                break;
        }
    }

}
