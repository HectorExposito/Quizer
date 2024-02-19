using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
public class LenguageSelector : MonoBehaviour
{
    private void Awake()
    {
        ChangeLenguage(PlayerPrefs.GetInt("Lenguage"));
    }
    public void ChangeLenguage(int id)
    {
        PlayerPrefs.SetInt("Lenguage", id);
        StartCoroutine(SetLenguage(id));
    }
    IEnumerator SetLenguage(int id)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[id];
    }
}
