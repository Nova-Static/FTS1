using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{

    public void GoToSceneQuiz()
    {
        SceneManager.LoadScene(1);
    }
    
    public void GoToSceneMap()
    {
        SceneManager.LoadScene(2);
    }
}
