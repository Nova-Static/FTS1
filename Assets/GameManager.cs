using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Syn.Bot.Oscova.WorkspaceNodes;
using Syn.Workspace;
using TextSpeech;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Networking;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;

public class Message
{
    public string Text;
    public Text TextObject;
    public MessageTypes MessageType;
}

public enum MessageTypes
{
    User, Bot
}

public class BotDialog: Dialog
{
    private UIInteractions UIInteractions;
    private VoiceMovement voiceMovement;

    public BotDialog(UIInteractions uiInteractions, VoiceMovement voiceMovement)
    {
        this.UIInteractions = uiInteractions;
        this.voiceMovement = voiceMovement;
    }
    
    [Expression("Hello Bot")]
    public void Hello(Context context, Result result)
    {
        result.SendResponse("Hello User!");
    }

    [Expression("Open Account")]
    public void OpenAccount(Context context, Result result)
    {
        result.SendResponse("Opening Account");
        UIInteractions.AccountPress();
    }
    [Expression("Close Account")]
    public void CloseAccount(Context context, Result result)
    {
        result.SendResponse("closing Account");
        UIInteractions.BackPressAccount();
    }
    
    [Expression("Open Calendar")]
    public void OpenCalendar(Context context, Result result)
    {
        result.SendResponse("Opening calendar");
        UIInteractions.CalendarPress();
    }
    [Expression("Close Calendar")]
    public void CloseCalendar(Context context, Result result)
    {
        result.SendResponse("closing calendar");
        UIInteractions.BackPressCalendar();
    }
    
    [Expression("Open Camera")]
    public void OpenCamera(Context context, Result result)
    {
        result.SendResponse("Opening Camera");
        UIInteractions.AREnable();
    }
    [Expression("Close Camera")]
    public void CloseCamera(Context context, Result result)
    {
        result.SendResponse("closing Camera");
        UIInteractions.AREnable();
    }
    
    [Expression("Create Gym Schedule")]
    public void CreateGymSchedule(Context context, Result result)
    {
        result.SendResponse("creating gym schedule");
        voiceMovement.GenerateTrainingSchedule();
    }
    
    
}

public class GameManager : MonoBehaviour
{
    OscovaBot MainBot;
    GameObject dialog = null;
    [SerializeField] private UIInteractions UIInteractions;
    [SerializeField] private VoiceMovement voiceMovement;
    [SerializeField] private GameObject startListeningBtn;
    [SerializeField] private GameObject stopListeningBtn;
    
    // public GameObject chatPanel, textObject;
    // public InputField chatBox;
    //
    // public Color UserColor, BotColor;

    List<Message> Messages = new List<Message>();

    // Start is called before the first frame update
    void Start()
    {
        
        #if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        }
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
#endif
        // StartCoroutine(ReadFromStreamingAssets());
        StartBot();
        Setup(LANG_CODE);
        SpeechToText.instance.onResultCallback = OnFinalSpeechResult;
#if UNITY_ANDROID
        Debug.Log("Running on android");
        SpeechToText.instance.onPartialResultsCallback = OnPartialResult;
#endif
        TextToSpeech.instance.onStartCallBack = OnSpeakStart;
        TextToSpeech.instance.onDoneCallback = OnSpeakStop;


        
       
    }
    
