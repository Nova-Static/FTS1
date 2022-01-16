using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class UIInteractions : MonoBehaviour
{
    public GameObject AccountPanel;
    public Transform AccountBox;
    public GameObject CalendarPanel;
    public Transform CalendarBox;
    
    public CanvasGroup Background;
    
    public Canvas BGCanvas;

    public GameObject Dragon;

    public Transform AccountBtn;
    private Vector2 initialPosAccount;
    public Transform CalendarBtn;
    private Vector2 initialPosCalendar;

    public Camera MainCamera;
    public Camera VuforiaCamera;

    private void Start()
    {
        //get initial positions for account and calendar buttons
        initialPosAccount = AccountBtn.transform.localPosition;
        initialPosCalendar = CalendarBtn.transform.localPosition;
    }

    public void AccountPress()
    {
        AccountPanel.SetActive(true);
        AccountBox.localPosition = new Vector2(Screen.width, 0);
        AccountBox.LeanMoveLocalX(0, 0.5f).setEaseOutExpo().delay = 0.2f;
    }
    
    public void CalendarPress()
    {
        CalendarPanel.SetActive(true);
        CalendarBox.localPosition = new Vector2(-Screen.width, 0);
        CalendarBox.LeanMoveLocalX(0, 0.5f).setEaseOutExpo().delay = 0.2f;
    }

    //back to main screen from account panel
    public void BackPressAccount()
    {
        AccountBox.LeanMoveLocalX(Screen.width, 0.5f).setEaseInExpo().setOnComplete(AccountPanelExitComplete);
    }
    
    public void BackPressCalendar()
    {
        AccountBox.LeanMoveLocalX(-Screen.width, 0.5f).setEaseInExpo().setOnComplete(CalendarPanelExitComplete);
    }

    //runs when BackPress() animation is complete
    void AccountPanelExitComplete()
    {
        AccountPanel.SetActive(false);
    }
    
    void CalendarPanelExitComplete()
    {
        CalendarPanel.SetActive(false);
    }
    
    //switch ar camera on/off
    public void AREnable()
    {
        //enable vuforia
        if (VuforiaCamera.gameObject.activeSelf == false)
        {
            SwitchVuforia(); //enable Vuforia camera
            Dragon.SetActive(false);
            BGCanvas.gameObject.SetActive(false);
            //animations
            AccountBtn.LeanMoveLocalY(-Screen.height, 0.5f).setEaseInExpo();
            CalendarBtn.LeanMoveLocalY(-Screen.height, 0.5f).setEaseInExpo();
            Background.alpha = 1;
            Background.LeanAlpha(0, 0.5f);
        }
        else //disable vuforia
        {
            Dragon.SetActive(true);
            BGCanvas.gameObject.SetActive(true);
            //animations
            AccountBtn.LeanMoveLocalY(initialPosAccount.y, 0.5f).setEaseOutExpo();
            CalendarBtn.LeanMoveLocalY(initialPosCalendar.y, 0.5f).setEaseOutExpo();
            Background.alpha = 0;
            Background.LeanAlpha(1, 0.5f).setOnComplete(SwitchVuforia); //disable vuforia when animation complete
        }
    }

    void SwitchVuforia()
    {
        MainCamera.gameObject.SetActive(!MainCamera.gameObject.activeSelf);
        VuforiaCamera.gameObject.SetActive(!VuforiaCamera.gameObject.activeSelf);
    }
}
