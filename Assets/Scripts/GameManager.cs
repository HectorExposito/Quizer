using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private FileManager fm;
    private List<Question>[] questions;//0->historia 1->deporte 2->ciencias 3->geografia 4->arte 5->entretenimiento

    //Panels
    [SerializeField] private GameObject questionsPanel;

    // Start is called before the first frame update
    void Start()
    {
        questions = new List<Question>[6];
        for (int i = 0; i < questions.Length; i++)
        {
            questions[i] = new List<Question>();
        }
        ReadQuestions();
    }

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

    internal void AskQuestion(Square.QuestionCategory questionCategory)
    {
        questionsPanel.SetActive(true);
        Question questionToAsk=null;
        int num;
        switch (questionCategory)
        {
            case Square.QuestionCategory.HISTORY:
                num=UnityEngine.Random.Range(0,questions[0].Count);
                questionToAsk = questions[0][num];
                break;
            case Square.QuestionCategory.SPORTS:
                num = UnityEngine.Random.Range(0, questions[0].Count);
                questionToAsk = questions[0][num];
                break;
            case Square.QuestionCategory.SCIENCE:
                num = UnityEngine.Random.Range(0, questions[0].Count);
                questionToAsk = questions[0][num];
                break;
            case Square.QuestionCategory.GEOGRAPHY:
                num = UnityEngine.Random.Range(0, questions[0].Count);
                questionToAsk = questions[0][num];
                break;
            case Square.QuestionCategory.ART:
                num = UnityEngine.Random.Range(0, questions[0].Count);
                questionToAsk = questions[0][num];
                break;
            case Square.QuestionCategory.ENTERTAINMENT:
                num = UnityEngine.Random.Range(0, questions[0].Count);
                questionToAsk = questions[0][num];
                break;
        }
        Debug.Log("Pregunta: "+questionToAsk.ToString());
    }

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
