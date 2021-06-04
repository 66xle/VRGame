using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField]
    int increaseScore = 10;

    int currentScore;


    TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponentsInChildren<TextMeshProUGUI>()[1];
    }

    public void AddScore()
    {
        currentScore += increaseScore;
        scoreText.text = currentScore.ToString();
    }
}
