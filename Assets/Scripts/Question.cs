using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question
{
    private Category category;
    private string question;
    private string correctAnswer;
    private string[] wrongAnswers;

    public enum Category
    {
        HISTORY, SPORTS, SCIENCE, ART, GEOGRAPHY, ENTERTAINMENT
    }
    public Question(string category, string question, string correctAnswer, string[] wrongAnswers)
    {
        SetCategory(category);
        this.question = question;
        this.correctAnswer = correctAnswer;
        this.wrongAnswers = wrongAnswers;
    }

    private void SetCategory(string category)
    {
        switch (category)
        {
            case "Hisotria":
                this.category = Category.HISTORY;
                break;
            case "Deporte":
                this.category = Category.SPORTS;
                break;
            case "Ciencias":
                this.category = Category.SCIENCE;
                break;
            case "Geografia":
                this.category = Category.GEOGRAPHY;
                break;
            case "Arte":
                this.category = Category.ART;
                break;
            case "Entretenimineto":
                this.category = Category.ENTERTAINMENT;
                break;
        }
    }

    public Category GetCategory()
    {
        return category;
    }

    public string GetQuestion()
    {
        return question;
    }

    public string GetCorrectAnswer()
    {
        return correctAnswer;
    }

    public string[] GetWrongAnswers()
    {
        return wrongAnswers;
    }

    public string ToString()
    {
        return category + ": \n" + question + "\n" + correctAnswer + "\n" + wrongAnswers.ToString();
    }
}
