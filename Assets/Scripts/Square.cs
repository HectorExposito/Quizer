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
    GameManager gm;
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
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
                Debug.Log("Estas en la base " + baseColor.ToString());
                break;
            case SquareType.QUESTION:
                gm.AskQuestion(questionCategory);
                break;
            case SquareType.SHOP:
                Debug.Log("Tienda");
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

    
}
