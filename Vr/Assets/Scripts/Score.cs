using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public GameManager gameManager;

    [SerializeField]
    int increaseScore = 10;
    public int currentScore = 0;
    int highScore = 0;

    TextMeshProUGUI scoreText;
    TextMeshProUGUI highscoreText;

    void Start()
    {
        scoreText = GetComponentsInChildren<TextMeshProUGUI>()[0];
        highscoreText = GetComponentsInChildren<TextMeshProUGUI>()[1];
    }

    public void AddScore()
    {
        // Increase score
        currentScore += increaseScore;
        scoreText.text = "Score: " + currentScore.ToString();
    }

    void Update()
    {        
        if (!gameManager.isRoundActive)
        {
            // If round ends check if highscore
            if (currentScore > highScore)
            {
                highScore = currentScore;
                highscoreText.text = "Highscore: " + highScore.ToString();
            }
        }
    }
}
