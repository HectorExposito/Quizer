
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private const int MAX_MOVEMENT=7;//Biggest number that the player can obtain from the dice +1
    private int movement;//Number obtained from the dice
    private Collider2D playerCollider;
    [SerializeField]private Square playerBase;
    private Square nextSquare;//Next square of the board
    private Square actualSquare;//The square the player is in
    [SerializeField]private float movementDuration;//Time the player takes to move from the actual square to the next
    private bool isMoving;//Tells if the player is currently moving or not
    private bool waitingForDiceAnimation;//Tells if the player is waiting for the dice animation to end
    private bool alreadyFinish;//Tells if the player has finished the game
    private bool canThrowDice;//Tells if the player can throw the dice
    [SerializeField] private Dice dice;
    private int savedMoney;
    private int cash;
    [SerializeField] private PlayerUI playerUI;
    private Item[] itemsOnBase;
    private List<Item> itemsOnInventory;
    [SerializeField] private Sprite[] sprites;
    public enum Item
    {
        HISTORY, SPORTS, SCIENCE, ART, GEOGRAPHY, ENTERTAINMENT,NONE
    }
    // Start is called before the first frame update
    void Start()
    {
        movement = 0;
        playerCollider = GetComponent<Collider2D>();
        transform.position = playerBase.gameObject.transform.position;
        actualSquare = playerBase;
        nextSquare = playerBase.GetNextSquare();
        alreadyFinish = false;
        canThrowDice = false;
        savedMoney = 200;
        cash = 0;
        playerUI.UpdateCashText(cash);
        playerUI.UpdateSavedMoneyText(savedMoney);
        itemsOnBase = new Item[6];
        for (int i = 0; i < itemsOnBase.Length; i++)
        {
            itemsOnBase[i] = Item.NONE;
        }
        itemsOnInventory = new List<Item>();
        actualSquare.AddPlayer(this);
    }
    //Generates a random number that represents the square the player is going to move
    public void ThrowDice()
    {
        movement = UnityEngine.Random.Range(1, MAX_MOVEMENT);
        //movement = 2;
        waitingForDiceAnimation = true;
        dice.DiceAnimation(movement);
        actualSquare.RemovePlayer(this);
        MovePlayer();
    }

    internal void SetSprite(int num)
    {
        GetComponent<SpriteRenderer>().sprite = sprites[num];
    }

    //Moves the player
    private void MovePlayer()
    {
        isMoving = true;
        StartCoroutine(MovePlayerCoroutine());
    }

    IEnumerator MovePlayerCoroutine()
    {
        //Waits until the dice animation is over
        if (waitingForDiceAnimation)
        {
            yield return new WaitForSeconds(2);
            waitingForDiceAnimation = false;
        }
        //Moves the player to the next square
        float timeElapsed = 0;
        Vector3 startPosition = transform.position;
        while (timeElapsed < movementDuration)
        {
            transform.position = Vector3.Lerp(startPosition, nextSquare.gameObject.transform.position, timeElapsed / movementDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = nextSquare.gameObject.transform.position;
        //Changes the actual and next square
        actualSquare = nextSquare;
        nextSquare = nextSquare.GetNextSquare();
        movement--;
        //If the player has movements left it will continue moving, otherwise, it does the action of the square
        if (movement != 0)
        {
            MovePlayer();
        }
        else
        {
            isMoving = false;
            actualSquare.AddPlayer(this);
            actualSquare.SquareAction();
        }
    }

    public void ReceiveMoney(int money)
    {
        cash += money;
        playerUI.UpdateCashText(cash);
    }

    public  bool CheckIfItIsOnItsBase()
    {
        if (actualSquare.GetBaseColor() == playerBase.GetBaseColor())
        {
            return true;
        }
        return false;
    }

    private void SaveMoney()
    {
        savedMoney += cash;
        cash = 0;
        playerUI.UpdateCashText(cash);
        playerUI.UpdateSavedMoneyText(savedMoney);
    }

    internal void BuyItem(Item item)
    {
        itemsOnInventory.Add(item);
        int moneyToPay = 200;
        if (cash>=200)
        {
            cash -= 200;
        }
        else
        {
            cash = 0;
            moneyToPay -= cash;
            savedMoney -= moneyToPay;
        }
        playerUI.UpdateCashText(cash);
        playerUI.UpdateSavedMoneyText(savedMoney);
        playerUI.UpdateInventoryImages(itemsOnInventory);
    }

    public void BaseAction()
    {
        SaveMoney();
        SaveItems();
    }

    private void SaveItems()
    {
        foreach (Item item in itemsOnInventory)
        {
            switch (item)
            {
                case Item.SPORTS:
                    itemsOnBase[0] = item;
                    break;
                case Item.ART:
                    itemsOnBase[1] = item;
                    break;
                case Item.HISTORY:
                    itemsOnBase[2] = item;
                    break;
                case Item.ENTERTAINMENT:
                    itemsOnBase[3] = item;
                    break;
                case Item.SCIENCE:
                    itemsOnBase[4] = item;
                    break;
                case Item.GEOGRAPHY:
                    itemsOnBase[5] = item;
                    break;
            }
        }
        itemsOnInventory.Clear();
        playerUI.UpdateItemList(itemsOnBase);
        playerUI.UpdateInventoryImages(itemsOnInventory);
    }

    internal void ReduceMoney()
    {
        if (cash > GameManager.MONEY_FROM_DUEL)
        {
            cash -= GameManager.MONEY_FROM_DUEL;
        }
        else
        {
            cash = 0;
        }
        Debug.Log("REDUCE MONEY "+cash);
        playerUI.UpdateCashText(cash);
    }

    internal void RemoveItemFromInventory(Item itemToCompete)
    {
        itemsOnInventory.Remove(itemToCompete);
        playerUI.UpdateInventoryImages(itemsOnInventory);
    }

    internal void Additem(Item itemToCompete)
    {
        itemsOnInventory.Add(itemToCompete);
        playerUI.UpdateInventoryImages(itemsOnInventory);
    }

    #region GETTERS
    public Square GetActualSquare()
    {
        return actualSquare;
    }

    public Sprite GetSprite()
    {
        return GetComponent<SpriteRenderer>().sprite;
    }

    public int GetCash()
    {
        return cash;
    }

    public List<Item> GetInventory()
    {
        return itemsOnInventory;
    }

    internal int GetTotalMoney()
    {
        return cash + savedMoney;
    }

    public bool GetAlreadyFinish()
    {
        return alreadyFinish;
    }
    #endregion
}
