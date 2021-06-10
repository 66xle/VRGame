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
    GameObject timerTextObj;

    [HideInInspector]
    public GameObject roundStarter;
    public GameObject[] targetArray;

    public AudioSource gameOver;
    bool endGame = false;

    // Game Over Delay
    float currentDuration;
    float maxDuration = 100.0f;

    //[HideInInspector]
    public bool isRoundActive; // made this public

    //Extra variables for the timer to allow dynamic modification
    [HideInInspector]
    public float minutes;
    [HideInInspector]
    public float seconds;

    void Awake()
    {
        // Choose random target to be round starter
        roundStarter = targetArray[Random.Range(0, targetArray.Length)];
    }

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
                endGame = true;

                currentDuration = 0.0f;
                maxDuration = Time.deltaTime + 3.0f;


                //round finished
                isRoundActive = false;

                // Game over sound
                gameOver.Play();

                foreach (GameObject target in targetArray)
                {
                    target.GetComponent<TargetHit>().DisableTarget();
                }
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


        if (endGame && currentDuration > maxDuration)
        {
            endGame = false;

            roundStarter = targetArray[Random.Range(0, targetArray.Length)];
            roundStarter.GetComponent<TargetHit>().EnableTarget();
        }
        else if (endGame)
        {
            currentDuration += Time.deltaTime;
        }
    }

    
}
