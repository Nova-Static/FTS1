using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListManager : MonoBehaviour
{
    public GameObject List;
    public GameObject TouchUp;
    
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
}
