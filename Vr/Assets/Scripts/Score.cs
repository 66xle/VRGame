using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField]
    int increaseScore = 10;

    int currentScore = 0;
    int highScore = 0;

    public GameManager gameManager;

    TextMeshProUGUI scoreText;
    TextMeshProUGUI highscoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponentsInChildren<TextMeshProUGUI>()[0];
        highscoreText = GetComponentsInChildren<TextMeshProUGUI>()[1];
    }

    public void AddScore()
    {
        currentScore += increaseScore;
        scoreText.text = "Score: " + currentScore.ToString();
    }
    void Update()
    {
        if (!gameManager.isRoundActive)
        {
            if (currentScore > highScore)
            {
                highScore = currentScore;
                highscoreText.text = "Highscore: " + highScore.ToString();
            }
        }
    }
}
