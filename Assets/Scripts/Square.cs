using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    [SerializeField] private Square nextSquare;
    [SerializeField] private SquareType squareType;
    [SerializeField] private QuestionCategory questionCategory;
    [SerializeField] private BaseColor baseColor;
    [SerializeField] private List<Player> playersOnSquare;
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

    public void AddPlayer(Player player)
    {
        Debug.Log("jugadores en casilla " + playersOnSquare.Count);
        playersOnSquare.Add(player);
        Debug.Log("jugadores en casilla " + playersOnSquare.Count);
    }

    public void RemovePlayer(Player player)
    {
        Debug.Log("jugadores en casilla "+playersOnSquare.Count);
        playersOnSquare.Remove(player);
        Debug.Log("jugadores en casilla " + playersOnSquare.Count);
    }
    
}
