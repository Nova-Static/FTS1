using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vuforia;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class QuestionHandler : MonoBehaviour
{
    [SerializeField] private List<Question> questionList;
    private int currentQuestionN  = 0;
    [SerializeField] private Text questionText;
    [SerializeField] private TextMeshPro answer1, answer2;
    [SerializeField] private Image borderImage;
    private bool answer, seeRed, seeGreen = false;
    [SerializeField] private Animator dragonAnimator;
    private void Start()
    {

        NextQuestion();
    }


    private void FixedUpdate()
    {
        borderImage.color = new Color(borderImage.color.r+0.01f, borderImage.color.g+0.01f, borderImage.color.b+0.01f);
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
        answer1.text = questionList[currentQuestionN].questionOptionA;
        answer2.text = questionList[currentQuestionN].questionOptionB;
    }

    public void CheckAnswer(int answer)
    {
        if (answer == questionList[currentQuestionN].questionAnswer)
        {
            this.answer = true;
            borderImage.color = Color.green;
            dragonAnimator.SetTrigger("Correct");
        }
        else
        {

            dragonAnimator.SetTrigger("Wrong");
            borderImage.color = Color.red;
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
    
    public void GoToMainScene()
    {
        SceneManager.LoadScene(0);
    }
}
