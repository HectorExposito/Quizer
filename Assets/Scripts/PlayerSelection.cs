using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class PlayerSelection : MonoBehaviour
{
    [SerializeField] private Button[] yellowPlayerButtons;
    [SerializeField] private Button[] bluePlayerButtons;
    [SerializeField] private Button[] redPlayerButtons;
    [SerializeField] private Button[] greenPlayerButtons;
    [SerializeField] private TMP_Dropdown numberOfPlayersDropdown;
    [SerializeField] private Button startGameButton;
    private List<string[]> playersChosen; 
    private int numberOfPlayers;
    private int playerTurn;
    private bool allPlayersChosen;

    private void Start()
    {
        ChangeNumberOfPlayers();
    }

    public void ChangeNumberOfPlayers()
    {
        numberOfPlayers = int.Parse(numberOfPlayersDropdown.options[numberOfPlayersDropdown.value].text);
        playersChosen = new List<string[]>();
        playerTurn = 0;
        allPlayersChosen = false;
        startGameButton.interactable = false;
        ReactivateButtons("all");
    }

    public void SelectPlayer(string player)
    {
        string[] playerInfo = player.Split("_");
        string[] playerSameColor = PlayerWithTheSameColor(playerInfo[0]);
        if (playerSameColor != null)
        {
            playersChosen.Remove(playerSameColor);
        }
        else
        {
            if (allPlayersChosen)
            {
                ReactivateButtons(playersChosen[0][0]);
                playersChosen.Remove(playersChosen[0]);
            }
            if (playerTurn < numberOfPlayers - 1)
            {
                playerTurn++;
            }
            else
            {
                playerTurn = 0;
                allPlayersChosen = true;
                startGameButton.interactable = true;
            }
        }
        
        playersChosen.Add(playerInfo);
        Debug.Log(playersChosen.Count);
        Debug.Log("Jugadores:");
        foreach (string[] p in playersChosen)
        {
            Debug.Log(p[0]+" "+p[1]);
        }
    }

    private string[] PlayerWithTheSameColor(string playerColor)
    {
        foreach (string[] player in playersChosen)
        {
            if (player[0] == playerColor)
            {
                return player;
            }
        }
        return null;
    }

    private void ReactivateButtons(string color)
    {
        switch (color)
        {
            case "yellow":
                for (int i = 0; i < yellowPlayerButtons.Length; i++)
                {
                    yellowPlayerButtons[i].interactable = true;
                }
                break;
            case "blue":
                for (int i = 0; i < bluePlayerButtons.Length; i++)
                {
                    bluePlayerButtons[i].interactable = true;
                }
                break;
            case "green":
                for (int i = 0; i < greenPlayerButtons.Length; i++)
                {
                    greenPlayerButtons[i].interactable = true;
                }
                break;
            case "red":
                for (int i = 0; i < redPlayerButtons.Length; i++)
                {
                    redPlayerButtons[i].interactable = true;
                }
                break;
            case "all":
                for (int i = 0; i < yellowPlayerButtons.Length; i++)
                {
                    yellowPlayerButtons[i].interactable = true;
                    bluePlayerButtons[i].interactable = true;
                    greenPlayerButtons[i].interactable = true;
                    redPlayerButtons[i].interactable = true;
                }
                break;
            default:
                break;
        }
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("yellow", 10);
        PlayerPrefs.SetInt("blue", 10);
        PlayerPrefs.SetInt("green", 10);
        PlayerPrefs.SetInt("red", 10);

        foreach (string[] player in playersChosen)
        {
            switch (player[0])
            {
                case "yellow":
                    PlayerPrefs.SetInt("yellow", int.Parse(player[1]));
                    break;
                case "blue":
                    PlayerPrefs.SetInt("blue", int.Parse(player[1]));
                    break;
                case "green":
                    PlayerPrefs.SetInt("green", int.Parse(player[1]));
                    break;
                case "red":
                    PlayerPrefs.SetInt("red", int.Parse(player[1]));
                    break;
                default:
                    break;
            }
        }
        SceneManager.LoadScene("GameScene");
    }
}
