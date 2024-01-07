using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System;

public class TimeBar : MonoBehaviour
{
    [SerializeField]private Image timeBar;//Image that will fill to represent the time
    [SerializeField] private float questionTime;//Total time to answer the question
    private float actualTime;//Time that has passed
    private bool stopTimer;
    private GameManager gm;
    
    // Update is called once per frame
    void Update()
    {
        //If the timer is stopped it won't update the time bar
        if (!stopTimer)
        {
            actualTime += Time.deltaTime;
            UpdateTimeBar();
        }
    }

    private void UpdateTimeBar()
    {
        //If the the actual time is equal or greater than the total time, the timer stops and it tells the game manager to check the answer
        //in order to close the panel and make the player fail the question
        if (actualTime >= questionTime)
        {
            stopTimer = true;
            gm.CheckAnswer(5);
        }
        timeBar.fillAmount = actualTime / questionTime;
    }

    //Initialize all the values of the time bar
    public void StartTime(GameManager gm)
    {
        Debug.Log("empieza el tiempo");
        this.gm = gm;
        actualTime = 0f;
        timeBar.fillAmount = 0f;
        stopTimer = false;
    }

    public void StopTime()
    {
        Debug.Log("para el tiempo");
        stopTimer = true;
    }
}
