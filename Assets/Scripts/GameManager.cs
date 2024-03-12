using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private FileManager fm;
    [SerializeField] private AudioPlayer audioPlayer;
    private List<Question>[] questions;//0->historia 1->deporte 2->ciencias 3->geografia 4->arte 5->entretenimiento

    private int positionOfCorrectAnswer;
    private int canBuyItem;
    #region QUESTION_PANEL_VARIABLES
    [SerializeField] private GameObject questionsPanel;
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Button[] answerButtons;
    [SerializeField] private Sprite[] buttonSprites;//0->normal 1->correct 2->wrong
    [SerializeField] private TimeBar timeBar;
    [SerializeField] private float timeBeforeClosingPanel;
    #endregion

    #region PLAYERS_VARIABLES
    [SerializeField] private Player[] allPlayers;//0->yellow 1->red 2->green 3->blue
    private Player[] playersPlaying;
    [SerializeField] private GameObject[] playersUIPanels;//0->yellow 1->red 2->green 3->blue
    private Player currentPlayer;
    #endregion

    #region GAME_VARIABLES
    public bool diceThrown;
    private bool actionFinished;
    private bool playerFailedQuestion;
    private bool questionForBuyingItem;
    private bool waitingForPlayerToSelectAnItem;
    private Player playerToCompete;
    private Player winnerPlayer;
    private const int MONEY_FOR_CORRECT_ANSWER = 50;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject noMoneyPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Image winnerImage;
    [SerializeField] private Image playerPlayingImage;
    [SerializeField] private GameObject playerPlayingPanel;
    #endregion

    #region DUEL_VARIABLES
    private bool playerToCompeteChosen;
    private int priceChosen; //-1-> No price chosen  0->Money  1->Item
    private int itemChosen;
    private bool duelQuestion;
    public const int MONEY_FROM_DUEL = 100;
    private Player.Item itemToCompete;
    [SerializeField] private GameObject chooseRivalPanel;
    [SerializeField] private GameObject choosePricePanel;
    [SerializeField] private GameObject chooseItemPanel;
    [SerializeField] private Button[] chooseRivalButtons;
    [SerializeField] private Button[] chooseItemButtons;
    [SerializeField] private Sprite[] itemsSprites;
    #endregion
    

    #region SETGAME_METHODS
    private void Awake()
    {
        SetPlayersPlaying();
    }

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

    //It creates all the questions reading from the file that contain them
    private void ReadQuestions()
    {
        List<string[]> data = fm.ReadFile();

        foreach (string[] q in data)
        {
            Question question;
            string[] wrongAnswers = new string[3];
            for (int i = 3; i < q.Length; i++)
            {
                //Debug.Log(q[1]+" "+q[i]);
                wrongAnswers[i - 3] = q[i];
            }
            question = new Question(q[0], q[1], q[2], wrongAnswers);
            SaveQuestion(question);
        }
    }

    //It saves each question on the position of the array that correspond
    private void SaveQuestion(Question question)
    {
        switch (question.GetCategory())
        {
            case Question.Category.HISTORY:
                Debug.Log("historia");
                questions[0].Add(question);
                break;
            case Question.Category.SPORTS:
                Debug.Log("dep");
                questions[1].Add(question);
                break;
            case Question.Category.SCIENCE:
                Debug.Log("cie");
                questions[2].Add(question);
                break;
            case Question.Category.GEOGRAPHY:
                Debug.Log("geo");
                questions[3].Add(question);
                break;
            case Question.Category.ART:
                Debug.Log("arte");
                questions[4].Add(question);
                break;
            case Question.Category.ENTERTAINMENT:
                Debug.Log("ent");
                questions[5].Add(question);
                break;
        }
    }
    
    private void SetPlayersPlaying()
    {
        int numberOfPlayers = 0;
        if (PlayerPrefs.GetInt("yellow") < 10)
        {
            numberOfPlayers++;
        }
        if (PlayerPrefs.GetInt("red") < 10)
        {
            numberOfPlayers++;
        }
        if (PlayerPrefs.GetInt("blue") < 10)
        {
            numberOfPlayers++;
        }
        if (PlayerPrefs.GetInt("green") < 10)
        {
            numberOfPlayers++;
        }

        playersPlaying = new Player[numberOfPlayers];

        ActivatePlayers();
    }

    private void ActivatePlayers()
    {
        int num = 0;

        if (PlayerPrefs.GetInt("yellow") < 10)
        {
            playersPlaying[num] = allPlayers[0];
            allPlayers[0].SetSprite(PlayerPrefs.GetInt("yellow"));
            num++;
        }
        else
        {
            allPlayers[0].gameObject.SetActive(false);
            playersUIPanels[0].SetActive(false);
        }

        if (PlayerPrefs.GetInt("red") < 10)
        {
            playersPlaying[num] = allPlayers[1];
            allPlayers[1].SetSprite(PlayerPrefs.GetInt("red"));
            num++;
        }
        else
        {
            allPlayers[1].gameObject.SetActive(false);
            playersUIPanels[1].SetActive(false);
        }

        if (PlayerPrefs.GetInt("green") < 10)
        {
            playersPlaying[num] = allPlayers[2];
            allPlayers[2].SetSprite(PlayerPrefs.GetInt("green"));
            num++;
        }
        else
        {
            allPlayers[2].gameObject.SetActive(false);
            playersUIPanels[2].SetActive(false);
        }

        if (PlayerPrefs.GetInt("blue") < 10)
        {
            playersPlaying[num] = allPlayers[3];
            allPlayers[3].SetSprite(PlayerPrefs.GetInt("blue"));
            num++;
        }
        else
        {
            allPlayers[3].gameObject.SetActive(false);
            playersUIPanels[3].SetActive(false);
        }
    }
    #endregion

    #region GAME_METHODS
    IEnumerator Game()
    {
        int playerTurn = 0;
        playerFailedQuestion = false;

        do
        {
            diceThrown = false;
            actionFinished = false;
            currentPlayer = playersPlaying[playerTurn];
            playerPlayingImage.sprite = currentPlayer.GetSprite();
            while (!diceThrown)
            {
                yield return new WaitForSeconds(0.1f);
            }

            currentPlayer.ThrowDice();

            while (!actionFinished)
            {
                yield return new WaitForSeconds(0.1f);
            }
            
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
        } while (!CheckIfGameEnded());
        GameOver();
    }

    private void GameOver()
    {
        winnerImage.sprite = winnerPlayer.GetSprite();
        gameOverPanel.SetActive(true);
    }

    private bool CheckIfGameEnded()
    {
        for (int i = 0; i < playersPlaying.Length; i++)
        {
            if (playersPlaying[i].GetAlreadyFinish())
            {
                winnerPlayer = playersPlaying[i];
                return true;
            }
        }

        return false;
    }

    public void ThrowDice()
    {
        Debug.Log("Tirar dado");
        diceThrown = true;
    }

    public void Base()
    {
        if (currentPlayer.CheckIfItIsOnItsBase())
        {
            audioPlayer.PlayBaseSound();
            currentPlayer.BaseAction();
        }
        Debug.Log("BASE ACTION FINISHED");
        ActionFinished();
    }

    private void ActionFinished()
    {
        actionFinished = true;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
    #endregion

    #region QUESTION_METHODS
    //It selects the question to ask according to the category it receives and then it asks it to the player
    public void AskQuestion(Square.QuestionCategory questionCategory)
    {
        playerPlayingPanel.SetActive(false);
        audioPlayer.PlayQuestionMusic();
        Question questionToAsk = null;
        int num;
        //It selects a random question from the array
        switch (questionCategory)
        {
            case Square.QuestionCategory.HISTORY:
                num = UnityEngine.Random.Range(0, questions[0].Count);
                questionToAsk = questions[0][num];
                //questionToAsk = questions[0][0];
                break;
            case Square.QuestionCategory.SPORTS:
                num = UnityEngine.Random.Range(0, questions[1].Count);
                questionToAsk = questions[1][num];
                //questionToAsk = questions[0][0];
                break;
            case Square.QuestionCategory.SCIENCE:
                num = UnityEngine.Random.Range(0, questions[2].Count);
                questionToAsk = questions[2][num];
                //questionToAsk = questions[0][0];
                break;
            case Square.QuestionCategory.GEOGRAPHY:
                num = UnityEngine.Random.Range(0, questions[3].Count);
                questionToAsk = questions[3][num];
                //questionToAsk = questions[0][0];
                break;
            case Square.QuestionCategory.ART:
                num = UnityEngine.Random.Range(0, questions[4].Count);
                questionToAsk = questions[4][num];
                //questionToAsk = questions[0][0];
                break;
            case Square.QuestionCategory.ENTERTAINMENT:
                num = UnityEngine.Random.Range(0, questions[5].Count);
                Debug.Log(num);
                questionToAsk = questions[5][num];
                //questionToAsk = questions[0][0];
                break;
        }
        SetQuestionPanel(questionToAsk);
        questionsPanel.SetActive(true);
        timeBar.StartTime(this);
    }

    //It checks if the answer given is correct and updates the question panel
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
        Debug.Log("Posicion del boton " + buttonPosition + " " + positionOfCorrectAnswer);
        if (buttonPosition != positionOfCorrectAnswer)
        {
            playerFailedQuestion = true;
            if (questionForBuyingItem)
            {
                questionForBuyingItem = false;
                canBuyItem = -1;
            }
            audioPlayer.PlayWrongAnswerMusic();
        }
        else
        {
            audioPlayer.PlayRightAnswerMusic();
            if (questionForBuyingItem)
            {
                questionForBuyingItem = false;
                canBuyItem = 1;
            }
            else if (duelQuestion)
            {
                ResolveDuel();
            }
            else
            {
                currentPlayer.ReceiveMoney(MONEY_FOR_CORRECT_ANSWER);
            }
        }
        timeBar.StopTime();
        Debug.Log("CHECK ANSWER ACTION FINISHED");
        ActionFinished();
        StartCoroutine(CloseQuestionPanel());
    }

    //It sets the components of the question panel
    private void SetQuestionPanel(Question questionToAsk)
    {
        questionText.text = questionToAsk.GetQuestion();
        positionOfCorrectAnswer = UnityEngine.Random.Range(0, 4);
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

    //Waits X seconds before closing the question panel to let the players see the correct answer
    IEnumerator CloseQuestionPanel()
    {
        yield return new WaitForSeconds(timeBeforeClosingPanel);
        questionsPanel.SetActive(false);
        audioPlayer.PlayGameMusic();
        playerPlayingPanel.SetActive(true);
    }
    #endregion

    #region DUEL_METHODS
    public void Duel(List<Player> playersOnSquare)
    {
        if (playersAbleToDuel(playersOnSquare) >= 1)
        {
            StartCoroutine(DuelCoroutine(playersOnSquare));
        }
        else
        {
            AskQuestion(currentPlayer.GetActualSquare().GetQuestionCategory());
        }
    }

    private int playersAbleToDuel(List<Player> playersOnSquare)
    {
        int playersAbleToDuel = 0;

        for (int i = 0; i < playersOnSquare.Count; i++)
        {
            if (playersOnSquare[i] != currentPlayer)
            {
                if (playersOnSquare[i].GetCash() > 0 || playersOnSquare[i].GetInventory().Count > 0)
                {
                    playersAbleToDuel++;
                }
            }
        }
        return playersAbleToDuel;
    }

    IEnumerator DuelCoroutine(List<Player> playersOnSquare)
    {
        playerToCompete = null;
        priceChosen = -1;
        duelQuestion = true;
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
            chooseRivalPanel.SetActive(true);
            while (!playerToCompeteChosen)
            {
                yield return new WaitForSeconds(0.1f);
            }
            chooseRivalPanel.SetActive(false);
        }

        if (playerToCompete.GetCash() > 0 && playerToCompete.GetInventory().Count > 0)
        {
            choosePricePanel.SetActive(true);
            while (priceChosen == -1)
            {
                yield return new WaitForSeconds(0.1f);
            }
            choosePricePanel.SetActive(false);
            if (priceChosen == 1)
            {
                StartCoroutine(CompeteForAnItemCoroutine());
            }
            else
            {
                AskQuestion(currentPlayer.GetActualSquare().GetQuestionCategory());
            }
        }
        else if (playerToCompete.GetInventory().Count == 0)
        {
            priceChosen = 0;
            AskQuestion(currentPlayer.GetActualSquare().GetQuestionCategory());
        }
        else if (playerToCompete.GetCash() == 0)
        {
            StartCoroutine(CompeteForAnItemCoroutine());
        }
    }

    private IEnumerator CompeteForAnItemCoroutine()
    {
        priceChosen = 1;
        itemChosen = -1;
        SetChooseItemPanel(playerToCompete);
        chooseItemPanel.SetActive(true);
        while (itemChosen == -1)
        {
            yield return new WaitForSeconds(0.1f);
        }
        chooseItemPanel.SetActive(false);
        AskQuestion(currentPlayer.GetActualSquare().GetQuestionCategory());
    }

    private void SetChooseItemPanel(Player playerToCompete)
    {
        for (int i = 0; i < playerToCompete.GetInventory().Count; i++)
        {
            chooseItemButtons[i].interactable = true;
            SetItemButtonImage(chooseItemButtons[i],playerToCompete.GetInventory()[i]);
        }

        for (int i = playerToCompete.GetInventory().Count; i < chooseItemButtons.Length; i++)
        {
            chooseItemButtons[i].interactable = false;
            chooseItemButtons[i].gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void SetItemButtonImage(Button button, Player.Item item)
    {
        switch (item)
        {
            case Player.Item.ART:
                button.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = itemsSprites[0];
                break;
            case Player.Item.ENTERTAINMENT:
                button.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = itemsSprites[1];
                break;
            case Player.Item.GEOGRAPHY:
                button.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = itemsSprites[2];
                break;
            case Player.Item.HISTORY:
                button.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = itemsSprites[3];
                break;
            case Player.Item.SCIENCE:
                button.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = itemsSprites[4];
                break;
            case Player.Item.SPORTS:
                button.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = itemsSprites[5];
                break;
        }
        button.gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void SelectItem(int item)
    {

        itemChosen = item;
        itemToCompete = playerToCompete.GetInventory()[item];
    }

    private void SetChooseRivalsButtons(List<Player> playersOnSquare)
    {
        int i = 0;
        if (playersOnSquare.Count == 4)
        {
            chooseRivalButtons[1].interactable = true;
            chooseRivalButtons[1].image.color = new Color(255, 255, 255, 255);
            chooseRivalButtons[1].gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 255);

            for (int j = 0; j < playersOnSquare.Count; j++)
            {
                if (playersOnSquare[j] != currentPlayer)
                {
                    chooseRivalButtons[j].gameObject.transform.GetChild(0).GetComponent<Image>().sprite = playersOnSquare[j].GetSprite();
                }
            }
        }
        else
        {
            chooseRivalButtons[1].interactable = false;
            chooseRivalButtons[1].image.color = new Color(255, 255, 255, 0);
            chooseRivalButtons[1].gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 0);

            bool firstButton = true;
            for (int j = 0; j < playersOnSquare.Count; j++)
            {
                if (playersOnSquare[j] != currentPlayer)
                {
                    if (firstButton)
                    {
                        chooseRivalButtons[0].gameObject.transform.GetChild(0).GetComponent<Image>().sprite = playersOnSquare[j].GetSprite();
                        firstButton = false;
                    }
                    else
                    {
                        chooseRivalButtons[2].gameObject.transform.GetChild(0).GetComponent<Image>().sprite = playersOnSquare[j].GetSprite();
                    }

                }
            }
        }
    }

    public void SelectPrice(int price)
    {
        if (price == 0)
        {
            priceChosen = 0;
        }
        else
        {
            priceChosen = 1;
        }

    }

    private void ResolveDuel()
    {
        if (priceChosen == 0)
        {
            if (playerToCompete.GetCash() > MONEY_FROM_DUEL)
            {
                currentPlayer.ReceiveMoney(MONEY_FROM_DUEL);
            }
            else
            {
                currentPlayer.ReceiveMoney(playerToCompete.GetCash());
            }
            playerToCompete.ReduceMoney();
        }
        else
        {
            playerToCompete.RemoveItemFromInventory(itemToCompete);
            currentPlayer.Additem(itemToCompete);
        }
        duelQuestion = false;
    }

    public void SelectPlayerToCompete(int player)
    {
        for (int i = 0; i < playersPlaying.Length; i++)
        {
            if (chooseRivalButtons[player].gameObject.transform.GetChild(0).GetComponent<Image>().sprite == playersPlaying[i].GetSprite())
            {
                playerToCompete = playersPlaying[i];
                playerToCompeteChosen = true;
                return;
            }
        }
    }
    #endregion

    #region SHOP_METHODS
    public void Shop()
    {
        if (currentPlayer.GetTotalMoney() >= 200 && currentPlayer.GetInventory().Count<6)
        {
            ShopPanel();
        }
        else
        {
            StartCoroutine(ShowNoMoneyPanel());
        }
    }

    public void ShopPanel()
    {
        shopPanel.SetActive(true);
        waitingForPlayerToSelectAnItem = true;
        //yield return new WaitForSeconds(15f);
        //if (waitingForPlayerToSelectAnItem)
        //{
        //    shopPanel.SetActive(false);
        //    Debug.Log("SHOP ACTION FINISHED");
        //    ActionFinished();
        //}
    }

    IEnumerator ShowNoMoneyPanel()
    {
        noMoneyPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        noMoneyPanel.SetActive(false);
        Debug.Log("NO MONEY ACTION FINISHED");
        ActionFinished();
    }

    public void BuyItem(int item)
    {
        waitingForPlayerToSelectAnItem = false;
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
                while (canBuyItem == 0)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                Debug.Log("cabuyitem " + canBuyItem);
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

        //ActionFinished();
    }

    #endregion

    

    
    
}
