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

    //Players
    [SerializeField] private Player[] allPlayers;
    [SerializeField] private Player[] playersPlaying;
    private Player currentPlayer;

    //Game
    private bool diceThrown;
    private bool actionFinished;
    private bool playerFailedQuestion;
    private bool questionForBuyingItem;
    private const int MONEY_FOR_CORRECT_ANSWER = 200;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject noMoneyPanel;
    [SerializeField] private GameObject ChooseRivalPanel;
    [SerializeField] private Button[] chooseRivalButtons;

    private int canBuyItem;
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
        StartCoroutine(Game());
    }

    IEnumerator Game()
    {
        int playerTurn = 0;
        diceThrown = false;
        actionFinished = false;
        playerFailedQuestion = false;
        do
        {
            currentPlayer = playersPlaying[playerTurn];
            while (!diceThrown)
            {
                Debug.Log(diceThrown);
                yield return new WaitForSeconds(0.1f);
            }

            currentPlayer.ThrowDice();

            while (!actionFinished)
            {
                yield return new WaitForSeconds(0.1f);
            }

            diceThrown = false;
            actionFinished = false;

            if (playerFailedQuestion)
            {
                if (playerTurn == playersPlaying.Length - 1)
                {
                    playerTurn = 0;
                }
                else
                {
                    playerTurn++;
                }
                playerFailedQuestion = false;
            }
            Debug.Log("Fin juego "+CheckIfGameEnded());
        } while (!CheckIfGameEnded());
    }

    private bool CheckIfGameEnded()
    {
        int playersThatFinishedTheGame = 0;
        for (int i = 0; i < playersPlaying.Length; i++)
        {
            if (playersPlaying[i].GetAlreadyFinish())
            {
                playersThatFinishedTheGame++;
            }
        }

        if (playersThatFinishedTheGame >= playersPlaying.Length - 1)
        {
            return true;
        }
        else
        {
            return false;
        }
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

    internal void Duel(List<Player> playersOnSquare)
    {
        Player playerToCompete = null;
        if (playersOnSquare.Count == 2)
        {
            if (playersOnSquare[0] == currentPlayer)
            {
                playerToCompete = playersOnSquare[1];
            }
            else
            {
                playerToCompete = playersOnSquare[0];
            }
        }
        else
        {
            SetChooseRivalsButtons(playersOnSquare);
            ChooseRivalPanel.SetActive(true);
        }
    }

    private void SetChooseRivalsButtons(List<Player> playersOnSquare)
    {
        int i = 0;
        if (playersOnSquare.Count==3)
        {
            
        }
        else
        {
            
        }
    }

    //It sets the components of the question panel
    private void SetQuestionPanel(Question questionToAsk)
    {
        questionText.text = questionToAsk.GetQuestion();
        positionOfCorrectAnswer = UnityEngine.Random.Range(0,4);
        int wrongAnswersUsed = 0;
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int positionOfTheButton = i;
            answerButtons[i].interactable = true;
            answerButtons[i].GetComponent<Image>().sprite = buttonSprites[0];
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
        Debug.Log(buttonPosition+" "+positionOfCorrectAnswer);
        if (buttonPosition != positionOfCorrectAnswer)
        {
            playerFailedQuestion = true;
            if (questionForBuyingItem)
            {
                questionForBuyingItem = false;
                canBuyItem = -1;
            }
        }
        else
        {
            if (questionForBuyingItem)
            {
                questionForBuyingItem = false;
                canBuyItem = 1;
            }
            else
            {
                currentPlayer.ReceiveMoney(MONEY_FOR_CORRECT_ANSWER);
            }
        }
        timeBar.StopTime();
        ActionFinished();
        StartCoroutine(CloseQuestionPanel());
    }

    //Waits X seconds before closing the question panel to let the players see the correct answer
    IEnumerator CloseQuestionPanel()
    {
        yield return new WaitForSeconds(timeBeforeClosingPanel);
        questionsPanel.SetActive(false);
    }

    private void ActionFinished()
    {
        Debug.Log("ActionFinished");
        actionFinished = true;
    }

    public void Base()
    {
        if (currentPlayer.CheckIfItIsOnItsBase())
        {
            currentPlayer.BaseAction();
        }
        ActionFinished();
    }

    public void Shop()
    {
        if (currentPlayer.GetTotalMoney() >= 200)
        {
            shopPanel.SetActive(true);
        }
        else
        {
            StartCoroutine(ShowNoMoneyPanel());
        }
    }

    IEnumerator ShowNoMoneyPanel()
    {
        noMoneyPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        noMoneyPanel.SetActive(false);
        ActionFinished();
    }
    public void BuyItem(int item)
    {
        StartCoroutine(BuyItemCoroutine(item));
    }
    public IEnumerator BuyItemCoroutine(int item)
    {
        canBuyItem = 0;
        questionForBuyingItem = true;
        shopPanel.SetActive(false);
        switch (item)
        {
            case 0:
                AskQuestion(Square.QuestionCategory.SPORTS);
                while (canBuyItem==0)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                Debug.Log("cabuyitem "+canBuyItem);
                if (canBuyItem == 1)
                {
                    currentPlayer.BuyItem(Player.Item.SPORTS);
                }
                break;
            case 1:
                AskQuestion(Square.QuestionCategory.ART);
                while (canBuyItem == 0)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                if (canBuyItem == 1)
                {
                    currentPlayer.BuyItem(Player.Item.ART);
                }
                break;
            case 2:
                AskQuestion(Square.QuestionCategory.HISTORY);
                while (canBuyItem == 0)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                if (canBuyItem == 1)
                {
                    currentPlayer.BuyItem(Player.Item.HISTORY);
                }
                break;
            case 3:
                AskQuestion(Square.QuestionCategory.ENTERTAINMENT);
                while (canBuyItem == 0)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                if (canBuyItem == 1)
                {
                    currentPlayer.BuyItem(Player.Item.ENTERTAINMENT);
                }
                break;
            case 4:
                AskQuestion(Square.QuestionCategory.SCIENCE);
                while (canBuyItem == 0)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                if (canBuyItem == 1)
                {
                    currentPlayer.BuyItem(Player.Item.SCIENCE);
                }
                break;
            case 5:
                AskQuestion(Square.QuestionCategory.GEOGRAPHY);
                while (canBuyItem == 0)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                if (canBuyItem == 1)
                {
                    currentPlayer.BuyItem(Player.Item.GEOGRAPHY);
                }
                break;
        }
        canBuyItem = 0;
        
        ActionFinished();
    }

    public void ThrowDice()
    {
        Debug.Log("Tirar dado");
        diceThrown = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
