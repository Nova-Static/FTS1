using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListManager : MonoBehaviour
{
    public GameObject List;
    public GameObject TouchUp;
    private bool qr1seen = false;
    private bool b3selected = false;
    [SerializeField] private GameObject winbtn;
    
    
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
        check3();
    }

    private void check3()
    {
        if (qr1seen && b3selected)
        {
            //beautiful
            winbtn.SetActive(true);
        }
        else if (qr1seen && !b3selected)
        {
            b3selected = false;
        }
    }

    public void building3selected()
    {
        b3selected = true;
    }
    
    public void seeqr1()
    {
        qr1seen = true;
    }
    
    public void loseqr1()
    {
        qr1seen = false;
    }
}
