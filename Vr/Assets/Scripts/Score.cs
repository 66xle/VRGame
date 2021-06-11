using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public GameManager gameManager;

    #region Internal Variables

    [SerializeField] public int currentScore = 0;
    int increaseScore = 10;
    int highScore = 0;

    // Classes
    TextMeshProUGUI scoreText;
    TextMeshProUGUI highscoreText;

    #endregion

    void Start()
    {
        // Get References
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
        // If round ends check if score > highscore
        if (!gameManager.isRoundActive && currentScore > highScore)
        {
            highScore = currentScore;
            highscoreText.text = "Highscore: " + highScore.ToString();
        }
    }
}
