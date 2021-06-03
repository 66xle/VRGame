using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    float roundTimerMinutes = 2;
    [SerializeField]
    float roundTimerSeconds = 30;

    bool isRoundActive = false;

    [SerializeField]
    GameObject roundStarter;

    [SerializeField]
    GameObject timerTextObj;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        //Timer stuff
        if(roundTimerMinutes <= 0 && roundTimerSeconds <= 0)
        {
            //round finished
            isRoundActive = false;
        }
        else
        {
            if (roundTimerSeconds <= 0)
            {
                //Set timer values appropriately to continue countdown
                roundTimerSeconds = 59;
                roundTimerMinutes--;
            }
            else if (roundTimerSeconds > 59)//in case seconds is set to more than 59
            {
                roundTimerSeconds = 59;
            }
            roundTimerSeconds -= Time.deltaTime;
        }

        //Conversion to remove decimals from displaying
        int convertedSeconds = (int)roundTimerSeconds;
        
        string timerText = roundTimerMinutes.ToString() + " : " + convertedSeconds.ToString();

        //Set UI text to our timer 
        timerTextObj.GetComponent<TextMeshProUGUI>().text = timerText;
        




    }
}
