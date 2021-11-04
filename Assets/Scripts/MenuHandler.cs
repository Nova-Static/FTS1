using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;

public class MenuHandler : MonoBehaviour
{

    public void GoToSceneQuiz()
    {
        
        Debug.Log("Vuforia targets"  + VuforiaConfiguration.Instance.Vuforia.MaxSimultaneousImageTargets );
        VuforiaConfiguration.Instance.Vuforia.MaxSimultaneousImageTargets = 4;
        VuforiaApplication.Instance.Initialize();
        
        Debug.Log("Vuforia targets"  + VuforiaConfiguration.Instance.Vuforia.MaxSimultaneousImageTargets );
        SceneManager.LoadScene(1);
    }
    
    public void GoToSceneMap()
    {
        Debug.Log("Vuforia targets"  + VuforiaConfiguration.Instance.Vuforia.MaxSimultaneousImageTargets );
   
        VuforiaConfiguration.Instance.Vuforia.MaxSimultaneousImageTargets = 1;
        VuforiaApplication.Instance.Initialize();
        Debug.Log("Vuforia targets"  + VuforiaConfiguration.Instance.Vuforia.MaxSimultaneousImageTargets );
        SceneManager.LoadScene(2);
    }
}
