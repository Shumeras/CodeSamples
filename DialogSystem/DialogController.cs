using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogSystem;
using System;

public class DialogController : Singleton<DialogController>
{
    [SerializeField][Range(0f, 0.2f)] private float textDelay = 0.01f;
    [SerializeField] private Color textHighlightColor = new Color(0.9f, 0.78f, 0.2f);
    [SerializeField][Range(0f, 1f)] private float inputDelay = 0.5f;

    private Dialog currentDialog = null; 
    private BaseDialogNode currentDialogNode = null;

    private bool dialogIsActive = false;
    private bool currentBoxFinished = false;
    private bool typingFinished = true;
    private float inputDelayRemaining = 0;

    private void Awake() 
    {
        EventManager.Register(EventDefinitions.DIALOG_STARTED);
        EventManager.Register(EventDefinitions.DIALOG_ENDED);

        EventManager.Subscribe(EventDefinitions.UI_CLOSED, OnUIClose);
    }

    public void StartDialog(Dialog dialog)
    {
        if(currentDialog != null)
        {
            Logger.Log("New start Dialog command recieved. There is already a dialog set.");
            return;
        }
        currentDialog = dialog;
        
        EventManager.Notify(EventDefinitions.DIALOG_STARTED);
        
        StartCoroutine(CrawlDialog());
    }

    public void StopDialog()
    {
        StopCoroutine(CrawlDialog());
        EndDialog();
    }

    private IEnumerator CrawlDialog()
    {
        currentDialogNode = currentDialog.RootNode;
        UIController.Instance.ActivateDialogUI(color: Color.white);
        dialogIsActive = true;

        while(currentDialogNode != null)
        {
            if(currentDialogNode is SimpleDialogNode)
            {
                #region CommonTasks
                currentBoxFinished = false;

                var simple = currentDialogNode as SimpleDialogNode;
                
                UIController.Instance.DialogLeftSprite = simple.leftImage;
                UIController.Instance.DialogRightSprite = simple.rightImage;
                UIController.Instance.DialogBoxFrameColor = simple.prefixColor == default(Color) ? Color.white : simple.prefixColor;
                string formatedText = FormatText(simple.prefix, simple.Text, simple.prefixColor);
                
                typingFinished = false;
                StartCoroutine(TypeDialog(formatedText));
                
                yield return new WaitUntil(() => typingFinished);
                #endregion CommonTasks

                //Individual node handlers
                #region SelectionDialogNode
                if(simple is SelectionDialogNode selection)
                {
                    yield return new WaitForSeconds(0.1f); //just a bit of delay to help accidental skips;
                    int selected = 0;
                    
                    while(!currentBoxFinished)
                    {
                        if(inputDelayRemaining <= 0)
                        {
                            if(Input.GetAxisRaw("Vertical") != 0) 
                            {
                                selected += Input.GetAxisRaw("Vertical") > 0 ? -1 : 1;
                                
                                if(selected >= selection.selections.Count) selected = 0;
                                if(selected < 0) selected = selection.selections.Count-1;
                                
                                inputDelayRemaining = inputDelay;
                            }
                            else if(Input.GetAxisRaw("Submit") > 0 || Input.GetAxisRaw("Interact") > 0)
                            {
                                currentDialogNode = selection.GetNext(selected);
                                currentBoxFinished = true;
                                inputDelayRemaining = inputDelay;
                            }

                            UIController.Instance.DialogText = AddSelections(formatedText, selection.selections.ToArray(), selected); 
                        }
                        else
                            inputDelayRemaining -= Time.deltaTime;

                        yield return new WaitForEndOfFrame();
                    }
                }
                #endregion SelectionDialogNode
                #region InputDialogNode
                else if(simple is InputDialogNode input)
                {
                    string inputString =  input.LastInput;
                    while(!currentBoxFinished)
                    {
                        
                        foreach (char c in Input.inputString)
                        {
                            if (c == '\b') // has backspace/delete been pressed?
                            {
                                if (inputString.Length != 0)
                                {
                                    inputString = inputString.Substring(0, inputString.Length - 1);
                                }
                            }
                            else
                            {
                                inputString += c;
                            }
                        }

                        UIController.Instance.DialogText = AddUserInput(formatedText, inputString);
                        
                        if(inputDelayRemaining <=0)
                        {
                            if(Input.GetAxisRaw("Submit") > 0)
                            {
                                currentDialogNode = input.GetNext(inputString);
                                currentBoxFinished = true;
                                inputDelayRemaining = inputDelay;
                            }
                        }
                        else
                            inputDelayRemaining -= Time.deltaTime;

                        yield return null;
                    }
                }
                #endregion InputDialogNode
                #region SimpleDialogNode
                else
                {
                    yield return new WaitForSeconds(0.1f); //just a bit of delay to help accidental skips;
                    while (!currentBoxFinished)
                    {
                        if(inputDelayRemaining <= 0)
                        {
                            if(Input.GetAxisRaw("Submit") > 0 || Input.GetAxisRaw("Interact") > 0)
                            {
                                currentDialogNode = simple.GetNext();
                                currentBoxFinished = true;
                                inputDelayRemaining = inputDelay;
                            }
                        }
                        else
                            inputDelayRemaining -= Time.deltaTime;
                           
                        yield return new WaitForEndOfFrame();
                    }
                }
                #endregion SimpleDialogNode
            }
            else 
            {
                //For nodes that do not interact with dialog window
                currentDialogNode = currentDialogNode.GetNext();
            }
        }

        EndDialog();
    }

    private void EndDialog()
    {
        inputDelayRemaining = 0;
        currentBoxFinished = true;
        currentDialogNode = null;
        currentDialog = null;
        dialogIsActive = false;
        EventManager.Notify(EventDefinitions.DIALOG_ENDED);
    }

    ///No rich text support yet
    ///Will probably need to extend to include rich text parser
    private IEnumerator TypeDialog(string text)
    {
        typingFinished = false;

        for(int i = text.IndexOf(":</Color> ")+11; i <= text.Length; i++)
        {
            if(!dialogIsActive) break;

            if((Input.GetAxisRaw("Submit") > 0 || Input.GetAxisRaw("Interact") > 0) && inputDelayRemaining <= 0) 
            {
                i = text.Length;
                inputDelayRemaining = inputDelay;
            }
            else
                inputDelayRemaining -= textDelay;

            UIController.Instance.DialogText = text.Substring(0, i);
            yield return new WaitForSeconds(textDelay);
        }

        typingFinished = true;
    }

    private string AddSelections(string text, string[] selections, int selected = -1)
    {
        if(selections == null) return text;

        text += System.Environment.NewLine;
        for(int i = 0; i < selections.Length; i++)
        {
            string current = "";
            if(i == selected)
                current += "<Color='#" + ColorUtility.ToHtmlStringRGB(textHighlightColor) + "'>";

            current += selections[i];

            if(i == selected)
                current += "</Color>";

            text += current + System.Environment.NewLine;
        }
        
        return text;
    }

    private string AddUserInput(string text, string input)
    {
        return  text + System.Environment.NewLine + System.Environment.NewLine 
            + "<Color='#" + ColorUtility.ToHtmlStringRGB(textHighlightColor) + "'>" + input + "_" + "</Color>";   
    }

    private string FormatText(string prefix, string text, Color color = new Color())
    {
        return (prefix.Length > 0 ? "<Color='#" + ColorUtility.ToHtmlStringRGB(color) + "'>" + prefix + ":</Color> " : "") + text;
    }

    private void OnUIClose(EventArgs args)
    {
        if(dialogIsActive)
            StopDialog();
    }


}
