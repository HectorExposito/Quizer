using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private FileManager fm;
    private List<Question>[] questions;//0->historia 1->deporte 2->ciencias 3->geografia 4->arte 5->entretenimiento

    private int positionOfCorrectAnswer;
   //Questions panel
    [SerializeField] private GameObject questionsPanel;
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Button[] answerButtons;
    [SerializeField] private Sprite[] buttonSprites;//0->normal 1->correct 2->wrong
    [SerializeField] private TimeBar timeBar;
    [SerializeField] private float timeBeforeClosingPanel;

    // Start is called before the first frame update
    void Start()
    {
        //We initialize the array that will store all the questions
        questions = new List<Question>[6];
        for (int i = 0; i < questions.Length; i++)
        {
            questions[i] = new List<Question>();
        }
        ReadQuestions();
    }

    //It creates all the questions reading from the file that contain them
    private void ReadQuestions()
    {
        List<string[]> data=fm.ReadFile();

        foreach (string[] q in data)
        {
            Question question;
            string[] wrongAnswers = new string[3];
            for (int i = 3; i < q.Length; i++)
            {
                Debug.Log(q[i]);
                wrongAnswers[i - 3] = q[i];
            }
            question = new Question(q[0], q[1], q[2],wrongAnswers);
            SaveQuestion(question);
        }
    }

    //It saves each question on the position of the array that correspond
    private void SaveQuestion(Question question)
    {
        switch (question.GetCategory())
        {
            case Question.Category.HISTORY:
                questions[0].Add(question);
                break;
            case Question.Category.SPORTS:
                questions[1].Add(question);
                break;
            case Question.Category.SCIENCE:
                questions[2].Add(question);
                break;
            case Question.Category.GEOGRAPHY:
                questions[3].Add(question);
                break;
            case Question.Category.ART:
                questions[4].Add(question);
                break;
            case Question.Category.ENTERTAINMENT:
                questions[5].Add(question);
                break;
        }
    }
    
    //It selects the question to ask according to the category it receives and then it asks it to the player
    public void AskQuestion(Square.QuestionCategory questionCategory)
    {
        Question questionToAsk=null;
        int num;
        //It selects a random question from the array
        switch (questionCategory)
        {
            case Square.QuestionCategory.HISTORY:
                num=UnityEngine.Random.Range(0,questions[0].Count);
                //questionToAsk = questions[0][num];
                questionToAsk = questions[0][0];
                break;
            case Square.QuestionCategory.SPORTS:
                num = UnityEngine.Random.Range(0, questions[0].Count);
                //questionToAsk = questions[0][num];
                questionToAsk = questions[0][0];
                break;
            case Square.QuestionCategory.SCIENCE:
                num = UnityEngine.Random.Range(0, questions[0].Count);
                //questionToAsk = questions[0][num];
                questionToAsk = questions[0][0];
                break;
            case Square.QuestionCategory.GEOGRAPHY:
                num = UnityEngine.Random.Range(0, questions[0].Count);
                //questionToAsk = questions[0][num];
                questionToAsk = questions[0][0];
                break;
            case Square.QuestionCategory.ART:
                num = UnityEngine.Random.Range(0, questions[0].Count);
                //questionToAsk = questions[0][num];
                questionToAsk = questions[0][0];
                break;
            case Square.QuestionCategory.ENTERTAINMENT:
                num = UnityEngine.Random.Range(0, questions[0].Count);
                //questionToAsk = questions[0][num];
                questionToAsk = questions[0][0];
                break;
        }
        SetQuestionPanel(questionToAsk);
        questionsPanel.SetActive(true);
        timeBar.StartTime(this);
    }

    //It sets the components of the question panel
    private void SetQuestionPanel(Question questionToAsk)
    {
        questionText.text = questionToAsk.GetQuestion();
        positionOfCorrectAnswer = UnityEngine.Random.Range(0,4);
        int wrongAnswersUsed = 0;
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].interactable = true;
            answerButtons[i].GetComponent<Image>().sprite = buttonSprites[0];
            answerButtons[i].onClick.AddListener(()=>CheckAnswer(i));
            if (i == positionOfCorrectAnswer)
            {
                answerButtons[i].GetComponentInChildren<TMP_Text>().text = questionToAsk.GetCorrectAnswer();
            }
            else
            {
                answerButtons[i].GetComponentInChildren<TMP_Text>().text = questionToAsk.GetWrongAnswers()[wrongAnswersUsed];
                wrongAnswersUsed++;
            }
        }
    }

    //It checks if the answer gicen is correct and updates the question panel
    public void CheckAnswer(int buttonPosition)
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].interactable = false;
            if (i == positionOfCorrectAnswer)
            {
                answerButtons[i].GetComponent<Image>().sprite = buttonSprites[1];
            }
            else
            {
                answerButtons[i].GetComponent<Image>().sprite = buttonSprites[2];
            }
        }
        timeBar.StopTime();
        StartCoroutine(CloseQuestionPanel());
    }

    //Waits X seconds before closing the question panel to let the players see the correct answer
    IEnumerator CloseQuestionPanel()
    {
        yield return new WaitForSeconds(timeBeforeClosingPanel);
        questionsPanel.SetActive(false);
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }
}
