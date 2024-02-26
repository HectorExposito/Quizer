using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class Instructions : MonoBehaviour
{
    private int pageNumber;
    private int totalPages;
    [SerializeField] private Sprite[] gameGoalImages;
    [SerializeField] private TMP_Text[] gameGoalTexts;
    [SerializeField] private Sprite[] turnsImages;
    [SerializeField] private TMP_Text[] turnsTexts;
    [SerializeField] private Sprite[] squaresImages;
    [SerializeField] private TMP_Text[] squaresTexts;
    [SerializeField] private Sprite[] duelImages;
    [SerializeField] private TMP_Text[] duelsTexts;
    private Sprite[] selectedImages;
    private TMP_Text[] selectedTexts;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text pageNumberText;
    [SerializeField] private GameObject instructionPanel;

    public void OpenInstructionPanel(int option)
    {
        SelectImagesAndTexts(option);
        DeactivateAllTexts();
        pageNumber = 1;
        UpdatePanel();
        instructionPanel.SetActive(true);
    }

    private void DeactivateAllTexts()
    {
        for (int i = 0; i < gameGoalTexts.Length; i++)
        {
            gameGoalTexts[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < turnsTexts.Length; i++)
        {
            turnsTexts[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < squaresTexts.Length; i++)
        {
            squaresTexts[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < duelsTexts.Length; i++)
        {
            duelsTexts[i].gameObject.SetActive(false);
        }
    }

    private void SelectImagesAndTexts(int option)
    {
        switch (option)
        {
            case 0:
                selectedImages = gameGoalImages;
                selectedTexts = gameGoalTexts;
                totalPages = gameGoalTexts.Length;
                break;
            case 1:
                selectedImages = turnsImages;
                selectedTexts = turnsTexts;
                totalPages = turnsTexts.Length;
                break;
            case 2:
                selectedImages = squaresImages;
                selectedTexts = squaresTexts;
                totalPages = squaresTexts.Length;
                break;
            case 3:
                selectedImages = duelImages;
                selectedTexts = duelsTexts;
                totalPages = duelsTexts.Length;
                break;
        }
    }

    public void NextPage()
    {
        if (pageNumber == totalPages)
        {
            pageNumber = 1 ;
        }
        else
        {
            pageNumber++;
        }
        UpdatePanel();
    }

    private void UpdatePanel()
    {
        image.sprite = selectedImages[pageNumber - 1];
        for (int i = 0; i < selectedTexts.Length; i++)
        {
            if (pageNumber-1==i)
            {
                selectedTexts[i].gameObject.SetActive(true);
            }
            else
            {
                selectedTexts[i].gameObject.SetActive(false);
            }
        }

        if (PlayerPrefs.GetInt("Lenguage") == 1)
        {
            pageNumberText.text = "PÁGINA " + pageNumber;
        }
        else
        {
            pageNumberText.text = "PAGE " + pageNumber;
        }
        
    }

    public void PreviousPage()
    {
        if (pageNumber == 1)
        {
            pageNumber = totalPages;
        }
        else
        {
            pageNumber--;
        }
        UpdatePanel();
    }
}
