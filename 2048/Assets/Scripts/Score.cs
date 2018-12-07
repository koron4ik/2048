using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public int score = 0;
    int highScore;

    public Text scoreText;
    public Text highScoreText;

    public static Score Instance;

    // Use this for initialization
    void Start () 
    {
        Instance = this;

        if (!PlayerPrefs.HasKey("HighScore"))
            PlayerPrefs.SetInt("HighScore", 0);

        scoreText = GameObject.Find("ScoreValue").GetComponent<Text>();
        highScoreText = GameObject.Find("HighScoreValue").GetComponent<Text>();

        scoreText.text = score.ToString();
        highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
    }

    public void AddScore(int value)
    {
        score += value;

        scoreText.text = score.ToString();
        if (score > highScore)
        {
            highScore = score;
            highScoreText.text = score.ToString();
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }
}
