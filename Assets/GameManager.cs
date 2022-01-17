using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [Expression("Hello Bot")]
    public void Hello(Context context, Result result)
    {
        result.SendResponse("Hello User!");
    }
}

public class GameManager : MonoBehaviour
{
    OscovaBot MainBot;

    public GameObject chatPanel, textObject;
    public InputField chatBox;

    public Color UserColor, BotColor;

    List<Message> Messages = new List<Message>();

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            MainBot = new OscovaBot(); 
            OscovaBot.Logger.LogReceived += (s, o) =>
            {
                Debug.Log($"OscovaBot: {o.Log}");
            };

            MainBot.Dialogs.Add(new BotDialog());
            //MainBot.ImportWorkspace("Assets/bot-kb.west");
            MainBot.Trainer.StartTraining();

            MainBot.MainUser.ResponseReceived += (sender, evt) =>
            {
                AddMessage($"Bot: {evt.Response.Text}", MessageTypes.Bot);
            };
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    public void AddMessage(string messageText, MessageTypes messageType)
    {
        if (Messages.Count >= 25)
        {
            //Remove when too much.
            Destroy(Messages[0].TextObject.gameObject);
            Messages.Remove(Messages[0]);
        }

        var newMessage = new Message { Text = messageText };

        var newText = Instantiate(textObject, chatPanel.transform);

        newMessage.TextObject = newText.GetComponent<Text>();
        newMessage.TextObject.text = messageText;
        newMessage.TextObject.color = messageType == MessageTypes.User ? UserColor : BotColor;

        Messages.Add(newMessage);
    }

    public void SendMessageToBot()
    {
        var userMessage = chatBox.text;

        if (!string.IsNullOrEmpty(userMessage))
        {
            Debug.Log($"OscovaBot:[USER] {userMessage}");
            AddMessage($"User: {userMessage}", MessageTypes.User);
            var request = MainBot.MainUser.CreateRequest(userMessage);
            var evaluationResult = MainBot.Evaluate(request);
            evaluationResult.Invoke();

            chatBox.Select();
            chatBox.text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendMessageToBot();
        }
    }
}
