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
    public GameObject shootTargetText;

    public AudioSource gameOverSound;

    [HideInInspector]
    public bool endGameDelay = false;
    bool isTargetMoving = false;

    [HideInInspector]
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
                StartCoroutine(EndGame());
            }
            else if (!isTargetMoving && minutes < 1)
            {
                // Targets moves when under 1 min
          
                foreach (GameObject target in targetArray)
                {
                    target.GetComponent<TargetHit>().targetMove = true;
                }

                isTargetMoving = true;
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

    IEnumerator EndGame()
    {
        endGameDelay = true;

        //round finished
        isRoundActive = false;
        isTargetMoving = false;

        // Game over sound
        gameOverSound.Play();

        foreach (GameObject target in targetArray)
        {
            TargetHit hit = target.GetComponent<TargetHit>();

            hit.targetMove = false;
            hit.DisableTarget();
            hit.transform.position = hit.originalPosition;
        }


        // Delay
        yield return new WaitForSeconds(3f);


        endGameDelay = false;

        // Setup target to start round
        roundStarter = targetArray[Random.Range(0, targetArray.Length)];
        roundStarter.GetComponent<TargetHit>().EnableTarget();
        shootTargetText.SetActive(true);
    }
}
