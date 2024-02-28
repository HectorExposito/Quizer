using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    [SerializeField] private Square nextSquare;//Points to next square the player has to move
    [SerializeField] private SquareType squareType;//Type of square (base,shop,question)
    [SerializeField] private QuestionCategory questionCategory;//Category of the question (sports, art...)
    [SerializeField] private BaseColor baseColor;//Player that has that base
    [SerializeField] private List<Player> playersOnSquare;//Players that are actually on the sqaure
    GameManager gm;
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        playersOnSquare = new List<Player>();
    }
    public enum SquareType
    {
        BASE,QUESTION,SHOP
    }

    public enum QuestionCategory
    {
        HISTORY,SPORTS,SCIENCE,ART,GEOGRAPHY,ENTERTAINMENT,NONE
    }

    public enum BaseColor
    {
        YELLOW,RED,GREEN,BLUE,NONE
    }

    //Depending of the sqauretype it will do an action
    public void SquareAction()
    {
        switch (squareType)
        {
            case SquareType.BASE:
                gm.Base();
                break;
            case SquareType.QUESTION:
                if (playersOnSquare.Count == 1)
                {
                    gm.AskQuestion(questionCategory);
                }
                else
                {
                    gm.Duel(playersOnSquare);
                }
                
                break;
            case SquareType.SHOP:
                Debug.Log("TIENDA");
                gm.Shop();
                break;
        }
    }

    internal Square GetNextSquare()
    {
        return nextSquare;
    }

    public SquareType GetSquareType()
    {
        return squareType;
    }

    public QuestionCategory GetQuestionCategory()
    {
        return questionCategory;
    }

    public BaseColor GetBaseColor()
    {
        return baseColor;
    }

    //Add a player to the list of players on the square
    public void AddPlayer(Player player)
    {
        playersOnSquare.Add(player);
    }

    //Removes a player from the list of players on the square
    public void RemovePlayer(Player player)
    {
        playersOnSquare.Remove(player);
    }
    
}
