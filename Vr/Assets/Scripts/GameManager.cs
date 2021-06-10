using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public float roundTimerMinutes = 2;
    [SerializeField]
    public float roundTimerSeconds = 30;
    [SerializeField]
    GameObject roundStarter;
    [SerializeField]
    GameObject timerTextObj;
    [SerializeField]
    GameObject[] targetArray;

    //[HideInInspector]
    public bool isRoundActive; // made this public

    //Extra variables for the timer to allow dynamic modification
    [HideInInspector]
    public float minutes;
    [HideInInspector]
    public float seconds;

    // Start is called before the first frame update
    void Start()
    {
        // temp round start
        isRoundActive = false;
        minutes = roundTimerMinutes;
        seconds = roundTimerSeconds;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRoundActive)
        {
            //Timer stuff
            if (minutes <= 0 && seconds <= 0)
            {
                //round finished
                isRoundActive = false;
            }
            else
            {
                if (seconds <= 0)
                {
                    //Set timer values appropriately to continue countdown
                    seconds = 59;
                    minutes--;
                }
                else if (seconds > 59)//in case seconds is set to more than 59
                {
                    seconds = 59;
                }
                seconds -= Time.deltaTime;
            }

            //Conversion to remove decimals from displaying
            int convertedSeconds = (int)seconds;

            string timerText = minutes.ToString() + " : " + convertedSeconds.ToString();
            timerText = string.Format("{0}:{1:00}", minutes, convertedSeconds);

            //Set UI text to our timer 
            timerTextObj.GetComponent<TextMeshProUGUI>().text = timerText;
        }
        


    }
}
