using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Sprite[] itemSprites;//References the item images such as basketball or book
    [SerializeField] private GameObject[] itemImages;//References the place where itemSprites are going to be shown on the shelf
    [SerializeField] private Image[] inventoryItemImages;//References the place where itemSprites are going to be shown on the inventory
    [SerializeField] private GameObject inventory;//Player's inventory
    [SerializeField] private TMP_Text savedMoneyText;//Text that shows the money saved by the player
    [SerializeField] private TMP_Text cashText;//Text that shows the cash that the player has

    #region ANIMATION_VARIABLES
    [SerializeField] private Image[] item1Animation;
    [SerializeField] private Image[] item2Animation;
    [SerializeField] private Image[] item3Animation;
    [SerializeField] private Image[] item4Animation;
    [SerializeField] private Image[] item5Animation;
    [SerializeField] private Image[] item6Animation;
    private float rotationSpeed = 5f;
    private const float ANIMATION_DURATION = 2f;
    #endregion

    private bool alreadyShowingInventory;

    //Changes the cash text on the players UI
    public void UpdateCashText(int cash)
    {
        cashText.text = cash + " $";
    }

    //Changes the saved money text on the players UI
    public void UpdateSavedMoneyText(int savedMoney)
    {
        savedMoneyText.text = savedMoney + " $";
    }

    //Activates the images of the items that the player has saved on his shelf
    internal void UpdateItemList(Player.Item[] itemsOnBase)
    {
        for (int i = 0; i < itemsOnBase.Length; i++)
        {
            switch (itemsOnBase[i])
            {
                case Player.Item.SPORTS:
                    if (itemImages[0].activeSelf == false)
                    {
                        itemImages[0].SetActive(true);
                        DoItemSavedAnimation(item1Animation);
                    }
                    break;
                case Player.Item.ART:
                    if (itemImages[1].activeSelf == false)
                    {
                        itemImages[1].SetActive(true);
                        DoItemSavedAnimation(item2Animation);
                    }
                    break;
                case Player.Item.HISTORY:
                    if (itemImages[2].activeSelf == false)
                    {
                        itemImages[2].SetActive(true);
                        DoItemSavedAnimation(item3Animation);
                    }
                    break;
                case Player.Item.ENTERTAINMENT:
                    if (itemImages[3].activeSelf == false)
                    {
                        itemImages[3].SetActive(true);
                        DoItemSavedAnimation(item4Animation);
                    }
                    break;
                case Player.Item.SCIENCE:
                    if (itemImages[4].activeSelf == false)
                    {
                        itemImages[4].SetActive(true);
                        DoItemSavedAnimation(item5Animation);
                    }
                    break;
                case Player.Item.GEOGRAPHY:
                    if (itemImages[5].activeSelf == false)
                    {
                        itemImages[5].SetActive(true);
                        DoItemSavedAnimation(item6Animation);
                    }
                    break;
            }
        }
    }

    //Starts the animation for when an item is saved
    public void DoItemSavedAnimation(Image[] itemAnimation)
    {
        Debug.Log("animacion");
        if (itemAnimation[0].gameObject.activeSelf == false && itemAnimation[1].gameObject.activeSelf == false)
        {
            itemAnimation[0].gameObject.SetActive(true);
            itemAnimation[1].gameObject.SetActive(true);
        }
        StartCoroutine(DoItemSavedAnimationCoroutine(itemAnimation,0));

    }

    private IEnumerator DoItemSavedAnimationCoroutine(Image[] itemAnimation,float actualAnimationDuration)
    {
        Debug.Log("animacion corutina");
        itemAnimation[0].transform.Rotate(new Vector3(0, 0, -rotationSpeed));
        itemAnimation[1].transform.Rotate(new Vector3(0, 0, rotationSpeed));
        yield return new WaitForSeconds(0.05f);
        actualAnimationDuration += 0.05f;
        Debug.Log(actualAnimationDuration + " ->" + ANIMATION_DURATION);
        if (actualAnimationDuration < ANIMATION_DURATION)
        {
            StartCoroutine(DoItemSavedAnimationCoroutine(itemAnimation, actualAnimationDuration));
        }
        else
        {
            itemAnimation[0].gameObject.SetActive(false);
            itemAnimation[1].gameObject.SetActive(false);
        }
    }

    //Changes the inventory images, showing the ones that the player actually has
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
        //If the player clicks on top of a piece, it will show the inventory of that player
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            if (hit.collider != null && hit.collider.transform == this.transform)
            {
                if (!alreadyShowingInventory)
                {
                    ShowInventory();
                }
            }
        }
    }

    //Shows the inventory
    public void ShowInventory()
    {
        alreadyShowingInventory = true;
        StartCoroutine(ShowInventoryCoroutine());
    }

    IEnumerator ShowInventoryCoroutine()
    {
        if (transform.position.y<0)
        {
            inventory.transform.GetComponentInParent<RectTransform>().rotation=new Quaternion(0,0,180,0);
            inventory.transform.GetComponentInParent<RectTransform>().localPosition = new Vector3(0, -0.75f, 0);
        }
        else
        {
            inventory.transform.GetComponentInParent<RectTransform>().rotation = new Quaternion(0, 0, 0, 0);
            inventory.transform.GetComponentInParent<RectTransform>().localPosition = new Vector3(0, 0.75f, 0);
        }
        inventory.SetActive(true);
        yield return new WaitForSeconds(2f);
        inventory.SetActive(false);
        alreadyShowingInventory = false;
    }
}
