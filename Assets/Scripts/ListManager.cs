using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ListManager : MonoBehaviour
{
    public GameObject List;
    public GameObject TouchUp;
    private bool qr1seen = false;
    private bool qr2seen = false;
    private bool qr3seen = false;
    private bool qr4seen = false;
    private bool b1selected = false;
    private bool b2selected = false;
    private bool b3selected = false;
    private bool b4selected = false;
    [SerializeField] private GameObject groteUI;
    [SerializeField] private GameObject martiniUI;
    [SerializeField] private GameObject deraakerkUI;
    [SerializeField] private GameObject rugUI;
    
    
    public void BringListUp()
    {
        List.SetActive(true);
        TouchUp.SetActive(false);
    }
    
    public void BringListDown()
    {
        List.SetActive(false);
        TouchUp.SetActive(true);
    }



    private void Update()
    {
        check1();
        check2();
        check3();
        check4();
    }
    
    private void check1()
    {
        if (qr1seen && b1selected)
        {
            //beautiful
            deraakerkUI.SetActive(true);
        }
        if (!qr1seen)
        {
            b1selected = false;
            deraakerkUI.SetActive(false);
        }
    }
    
    private void check2()
    {
        if (qr2seen && b2selected)
        {
            //beautiful
            rugUI.SetActive(true);
        }
        if (!qr2seen)
        {
            b2selected = false;
            rugUI.SetActive(false);
        }
    }

    private void check3()
    {
        if (qr3seen && b3selected)
        {
            //beautiful
            groteUI.SetActive(true);
        }
        if (!qr3seen)
        {
            b3selected = false;
            groteUI.SetActive(false);
        }
    }
    
    private void check4()
    {
        if (qr4seen && b4selected)
        {
            //beautiful
            martiniUI.SetActive(true);
        }
        if (!qr4seen)
        {
            b4selected = false;
            martiniUI.SetActive(false);
        }
    }

    #region UIListChecks

    public void building1selected()
    {
        b1selected = true;
    }
    
    public void building2selected()
    {
        b2selected = true;
    }

    public void building3selected()
    {
        b3selected = true;
    }
    
    public void building4selected()
    {
        b4selected = true;
    }

    #endregion

    #region ARListChecks

    public void seeqr1()
    {
        qr1seen = true;
    }
    
    public void loseqr1()
    {
        qr1seen = false;
    }
    
    public void seeqr2()
    {
        qr2seen = true;
    }
    
    public void loseqr2()
    {
        qr2seen = false;
    }
    
    public void seeqr3()
    {
        qr3seen = true;
    }
    
    public void loseqr3()
    {
        qr3seen = false;
    }
    
    public void seeqr4()
    {
        qr4seen = true;
    }
    
    public void loseqr4()
    {
        qr4seen = false;
    }

    #endregion

}
