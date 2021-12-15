using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class VoiceMovement : MonoBehaviour
{
    private KeywordRecognizer _keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    public Text voiceText;
    
    private void Start()
    {
        actions.Add("how are you", Good);
        actions.Add("hello", Forward);
        _keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        _keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        _keywordRecognizer.Start();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        voiceText.text = voiceText.text +"\n " +speech.text;
        actions[speech.text].Invoke();
    }

    private void Forward()
    {
        transform.Translate(1, 0 , 0);
    }

    private void Good()
    {
        voiceText.text = voiceText.text +"\n " + "Good";
    }
}
