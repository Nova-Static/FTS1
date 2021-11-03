using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class QuestionHandler : MonoBehaviour
{
    [SerializeField] private List<Question> questionList;
    private int currentQuestionN  = 0;
    [SerializeField] private Text questionText;
    private bool answer, seeRed, seeGreen = false;
    private void Start()
    {
        NextQuestion();
    }

    public void NextQuestion()
    {
        int RInt = Random.Range(0, questionList.Count);
        while (RInt == currentQuestionN)
        {
            RInt = Random.Range(0, questionList.Count);
        }

        currentQuestionN = RInt;
        Debug.Log(currentQuestionN);
        questionText.text = questionList[currentQuestionN].questionText;
    }

    public void CheckAnswer(int answer)
    {
        if (answer == questionList[currentQuestionN].questionAnswer)
        {
            this.answer = true;
        }
        NextQuestion();
    }

    public void GreenSeen()
    {
        seeGreen = true;
    }

    public void GreenLost()
    {
        seeGreen = false;
        CheckAnswer(0);
    }

    public void RedSeen()
    {
        seeRed = true;
    }

    public void RedLost()
    {
        seeRed = false;
        CheckAnswer(1);
    }
}
