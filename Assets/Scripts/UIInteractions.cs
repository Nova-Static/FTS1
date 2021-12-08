using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInteractions : MonoBehaviour
{
    public GameObject AccountPanel;

    public void AccountPress()
    {
        AccountPanel.SetActive(true);
    }

    public void BackPress()
    {
        AccountPanel.SetActive(false);
    }
}
