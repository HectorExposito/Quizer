using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
}