    IEnumerator ReadFromStreamingAssets()
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "buddy-space.west");
        string result = "";
        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest();
            result = www.downloadHandler.text;
        }
        else
            result = System.IO.File.ReadAllText(filePath);

        // File.CreateText(Application.persistentDataPath + "/data/trainingData.txt");
        File.WriteAllText(Path.Combine(Application.persistentDataPath + "/trainingData.txt"), result);
        StartBot();
        
    }

    void StartBot()
    {
        try
        {
            // File.ReadAllText(Path.Combine(Application.persistentDataPath + "/trainingData.txt"));
            Debug.Log(File.ReadAllText(Path.Combine(Application.persistentDataPath + "/trainingData.txt")).ToString());
            MainBot = new OscovaBot(); 
            OscovaBot.Logger.LogReceived += (s, o) =>
            {
                Debug.Log($"OscovaBot: {o.Log}");
            };

            MainBot.Dialogs.Add(new BotDialog(UIInteractions, voiceMovement));
            // MainBot.ImportWorkspace(Path.Combine(Application.persistentDataPath, "data/buddy-space.west"));
            // UnityWebRequest webRequest = new UnityWebRequest(Path.Combine(Application.streamingAssetsPath, "buddy-space.west"));
            MainBot.ImportWorkspace(Path.Combine(Application.persistentDataPath + "/trainingData.txt"));
            MainBot.Trainer.StartTraining();
            // MainBot.
            // MainBot.

            MainBot.MainUser.ResponseReceived += (sender, evt) =>
            {
                StartSpeaking(evt.Response.Text);
                AddMessage($"Bot: {evt.Response.Text}", MessageTypes.Bot);
            };
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }
    // public void Import(string filePath)
    // {
    //     WorkspaceGraph workspace = new WorkspaceGraph();
    //     workspace.Environment.Add("oscova-bot", (object) this.Bot);
    //     workspace.Logger.RedirectTo(OscovaBot.Logger);
    //     workspace.Load(filePath);
    //     this.Import(workspace);
    // }
    // public void Import(WorkspaceGraph workspace)
    // {
    //     foreach (Syn.Workspace.Elements.Node node in (Collection<Syn.Workspace.Elements.Node>) workspace.Nodes)
    //     {
    //         if (node is OscovaBotNode oscovaBotNode)
    //             this.BotNodes.Add(oscovaBotNode);
    //     }
    // }
    
    public void AddMessage(string messageText, MessageTypes messageType)
    {
        if (Messages.Count >= 25)
        {
            //Remove when too much.
            Destroy(Messages[0].TextObject.gameObject);
            Messages.Remove(Messages[0]);
        }

        var newMessage = new Message { Text = messageText };

        // var newText = Instantiate(textObject, chatPanel.transform);
        //
        // newMessage.TextObject = newText.GetComponent<Text>();
        // newMessage.TextObject.text = messageText;
        // newMessage.TextObject.color = messageType == MessageTypes.User ? UserColor : BotColor;

       
        Messages.Add(newMessage);
    }
    private const string LANG_CODE = "en-US";
    


    #region Text_To_speech
    public void StartSpeaking(string message)
    {
        TextToSpeech.instance.StartSpeak(message);
    }

    public void StopSpeaking()
    {
        TextToSpeech.instance.StopSpeak();
    }

    void OnSpeakStart()
    {
        
    }

    void OnSpeakStop()
    {
        
    }

    #endregion

    #region Speech to Text

    public void StartListening()
    {
        startListeningBtn.SetActive(false); 
        stopListeningBtn.SetActive(true); 
        
        SpeechToText.instance.StartRecording();
    }

    public void StopListening()
    {
        startListeningBtn.SetActive(true); 
        stopListeningBtn.SetActive(false); 
        SpeechToText.instance.StopRecording();
    }

    void OnFinalSpeechResult(string result)
    {
        Debug.Log(result);
        SendMessageToBot(result);
    }
    
    void OnPartialResult(string result)
    {
        
    }
    #endregion
    void Setup(string code)
    {
        TextToSpeech.instance.Setting(code, 1 , 1);
        SpeechToText.instance.Setting(code);
    }
    
    public void SendMessageToBot(string userMsg)
    {
        var userMessage = userMsg;

        if (!string.IsNullOrEmpty(userMessage))
        {
            Debug.Log($"OscovaBot:[USER] {userMessage}");
            AddMessage($"User: {userMessage}", MessageTypes.User);
            var request = MainBot.MainUser.CreateRequest(userMessage);
            var evaluationResult = MainBot.Evaluate(request);
            evaluationResult.Invoke();

            // chatBox.Select();
            // chatBox.text = "";
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Return))
        // {
        //     SendMessageToBot(":");
        // }
    }
}
