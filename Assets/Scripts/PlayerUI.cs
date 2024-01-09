using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject[] itemImages;
    [SerializeField] private TMP_Text savedMoneyText;
    [SerializeField] private TMP_Text cashText;
    
    public void UpdateCashText(int cash)
    {
        cashText.text = cash + " $";
    }

    public void UpdateSavedMoneyText(int savedMoney)
    {
        savedMoneyText.text = savedMoney + " $";
    }

    internal void UpdateItemList(List<Player.Item> itemsOnInventory)
    {
        foreach (Player.Item item in itemsOnInventory)
        {
            switch (item)
            {
                case Player.Item.SPORTS:
                    itemImages[0].SetActive(true);
                    break;
                case Player.Item.ART:
                    itemImages[1].SetActive(true);
                    break;
                case Player.Item.HISTORY:
                    itemImages[2].SetActive(true);
                    break;
                case Player.Item.ENTERTAINMENT:
                    itemImages[3].SetActive(true);
                    break;
                case Player.Item.SCIENCE:
                    itemImages[4].SetActive(true);
                    break;
                case Player.Item.GEOGRAPHY:
                    itemImages[5].SetActive(true);
                    break;
            }
        }
    }
}
