using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    DialogWindowController dialogWindow;
    [SerializeField]
    Sprite[] firstLevel, backToObey, backToEscape, toEscape1, toEscape2, toObey1, toObey2;

    void Start()
    {
        dialogWindow = GameObject.Find("Canvas").transform.Find("DialogWindow").GetComponent<DialogWindowController>();
    }

    public void ShowDialog()
    {
        dialogWindow.ShowDialog();
    }

    public void LoadDialogFirstLevel()
    {
        dialogWindow.LoadDialog(firstLevel);
    }

    public void LoadDialogBackToObey()
    {
        dialogWindow.LoadDialog(backToObey);
    }

    public void LoadDialogBackToEscape()
    {
        dialogWindow.LoadDialog(backToEscape);
    }

    public void LoadDialogToEscape(int score)
    {
        if (score == -1)
            dialogWindow.LoadDialog(toEscape1);
        else if (score == -2)
            dialogWindow.LoadDialog(toEscape2);
    }

    public void LoadDialogToObey(int score)
    {
        if (score == 1)
            dialogWindow.LoadDialog(toObey1);
        else if (score == 2)
            dialogWindow.LoadDialog(toObey2);
    }
}
