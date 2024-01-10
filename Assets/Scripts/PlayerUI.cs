using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Sprite[] itemSprites;
    [SerializeField] private GameObject[] itemImages;
    [SerializeField] private Image[] inventoryItemImages;
    [SerializeField] private GameObject inventory;
    [SerializeField] private TMP_Text savedMoneyText;
    [SerializeField] private TMP_Text cashText;
    private bool alreadyShowingInventory;
    
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

    public void UpdateInventoryImages(List<Player.Item> itemsOnInventory)
    {
        for (int i = 0; i < 6; i++)
        {
            if (itemsOnInventory.Count > i)
            {
                switch (itemsOnInventory[i])
                {
                    case Player.Item.SPORTS:
                        inventoryItemImages[i].sprite = itemSprites[0];
                        inventoryItemImages[i].gameObject.SetActive(true);
                        break;
                    case Player.Item.ART:
                        inventoryItemImages[i].sprite = itemSprites[1];
                        inventoryItemImages[i].gameObject.SetActive(true);
                        break;
                    case Player.Item.HISTORY:
                        inventoryItemImages[i].sprite = itemSprites[2];
                        inventoryItemImages[i].gameObject.SetActive(true);
                        break;
                    case Player.Item.ENTERTAINMENT:
                        inventoryItemImages[i].sprite = itemSprites[3];
                        inventoryItemImages[i].gameObject.SetActive(true);
                        break;
                    case Player.Item.SCIENCE:
                        inventoryItemImages[i].sprite = itemSprites[4];
                        inventoryItemImages[i].gameObject.SetActive(true);
                        break;
                    case Player.Item.GEOGRAPHY:
                        inventoryItemImages[i].sprite = itemSprites[5];
                        inventoryItemImages[i].gameObject.SetActive(true);
                        break;

                }
            }
            else
            {
                inventoryItemImages[i].gameObject.SetActive(false);
            }
            
        }
    }

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            Debug.Log("aaaaaaaaaaaaaa"+hit.collider+" "+alreadyShowingInventory);
            if (hit.collider != null && hit.collider.transform == this.transform)
            {
                Debug.Log("bbbbbbbbbb");
                if (!alreadyShowingInventory)
                {
                    ShowInventory();
                }
            }
        }
    }

    public void ShowInventory()
    {
        alreadyShowingInventory = true;
        StartCoroutine(ShowInventoryCoroutine());
    }

    IEnumerator ShowInventoryCoroutine()
    {
        inventory.SetActive(true);
        yield return new WaitForSeconds(2f);
        inventory.SetActive(false);
        alreadyShowingInventory = false;
    }
}
