using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
public class LenguageSelector : MonoBehaviour
{

    public void ChangeLenguage(int id)
    {
        StartCoroutine(SetLenguage(id));
    }
    IEnumerator SetLenguage(int id)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[id];
    }
}
