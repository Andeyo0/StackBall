using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager Instance;

    public int score;
    public Text scoreText;

    private void Awake()
    {
        makeSingleton();
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();

    }

    private void makeSingleton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        AddScore(0);
    }

    
    void Update()
    {
        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        }
    }

    public void AddScore(int value)
    {
        score += value;

        if (score>PlayerPrefs.GetInt("HighScore",0))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }

        scoreText.text = score.ToString();

    }

    public void ResetScore()
    {
        score = 0;
    }
    
}
